import React, { createContext, useContext, useEffect, useState } from 'react'
import authApi from '../services/modules/authApi';
import userApi from '../services/modules/userApi'
import { jwtDecode } from 'jwt-decode';

const AuthContext = createContext();

export const useAuth = () => useContext(AuthContext);

const AuthProvider = ({children}) => {
    const [user, setUser] = useState(JSON.parse(localStorage.getItem("user")));

    useEffect(() => {
        const token = localStorage.getItem('token');
        if (token && !isTokenExpired(token)) {
            initializeUser(token);
        } else{
            refreshAuthToken()
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
    const register = async(data) => {
        await userApi.register(data)
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
        const roles = decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];
        const normalizedRoles = Array.isArray(roles) ? roles : [roles];
        const userId = decoded.sub;
        await userApi.getUserById(userId)
        .then(response => {
            setUser({ ...response.data, roles: normalizedRoles });
            
            localStorage.setItem('user', JSON.stringify({ ...response.data, roles: normalizedRoles })); 
        }).catch(async (error) => {
            console.error('Error initializing user:', error);
            await getAuthToken();
        }) 
        
    };
    const refreshAuthToken = async () => {
        const refreshToken = localStorage.getItem('refreshToken');
        if (!refreshToken) {
            logout();
            return;
        }

        await authApi.createTokenByRefreshToken({ token: refreshToken })
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
    };
    const clearTokens = () => {
        localStorage.removeItem('token');
        localStorage.removeItem('refreshToken');
        localStorage.removeItem('user');
        setUser(null);
    };

    return (
        <AuthContext.Provider value={{user, login, logout, register, getAuthToken, refreshAuthToken}}>
            {children}
        </AuthContext.Provider>
    )
}

export default AuthProvider