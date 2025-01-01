import React, { useEffect, useState } from "react";
import { CiSearch } from "react-icons/ci";
import courseApi from "../services/modules/courseApi";
import { Link, useNavigate } from "react-router-dom";

const Search = ({ className }) => {
  const [searchTerm, setSearchTerm] = useState("");
  const [courses, setCourses] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  
  const navigate = useNavigate();

  const search = async (keyword) => {
    if (keyword !== null && keyword.trim() !== "") {
      await courseApi.searchCourseByName(keyword).then((response) => {
        setCourses(response.data);
        setIsLoading(false);
      });
    }
  };

  useEffect(() => {
    search(searchTerm)
  }, [searchTerm]);

  const handleNavigate = (id) =>{
    setSearchTerm("");
    setCourses([]);
    navigate(`/course/${id}`)
  }

  return (
    <div className={`relative w-full max-w-md ${className}`}>
      <input
        value={searchTerm}
        onChange={(e) => {
          setSearchTerm(e.target.value);
          if (e.target.value === "") {
            setCourses([]);
          }
        }}
        type="text"
        placeholder="Search a course name..."
        className="pl-10 pr-4 py-2 w-full border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-600"
      />
      <CiSearch className="absolute top-2 left-3 text-2xl text-gray-600" />
      <div className={`absolute bg-slate-200 p-1 max-h-80 overflow-y-auto rounded-lg shadow-lg ${courses.length > 0 ? 'block': 'hidden'}`}>
        {courses.length > 0 ? (
          <div className="grid grid-cols-1 gap-4 z-50">
            {courses.map((course) => (
              <div
                key={course.id}
                onClick={() => handleNavigate(course.id)}
                className="flex items-start border rounded-lg p-4 shadow-sm hover:shadow-md transition-shadow bg-white hover:cursor-pointer"
              >
                <div className="flex-grow">
                  <h3 className="text-lg font-semibold text-gray-800">
                    {course.name}
                  </h3>
                  <p className="text-sm text-gray-500 mt-1">
                    Instructor:{" "}
                    <span className="text-gray-800 font-medium">
                      {course.user.fullName}
                    </span>
                  </p>
                </div>
                <div className="ml-4 text-right">
                  <p className="text-blue-600 font-bold text-nowrap">{course.price} ₺</p>
                </div>
              </div>
            ))}
          </div>
        ) : (
          searchTerm.trim() && (
            <p className="text-center text-gray-600">No courses found.</p>
          )
        )}
      </div>
    </div>
  );
};

export default Search;
