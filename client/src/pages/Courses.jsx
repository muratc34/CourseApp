import React, { useEffect, useState } from "react";
import CourseCard from "../components/CourseCard";
import courseApi from "../services/modules/courseApi";
import LoadingSpinner from "../components/LoadingSpinner";
import Pagination from "../components/Pagination";

const Courses = () => {
  const [courses, setCourses] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const pageSize = 12; 

  useEffect(() => {
    courseApi
      .getCourses(currentPage, pageSize)
      .then((response) => {
        setCourses(response.data.items);
        setTotalPages(response.data.totalPages); 
        setIsLoading(false);
      })
      .catch((error) => {
        console.error(error);
        setIsLoading(false);
      });
  }, [currentPage]);

  const handlePageChange = (page) => {
    setCurrentPage(page);
  };

  if (isLoading) {
    return <LoadingSpinner />;
  }

  return (
    <div className="mx-auto max-w-2xl px-4 py-16 sm:px-6 sm:py-24 lg:max-w-7xl lg:px-8">
      <div className="mt-6 grid grid-cols-1 sm:grid-cols-2 gap-x-6 gap-y-10 lg:grid-cols-4 xl:gap-x-8">
        {courses.map((course) => (
          <CourseCard key={course.id} course={course} />
        ))}
      </div>
      <Pagination
        totalPages={totalPages}
        currentPage={currentPage}
        onPageChange={handlePageChange}
      />
    </div>
  );
};

export default Courses;
