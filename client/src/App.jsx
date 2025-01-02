import React from 'react'
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom'
import './App.css'
import Navbar from './components/Navbar'
import Home from './pages/Home'
import Login from './pages/Login'
import Register from './pages/Register'
import EmailConfirm from './pages/EmailConfirm'
import ProtectedRoute from './configs/ProtectedRoute'
import CourseDetail from './pages/CourseDetail'
import Courses from './pages/Courses'
import CreateCourse from './pages/CreateCourse'
import Order from './pages/Order'
import Profile from './pages/Profile'
import InstructorRoute from './configs/InstructorRoute'
import { CartProvider } from './contexts/CartContext'
import Payment from './pages/Payment'
import Footer from './components/Footer'
import PaymentCallback from './pages/PaymentCallback'

const App = () => {

  return (
    <div className="bg-slate-50 min-h-screen flex flex-col justify-between">
      <Router>
        <CartProvider>
        <Navbar/>
        <Routes>
          <Route path="/" element={<Home/>}/>
          <Route path="/login" element={<Login/>}/>
          <Route path="/register" element={<Register/>}/>
          <Route path='/email-confirm' element={<EmailConfirm/>}/>
          <Route path='/course/:id' element={<CourseDetail/>}/>
          <Route path='/courses' element={<Courses/>}/>
          <Route element={<ProtectedRoute/>}>
            <Route path='/basket' element={<Order/>}/>
            <Route path='/basket/:id' element={<Payment/>}/>
            <Route path='/profile' element={<Profile/>}/>
            <Route path="/payment-result" element={<PaymentCallback/>} />
          </Route>
          <Route element={<InstructorRoute/>}>
            <Route path='/create-course' element={<CreateCourse/>}/>
          </Route>
          <Route path="*" element={<Home/>} />
        </Routes>
        <Footer/>
        </CartProvider>
      </Router>
    </div>
  )
}

export default App
