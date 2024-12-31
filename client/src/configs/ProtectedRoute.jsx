import React from 'react'
import { Navigate, Outlet } from 'react-router-dom';

const ProtectedRoute = () => {
  const user = JSON.parse(localStorage.getItem("user"));
  if(!user)
  {
    return <Navigate to="/login" />
  }
  if(!user.roles ||user.roles == 0)
  {
    return <Navigate to="/email-confirm"/>
  }

  return <Outlet />
};

export default ProtectedRoute;