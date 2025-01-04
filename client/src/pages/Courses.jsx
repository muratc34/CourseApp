import React, { useEffect, useState } from "react";
import CourseCard from "../components/CourseCard";
import courseApi from "../services/modules/courseApi";
import LoadingSpinner from "../components/LoadingSpinner";
import Pagination from "../components/Pagination";
import categoryApi from "../services/modules/categoryApi";
import { IoClose, IoFunnel } from "react-icons/io5";

const Courses = () => {
  const [courses, setCourses] = useState([]);
  const [categories, setCategories] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [isFilterOpen, setIsFilterOpen] = useState(false);
  const [selectedCategoryId, setSelectedCategoryId] = useState(null);
  const pageSize = 12;

  const getCourses = () => {
    courseApi
      .getCourses(currentPage, pageSize)
      .then((response) => {
        setCourses(response.data.items);
        setTotalPages(response.data.totalPages);
        setIsLoading(false);
      })
      .catch((error) => {
        console.error(error);
      });
  };

  const getCategories = () => {
    categoryApi
      .getCategories()
      .then((response) => {
        setCategories(response.data);
      })
      .catch((error) => {
        console.error(error);
      });
  };

  const getCoursesByCategoryId = () => {
    setIsLoading(true);
    courseApi
      .getCoursesByCategoryId(selectedCategoryId, currentPage, pageSize)
      .then((response) => {
        setCourses(response.data.items);
        setTotalPages(response.data.totalPages);
        setIsLoading(false);
      })
      .catch((error) => {
        console.error(error);
        setIsLoading(false);
      });
  };

  const getAllData = () => {
    setIsLoading(true);
    Promise.all([getCourses(), getCategories()])
      .then(() => {
        setIsLoading(false);
      })
      .catch((error) => {
        setIsLoading(false);
        console.error(error);
      });
  };

  useEffect(() => {
    getAllData();
  }, [currentPage]);

  useEffect(() => {
    if (!selectedCategoryId) return;
    getCoursesByCategoryId();
  }, [selectedCategoryId]);

  const handleCategoryChange = (categoryId) => {
    setSelectedCategoryId(categoryId);
    setCurrentPage(1);
  };

  const handlePageChange = (page) => {
    setCurrentPage(page);
  };

  const clearFilter = () => {
    getAllData();
    setSelectedCategoryId(null);
  };

  if (isLoading) {
    return <LoadingSpinner />;
  }

  return (
    <div>
      {isFilterOpen && (
        <div className="relative z-40 lg:hidden">
          <div className="fixed inset-0 bg-black/25"></div>
          <div className="fixed inset-0 z-40 flex">
            <div className="relative ml-auto flex size-full max-w-xs flex-col overflow-y-auto bg-white py-4 pb-12 shadow-xl">
              <div className="flex items-center justify-between px-4">
                <h2 className="text-lg font-medium text-gray-900">Filters</h2>
                <button
                  onClick={() => setIsFilterOpen(false)}
                  type="button"
                  className="-mr-2 flex size-10 items-center justify-center rounded-md bg-white p-2 text-gray-400"
                >
                  <IoClose size={32} />
                </button>
              </div>

              <div className="mt-4 border-t border-gray-200">
                <div className="border-t border-gray-200 px-4 py-6">
                  <h3 className="-mx-2 -my-3 flow-root">
                    <span className="flex w-full items-center justify-between bg-white px-2 py-3 text-gray-400 font-medium text-gray-900">
                      Category
                    </span>
                  </h3>
                  <div className="pt-6">
                    <div className="space-y-6">
                      {categories.map((category) => (
                        <div key={category.id} className="flex gap-3">
                          <div className="flex h-5 shrink-0 items-center">
                            <div className="group grid size-4 grid-cols-1">
                              <input
                                value={category.id}
                                checked={selectedCategoryId === category.id}
                                onChange={() =>
                                  handleCategoryChange(category.id)
                                }
                                name="category"
                                type="radio"
                                className="col-start-1 row-start-1 rounded border border-gray-300 bg-white appearance-none checked:bg-indigo-600 indeterminate:border-indigo-600"
                              />
                            </div>
                          </div>
                          <span className="min-w-0 flex-1 text-gray-500">
                            {category.name}
                          </span>
                        </div>
                      ))}
                    </div>
                    <div className="space-y-6 mt-5">
                      <button onClick={clearFilter}
                      className="flex w-full items-center justify-center rounded-lg bg-indigo-700 px-5 py-2.5 text-sm font-medium text-white hover:bg-indigo-800">
                        Clear Filter
                      </button>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      )}
      <main className="mx-auto max-w-full px-4 sm:px-6 lg:px-32">
        <div className="flex items-baseline justify-between border-b border-gray-200 pb-6 pt-8">
          <h1 className="text-4xl font-bold tracking-tight text-gray-700">
            Courses
          </h1>
          <div className="flex items-center">
            <button
              onClick={() => setIsFilterOpen(true)}
              type="button"
              className="-m-2 ml-4 p-2 text-gray-400 hover:text-gray-500 sm:ml-6 block lg:hidden"
            >
              <IoFunnel className="size-5" />
            </button>
          </div>
        </div>
        <section>
          <div className="grid grid-cols-1 gap-x-8 gap-y-10 lg:grid-cols-4">
            <div className="hidden lg:block">
              <div className="py-6">
                <h3 className="-my-3 flow-root">
                  <span className="flex w-full items-center justify-between px-2 py-3 text-gray-400 font-medium text-gray-700">
                    Category
                  </span>
                </h3>
                <div className="pt-6">
                  <div className="space-y-4">
                    {categories.map((category) => (
                      <div key={category.id} className="flex gap-3">
                        <div className="flex h-5 shrink-0 items-center">
                          <div className="group grid size-4 grid-cols-1">
                            <input
                              value={category.id}
                              checked={selectedCategoryId === category.id}
                              onChange={() => handleCategoryChange(category.id)}
                              name="category"
                              type="radio"
                              className="col-start-1 row-start-1 rounded border border-gray-300 bg-white appearance-none checked:bg-indigo-600 indeterminate:border-indigo-600"
                            />
                          </div>
                        </div>
                        <span className="min-w-0 flex-1 text-gray-600">
                          {category.name}
                        </span>
                      </div>
                    ))}
                  </div>
                  <div className="space-y-4 mt-5">
                      <button onClick={clearFilter}
                      className="flex w-full items-center justify-center rounded-lg bg-indigo-700 px-5 py-2.5 text-sm font-medium text-white hover:bg-indigo-800">
                        Clear Filter
                      </button>
                    </div>
                </div>
              </div>
            </div>

            <div className="lg:col-span-3">
              <div className="mt-6 grid grid-cols-1 md:grid-cols-2 gap-x-6 gap-y-10 xl:grid-cols-3 xl:gap-x-8 mb-6">
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
          </div>
        </section>
      </main>
    </div>
  );
};

export default Courses;
