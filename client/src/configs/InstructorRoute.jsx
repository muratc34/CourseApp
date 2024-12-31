import React from 'react'
import { Navigate, Outlet } from 'react-router-dom';

const InstructorRoute = () => {
    const roles = ["instructor", "admin"]
    const user = JSON.parse(localStorage.getItem("user"));
    if(!user)
    {
      return <Navigate to="/login" />
    }
    if(!user.roles ||user.roles == 0)
    {
      return <Navigate to="/email-confirm"/>
    }
    if(!roles.some(role => user.roles.includes(role)))
    {
        return <Navigate to="/"/>
    }
  
    return <Outlet />
}

export default InstructorRoute