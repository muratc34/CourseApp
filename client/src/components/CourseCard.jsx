import React from "react";
import { Link } from "react-router-dom";
import { useCart } from "../contexts/CartContext";
import defaultCourseImg from "/src/assets/default-course-img.png";

const CourseCard = ({ course }) => {
  const { cart, addToCart, userCourses } = useCart();

  const isInCart = cart.some((item) => item.id === course.id);
  const isOwnedByUser = userCourses.some((userCourse) => userCourse.id === course.id);
  return (
    <div className="group relative shadow-lg rounded-2xl">
      <Link to={`/course/${course.id}`}>
        <img
          alt={course.name}
          src={course.imageUrl ?? defaultCourseImg}
          className="aspect-square w-full rounded-t-2xl bg-gray-200 object-cover group-hover:opacity-75 h-60 sm:h-40"
        />
      </Link>
      <div className="my-4 px-1 flex justify-between gap-4">
        <div className="flex flex-col truncate">
          <h3 className="text-gray-600 font-semibold text-md truncate">
            <Link to={`/course/${course.id}`}>{course.name}</Link>
          </h3>
          <Link
            to={`/profile/${course.user.id}`}
            className="text-sm mt-2 hover:text-indigo-700"
          >
            {course.user.fullName}
          </Link>
        </div>
        <p className="text-gray-600 font-semibold font-size text-lg text-nowrap">
          {course.price} ₺
        </p>
      </div>
      <div className="w-full flex justify-end items-center">
        <button
          onClick={() => !isInCart && !isOwnedByUser && addToCart(course)}
          disabled={isInCart}
          className={`w-full font-medium rounded-lg text-sm px-5 py-2.5 text-center ms-2 me-2 mb-2 ${
            isInCart
              ? "bg-gray-300 text-gray-500 cursor-not-allowed"
              : "text-white bg-gradient-to-r from-indigo-500 via-indigo-600 to-indigo-700 hover:bg-gradient-to-br"
          }`}
        >
          {isOwnedByUser ? "Already Owned" : isInCart ? "Already in Basket" : "Add to Basket"}
        </button>
      </div>
    </div>
  );
};

export default CourseCard;
