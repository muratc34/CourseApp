import React, { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { MdMenu, MdClose } from "react-icons/md";
import { PiShoppingCartSimpleThin } from "react-icons/pi";
import { useAuth } from "../contexts/AuthContext";
import { useCart } from "../contexts/CartContext";
import defaultUserPic from "/src/assets/default-user.png";
import Search from "./Search";

const Navbar = () => {
  const [isMobileMenuOpen, setIsMobileMenuOpen] = useState(false);
  const [isProfileMenuOpen, setIsProfileMenuOpen] = useState(false);
  const navigate = useNavigate()

  const { user, logout } = useAuth();
  const { cart, clearCart } = useCart();

  const handleLogout = () => {
    clearCart();
    logout();
    setIsMobileMenuOpen(false)
    navigate("/");
  }

  const navItems = [
    { id: 1, title: "Home", path: "/" },
    { id: 2, title: "Courses", path: "/courses" },
  ];

  useEffect(() => {
    if(user && user.roles && user.roles.includes("instructor"))
    {
      navItems.push({id:3, title:"Manage Courses", path:"/instructor/courses"})
    }
  }, [user])
  

  const profileNavItems = [
    { id: 1, title: "Profile", path: `/profile` }
  ];

  const cartItemCount = cart.reduce((total, item) => total + item.quantity, 0);

  return (
    <nav className="items-center py-8 bg-white shadow-md">
      <div className="container flex justify-between">
        <div className="flex items-center">
          <Link to={"/"} className="text-2xl font-bold text-indigo-600">
            Inveon Course
          </Link>
        </div>
        <div className="flex-grow justify-center md:flex hidden">
          <Search />
        </div>
        <button
          className="text-gray-600 xl:hidden block"
          onClick={() => setIsMobileMenuOpen(!isMobileMenuOpen)}
        >
          {isMobileMenuOpen ? (
            <MdClose
              size={32}
              className="hover:cursor-pointer hover:text-indigo-600"
            />
          ) : (
            <MdMenu
              size={32}
              className="hover:cursor-pointer hover:text-indigo-600"
            />
          )}
        </button>
        <ul className="hidden xl:flex items-center gap-6 text-gray-600 mx-5">
          {navItems.map((item) => (
            <li key={item.id}>
              <Link
                to={item.path}
                className="inline-block py-1 px-3 hover:text-indigo-600 font-semibold duration-200"
              >
                {item.title}
              </Link>
            </li>
          ))}
        </ul>
        {user !== null ? (
          <div className="hidden xl:flex items-center">
            <Link
              to={"/basket"}
              className="relative rounded-full p-1 hover:text-indigo-600"
            >
              <PiShoppingCartSimpleThin size={32} />
              {cartItemCount > 0 && (
                <span className="absolute -top-2 -right-2 bg-red-500 text-white rounded-full text-xs font-bold w-5 h-5 flex items-center justify-center">
                  {cartItemCount}
                </span>
              )}
            </Link>
            <div className="relative ml-3">
              <div>
                <div
                  className=" flex rounded-full text-sm hover:cursor-pointer"
                  onClick={() => setIsProfileMenuOpen(!isProfileMenuOpen)}
                >
                  <img
                    className="size-8 rounded-full"
                    src={
                      user.profilePictureUrl != null
                        ? user.profilePictureUrl
                        : defaultUserPic
                    }
                    alt="user-avatar"
                  />
                </div>
              </div>
              {isProfileMenuOpen && (
                <div className="absolute -right-20 z-10 mt-2 w-48 rounded-md bg-white py-1 shadow-lg ring-1 ring-black/5">
                  {profileNavItems.map((item) => (
                    <Link
                      to={item.path}
                      key={item.id}
                      onClick={() => setIsProfileMenuOpen(false)}
                      className="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100"
                    >
                      {item.title}
                    </Link>
                  ))}
                  <div
                    className="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 hover:cursor-pointer"
                    onClick={handleLogout}
                  >
                    Sign out
                  </div>
                </div>
              )}
            </div>
          </div>
        ) : (
          <div className="hidden xl:flex absolute inset-y-0 right-0 flex items-center sm:static sm:inset-auto sm:ml-6 sm:pr-0 gap-5">
            <Link
              to={"/login"}
              className="hover:bg-indigo-600 text-indigo-600 font-semibold hover:text-white rounded-md border-2 border-indigo-600 px-6 py-2 duration-200 hidden xl:block"
            >
              Login
            </Link>
            <Link
              to={"/register"}
              className="hover:bg-indigo-600 text-indigo-600 font-semibold hover:text-white rounded-md border-2 border-indigo-600 px-6 py-2 duration-200 hidden xl:block"
            >
              Register
            </Link>
          </div>
        )}
      </div>
      {/* Mobile Menu */}
      {isMobileMenuOpen && (
        <div className="container relative top-0 left-0 w-full h-screen bg-white z-50 p-6 flex flex-col xl:hidden block mx-auto">
          <ul className="flex flex-col items-start gap-6 text-gray-600 mb-4">
            <Search className="max-w-full md:hidden"/>
            {navItems.map((item) => (
              <li key={item.id} className="w-full">
                <Link
                  to={item.path}
                  className="block w-full text-left font-semibold rounded-md hover:bg-indigo-600 hover:text-white duration-200 py-2 px-3"
                  onClick={() => setIsMenuOpen(false)}
                >
                  {item.title}
                </Link>
              </li>
            ))}
          </ul>
          <hr />
          {user !== null ? (
            <div className="mt-4 flex flex-col gap-4">
              <div className="flex flex-row justify-between">
                <div className="flex items-center gap-3 px-3">
                  <img
                    className="size-12 rounded-full"
                    src={
                      user.profilePictureUrl != null
                        ? user.profilePictureUrl
                        : defaultUserPic
                    }
                    alt="user-avatar"
                  />
                  <div>
                    <p className="font-semibold text-gray-600">
                      {user.fullName}
                    </p>
                    <p className="text-sm text-gray-600">{user.email}</p>
                  </div>
                </div>
                <Link
                  to={"/basket"}
                  onClick={() => setIsMobileMenuOpen(!isMobileMenuOpen)}
                  className="relative flex items-center rounded-full p-1 hover:text-indigo-600"
                >
                  <PiShoppingCartSimpleThin size={32} />
                  {cartItemCount > 0 && (
                    <span className="absolute -top-2 -right-2 bg-red-500 text-white rounded-full text-xs font-bold w-5 h-5 flex items-center justify-center">
                      {cartItemCount}
                    </span>
                  )}
                </Link>
              </div>
              {profileNavItems.map((item) => (
                <Link
                  to={item.path}
                  key={item.id}
                  onClick={() => setIsMenuOpen(false)}
                >
                  <div className="hover:bg-indigo-600 text-gray-600 font-semibold hover:text-white rounded-md duration-200 w-full px-3 py-2">
                    {item.title}
                  </div>
                </Link>
              ))}
              <div
                className="hover:bg-indigo-600 text-gray-600 font-semibold hover:text-white rounded-md px-3 py-2 duration-200 w-full hover:cursor-pointer"
                onClick={handleLogout}
              >
                Sign out
              </div>
            </div>
          ) : (
            <div className="mt-8 flex flex-col gap-4">
              <Link
                to={"/login"}
                className="hover:bg-indigo-600 text-indigo-600 font-semibold hover:text-white rounded-md border-2 border-indigo-600 px-6 py-2 duration-200 w-full"
                onClick={() => setIsMenuOpen(false)}
              >
                Login
              </Link>
              <Link
                to={"/register"}
                className="hover:bg-indigo-600 text-indigo-600 font-semibold hover:text-white rounded-md border-2 border-indigo-600 px-6 py-2 duration-200 w-full"
                onClick={() => setIsMenuOpen(false)}
              >
                Register
              </Link>
            </div>
          )}
        </div>
      )}
    </nav>
  );
};

export default Navbar;
