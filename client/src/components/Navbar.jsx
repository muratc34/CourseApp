import React, { useState } from "react";
import { Link } from "react-router-dom";
import { MdMenu, MdClose } from "react-icons/md";
import { CiSearch } from "react-icons/ci";
import { PiShoppingCartSimpleThin } from "react-icons/pi";
import { useAuth } from "../contexts/AuthContext";
import defaultUserPic from "/src/assets/default-user.png";

const Navbar = () => {
  const [isMobileMenuOpen, setIsMobileMenuOpen] = useState(false);
  const [isProfileMenuOpen, setIsProfileMenuOpen] = useState(false);

  const { user, logout } = useAuth();
  const navItems = [
    { id: 1, title: "Home", path: "/" },
    { id: 2, title: "Courses", path: "/courses" },
  ];

  const profileNavItems = [
    { id: 1, title: "Profile", path: `/profile/${user.id}` },
    { id: 2, title: "My Courses", path: `/profile/${user.id}/courses` },
  ];

  return (
    <nav className="items-center py-8 bg-white shadow-md">
      <div className="container flex justify-between">
        <div className="flex items-center">
          <Link to={"/"} className="text-2xl font-bold text-indigo-600">
            Inveon Course
          </Link>
        </div>
        <div className="flex-grow justify-center md:flex hidden">
          <div className="relative w-full max-w-md">
            <input
              type="text"
              placeholder="Search"
              className="pl-10 pr-4 py-2 w-full border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-600"
            />
            <CiSearch className="absolute top-2 left-3 text-2xl text-gray-600" />
          </div>
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
              <PiShoppingCartSimpleThin
                size={32}
              />
            </Link>
            <div className="relative ml-3">
              <div>
                <div
                  className=" flex rounded-full bg-gray-800 text-sm hover:cursor-pointer"
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
                    onClick={logout}
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
                <Link to={"/basket"} onClick={() => setIsMobileMenuOpen(!isMobileMenuOpen)} className="relative flex items-center rounded-full p-1 hover:text-indigo-600">
                    <PiShoppingCartSimpleThin size={32}/>
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
                onClick={logout}
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
