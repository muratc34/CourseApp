import React from "react";
import { Link } from "react-router-dom";

const Footer = () => {
  const navItems = [
    { id: 1, title: "Home", path: "/" },
    { id: 2, title: "Courses", path: "/courses" },
  ];

  return (
    <footer className="bg-white w-full rounded-lg shadow shadow-lg">
      <div className="w-full max-w-screen-xl mx-auto p-4 md:py-8">
        <div className="sm:flex sm:items-center sm:justify-between">
          <Link
            to={"/"}
            className="flex items-center mb-4 sm:mb-0 space-x-3 rtl:space-x-reverse"
          >
            <span className="self-center text-2xl font-bold whitespace-nowrap text-indigo-600">
              Inveon Course
            </span>
          </Link>
          <ul className="flex flex-wrap items-center mb-6 text-sm font-medium text-gray-500 sm:mb-0">
            {navItems.map((item) => (
              <li key={item.id}>
                <Link
                  to={item.path}
                  className="hover:underline me-4 md:me-6 text-gray-500"
                >
                  {item.title}
                </Link>
              </li>
            ))}
          </ul>
        </div>
        <hr className="my-6 border-gray-200 sm:mx-auto lg:my-8" />
        <span className="block text-sm text-gray-500 sm:text-center">
          © 2025 Inveon Course. All Rights Reserved.
        </span>
      </div>
    </footer>
  );
};

export default Footer;
