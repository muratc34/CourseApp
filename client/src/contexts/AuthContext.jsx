import React, { createContext, useContext, useEffect, useState } from 'react'
import authApi from '../services/modules/authApi';
import userApi from '../services/modules/userApi'
import { jwtDecode } from 'jwt-decode';

const AuthContext = createContext();

export const useAuth = () => useContext(AuthContext);

const AuthProvider = ({children}) => {
    const [user, setUser] = useState(null);
    const [isAuthenticated, setIsAuthenticated] = useState(false);

    useEffect(() => {
        const token = localStorage.getItem('token');
        if (token && !isTokenExpired(token)) {
            initializeUser(token);
        }
    }, []);

    const login = async (email, password) => {
        await authApi.login({email, password})
        .then(async (response) => {
            const { token, refreshToken } = response.data;
            saveTokens(token, refreshToken);
            await initializeUser(token);
        }).catch(err => {
            console.error(err);
            throw err;
        });
    }
    const logout = () => {
        clearTokens();
    };
    const getAuthToken = async () => {
        const token = localStorage.getItem('token');
        const isExpired = isTokenExpired(token);
        if (isExpired) {
            await refreshAuthToken();
        }
        return localStorage.getItem('token');
    };

    const initializeUser = async (token) => {
        const decoded = jwtDecode(token);
        console.log(decoded);
        const userId = decoded.sub;
        await userApi.getUserById(userId)
        .then(response => {
            setUser(response.data);
            setIsAuthenticated(true);
        }).catch(error => {
            console.error('Error initializing user:', error);
            logout();
        })  
    };
    const refreshAuthToken = async () => {
        const refreshToken = localStorage.getItem('refreshToken');
        if (!refreshToken) {
            logout();
            return;
        }

        await authApi.createTokenByRefreshToken({ refreshToken })
        .then(async (response) => {
            const { token, refreshToken } = response.data;
            saveTokens(token, refreshToken);
            await initializeUser(token);
        }).catch(err =>{
            console.error('Token refresh error:', err);
        logout();
        })
    };
    const isTokenExpired = (token) => {
        if (!token) return true;

        try {
            const { exp } = jwtDecode(token);
            const now = Date.now() / 1000;
            return exp < now; 
        } catch (error) {
            console.error('Token decode error:', error);
            return true;
        }
    };
    const saveTokens = (token, refreshToken, ) => {
        localStorage.setItem('token', token);
        localStorage.setItem('refreshToken', refreshToken);
        setIsAuthenticated(true);
    };
    const clearTokens = () => {
        localStorage.removeItem('token');
        localStorage.removeItem('refreshToken');
        setUser(null);
        setIsAuthenticated(false);
    };

    return (
        <AuthContext.Provider value={{user, isAuthenticated, login, logout, getAuthToken}}>
            {children}
        </AuthContext.Provider>
    )
}

export default AuthProvider