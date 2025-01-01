import React, { useEffect, useState } from "react";
import CourseCard from "../components/CourseCard";
import courseApi from "../services/modules/courseApi";
import LoadingSpinner from "../components/LoadingSpinner";
import { Link } from "react-router-dom";

const Home = () => {
  const [courses, setCourses] = useState([])
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    courseApi
      .getCourses(2, 4)
      .then((response) => {
        setCourses(response.data.items);
        setIsLoading(false);
      })
      .catch((error) => {
        console.error(error);
        setIsLoading(false);
      });
  }, [])


  if(isLoading)
  {
    return <LoadingSpinner/>
  }
 
  return (
    <div className="bg-gray-100">
      {/* Hero Section */}
      <section className="bg-indigo-600 text-white py-40 text-center">
        <h1 className="text-4xl font-bold mb-4">
          Welcome to Our Course Platform
        </h1>
        <p className="text-xl">
          Learn new skills and enhance your knowledge with expert instructors.
        </p>
      </section>

      {/* Popular Courses */}
      <section className="py-24 px-6">
        <h2 className="text-4xl font-semibold text-center mb-12">
          Popular Courses
        </h2>
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-8">
          {courses.map((course) => (
            <CourseCard key={course.id} course={course}/>
          ))
          }
        </div>
      </section>

      {/* Call to Action */}
      <section className="bg-indigo-600 text-white py-32 text-center">
        <h2 className="text-3xl font-semibold mb-6">Start Learning Today</h2>
        <p className="text-lg mb-6">
          Sign up now and access our wide range of courses and learning
          resources.
        </p>
        <Link to={"/register"} className="bg-white text-indigo-600 py-2 px-6 rounded-full hover:bg-gray-200 transition duration-300">
          Sign Up
        </Link>
      </section>
    </div>
  );
};

export default Home;
