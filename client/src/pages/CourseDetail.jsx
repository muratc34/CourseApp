import React, { useEffect, useState } from "react";
import defaultCourseImg from "/src/assets/default-course-img.png";
import defaultUserPic from "/src/assets/default-user.png";
import { Link, useParams } from "react-router-dom";
import courseApi from "../services/modules/courseApi";
import LoadingSpinner from "../components/LoadingSpinner";
import { useCart } from "../contexts/CartContext";

const CourseDetail = () => {
  const [course, setCourse] = useState(null);
  const [isLoading, setIsLoading] = useState(true);
  const { id } = useParams();
  const { cart, addToCart } = useCart();

  const isInCart = cart?.some((item) => item.id === course.id);

  useEffect(() => {
    courseApi.getCourseById(id)
      .then((response) => {
        setCourse(response.data);
        setIsLoading(false);
      })
      .catch((error) => {
        console.error(error);
        setIsLoading(false);
      });
  }, []);

  if (isLoading) {
    return <LoadingSpinner />;
  }

  return (
    <div className="bg-white">
      <div className="pt-6">
        <div className="mx-auto mt-6 max-w-2xl md:max-w-4xl lg:max-w-6xl lg:grid-cols-3 lg:gap-x-8 lg:px-8">
          <img
            alt={course.name}
            src={course.imageUrl ?? defaultCourseImg}
            className="aspect-video size-full object-cover sm:rounded-lg"
          />
        </div>
        <div className="mx-auto max-w-2xl px-4 pb-16 pt-10 sm:px-6 lg:grid lg:max-w-7xl lg:grid-cols-3 lg:grid-rows-[auto_auto_1fr] lg:gap-x-8 lg:px-8 lg:pb-24 lg:pt-16">
          <div className="lg:col-span-2 lg:border-r lg:border-gray-200 lg:pr-8">
            <h1 className="text-2xl font-bold tracking-tight text-gray-900 sm:text-3xl">
              {course.name}
            </h1>
            <div className="mt-1">
              <Link
                to={`/courses/category/${course.category.id}`}
                className="text-gray-500 hover:text-indigo-500 hover:underline"
              >
                {course.category.name}
              </Link>
            </div>
          </div>
          <div className="mt-4 lg:row-span-3 lg:mt-0">
            <div className="flex flex-col items-center gap-3 px-3">
              <img
                className="size-24 rounded-full"
                src={course.user.profilePictureUrl ?? defaultUserPic}
                alt="user-avatar"
              />
              <div>
                <p className="font-semibold text-xl text-gray-600">
                  {course.user.fullName}
                </p>
                <p className="text-md text-gray-600">{course.user.email}</p>
              </div>
            </div>
          </div>

          <div className="py-10 lg:col-span-2 lg:col-start-1 lg:border-r lg:border-gray-200 lg:pb-16 lg:pr-8 lg:pt-6">
            <div>
              <div className="space-y-6">
                <p className="text-base text-gray-900">{course.description}</p>
              </div>
            </div>
            <p className="text-3xl text-gray-900 mt-5 font-semibold text-end">
              {course.price} ₺
            </p>
            <button
              onClick={() => !isInCart && addToCart(course)}
              disabled={isInCart}
              className={`w-full font-medium rounded-lg text-sm px-5 py-2.5 text-center ms-2 me-2 mt-5 ${
                isInCart
                  ? "bg-gray-300 text-gray-500 cursor-not-allowed"
                  : "text-white bg-gradient-to-r from-indigo-500 via-indigo-600 to-indigo-700 hover:bg-gradient-to-br"
              }`}
            >
              {isInCart ? "Already in basket" : "Add to basket"}
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default CourseDetail;
