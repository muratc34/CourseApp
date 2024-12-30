import React from 'react'
import { Navigate } from 'react-router-dom';
import { useAuth } from './AuthProvider';

const ProtectedRoute = ({ roles = [], redirectPath , noRoleRedirectPath }) => {
    const {isAuthenticated} = useAuth();

    if (!isAuthenticated) {
        return <Navigate to={redirectPath} replace />;
    }

    if (!user.role) {
        return <Navigate to={noRoleRedirectPath} replace />;
    }
    
    if (roles.length > 0 && !roles.includes(user.role)) {
        return <Navigate to="/" replace />;
    }

    return children;
}

export default ProtectedRoute;