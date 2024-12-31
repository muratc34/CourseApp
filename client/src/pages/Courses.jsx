import React from "react";
import CourseCard from "../components/CourseCard";

const Courses = () => {
  const courses = [
    {
      id: 1,
      name: "React for Beginners React for Beginners",
      description:
        "Learn the fundamentals of React.js and build amazing web applications.",
      price: 250,
      imageUrl: "https://via.placeholder.com/300",
      user:{
        id:1,
        name:"Test User"
      }
    },
    {
      id: 2,
      name: "React for Beginners",
      description:
        "Learn the fundamentals of React.js and build amazing web applications.",
      price: 250,
      imageUrl: "https://via.placeholder.com/300",
      user:{
        id:1,
        name:"Test User"
      }
    },
    {
      id: 3,
      name: "React for Beginners",
      description:
        "Learn the fundamentals of React.js and build amazing web applications.",
      price: 250,
      imageUrl: "https://via.placeholder.com/300",
      user:{
        id:1,
        name:"Test User"
      }
    },
    {
      id: 4,
      name: "React for Beginners",
      description:
        "Learn the fundamentals of React.js and build amazing web applications.",
      price: 250,
      imageUrl: "https://via.placeholder.com/300",
      user:{
        id:1,
        name:"Test User"
      }
    },
    {
      id: 5,
      name: "React for Beginners",
      description:
        "Learn the fundamentals of React.js and build amazing web applications.",
      price: 250,
      imageUrl: "https://via.placeholder.com/300",
      user:{
        id:1,
        name:"Test User"
      }
    },
  ];

  return (
    <>
      <div className="mx-auto max-w-2xl px-4 py-16 sm:px-6 sm:py-24 lg:max-w-7xl lg:px-8">
        <div className="mt-6 grid grid-cols-1 sm:grid-cols-2 gap-x-6 gap-y-10 lg:grid-cols-4 xl:gap-x-8">
          {courses.map((course) => (
            <CourseCard key={course.id} course={course} />
          ))}
        </div>
      </div>
    </>
  );
};

export default Courses;
