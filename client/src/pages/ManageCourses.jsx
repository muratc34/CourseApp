import React, { useEffect, useState } from "react";
import courseApi from "../services/modules/courseApi";
import defaultCourseImg from "/src/assets/default-course-img.png";
import { FaEdit, FaPlusCircle, FaTrashAlt } from "react-icons/fa";
import LoadingSpinner from "../components/LoadingSpinner";
import categoryApi from "../services/modules/categoryApi";
import { IoClose } from "react-icons/io5";
import Input from "../components/Input";

const ManageCourses = () => {
  const authUser = JSON.parse(localStorage.getItem("user"));
  const [isLoading, setIsLoading] = useState(true);
  const [courses, setCourses] = useState([]);
  const [categories, setCategories] = useState([]);
  const [isOpenAddModal, setIsOpenAddModal] = useState(false);
  const [isOpenUpdateModal, setIsOpenUpdateModal] = useState(false);
  const [errors, setErrors] = useState([]);
  const [formData, setFormData] = useState({
    name: "",
    description: "",
    price: "",
    categoryId: "",
    instructorId: authUser.id,
  });

  const getCategories = () => {
    categoryApi
      .getCategories()
      .then((response) => {
        setCategories(response.data);
      })
      .catch((err) => {
        console.error(err);
      });
  };

  const getInstructorCourses = () => {
    courseApi
      .getUserCoursesByInstructorId(authUser.id)
      .then((response) => {
        setCourses(response.data);
        setIsLoading(false);
      })
      .catch((error) => {
        console.error(error);
      });
  };

  useEffect(() => {
    getInstructorCourses();
    getCategories();
  }, []);

  if (isLoading) {
    return <LoadingSpinner />;
  }

  const handleAdd = (e) => {
    e.preventDefault();
    console.log(formData);
    
    courseApi.createCourse({
        name: formData.name,
        description: formData.description,
        price: formData.price,
        categoryId: formData.categoryId,
        instructorId: authUser.id
    }).then(response => {
        getInstructorCourses();
    }).catch(err => {
        setErrors(err.errors);
    })
  };

  const handleInputChange = (event) => {
    const { name, value } = event.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
    setErrors([]);
  };

  return (
    <div className="w-7xl my-24 mx-8">
      <div className="w-full flex justify-between items-center mb-5">
        <h2 className="text-4xl font-bold text-gray-700">
          Manage Your Courses
        </h2>
        <button
          onClick={() => setIsOpenAddModal(true)}
          className="flex justify-center items-center gap-2 p-2.5 bg-blue-500 rounded-lg shadow-md text-white font-bold w-16"
        >
          <FaPlusCircle size={24} />
        </button>
      </div>
      <ul role="list" className="divide-y divide-gray-100">
        {courses.map((course) => (
          <li key={course.id} className="grid grid-cols-4 gap-5 py-5">
            <div className="col-span-1 flex justify-center items-center">
              <img
                alt={course.name}
                src={course.imageUrl ?? defaultCourseImg}
                className="h-20 md:h-40 object-cover flex-none rounded-lg bg-gray-50"
              />
            </div>
            <div className="col-span-2 w-full">
              <p className="text-md md:text-lg font-bold text-gray-800 text-wrap">
                {course.name}
              </p>
              <p className="mt-1 text-sm md:text-md text-gray-500">
                {course.description}
              </p>
            </div>
            <div className="col-span-1 flex flex-col justify-center items-center gap-5">
              <button className="flex justify-center items-center gap-2 p-2.5 bg-yellow-500 rounded-lg shadow-md text-white font-bold w-32">
                <FaEdit size={24} />
                <span>Update</span>
              </button>
              <button className="flex justify-center items-center gap-2 p-2.5 bg-red-500 rounded-lg shadow-md text-white font-bold w-32">
                <FaTrashAlt size={24} />
                Delete
              </button>
            </div>
          </li>
        ))}
      </ul>

      {/* Create Course Modal */}
      {isOpenAddModal && (
        <div className="max-h-auto left-0 right-0 top-0 z-50 flex max-h-full w-full items-center justify-center overflow-y-auto overflow-x-hidden antialiased fixed inset-0 bg-gray-500/75 transition-opacity">
          <div className="max-h-auto relative max-h-full w-full max-w-lg p-4">
            <div className="relative rounded-lg bg-white shadow">
              <div className="flex items-center justify-between rounded-t border-b border-gray-200 p-4">
                <h3 className="text-lg font-semibold text-gray-900">
                  Create New Course
                </h3>
                <button
                  type="button"
                  onClick={() => setIsOpenAddModal(false)}
                  className="ms-auto inline-flex h-8 w-8 items-center justify-center rounded-lg bg-transparent text-sm text-gray-400 hover:bg-gray-200 hover:text-gray-900"
                >
                  <IoClose size={24} />
                </button>
              </div>
              <form onSubmit={(e) => handleAdd(e)} className="p-4 md:p-5">
                <div className="mb-5 grid grid-cols-1 gap-4 sm:grid-cols-2">
                  <div className="col-span-2 sm:col-span-1">
                    <Input
                      id={"name"}
                      type={"text"}
                      required={true}
                      value={formData.name}
                      onChange={handleInputChange}
                      label={"Title"}
                      placeholder={"Enter title"}
                    />
                  </div>
                  <div className="col-span-2 sm:col-span-1">
                    <Input
                      id={"description"}
                      type={"description"}
                      required={true}
                      value={formData.description}
                      onChange={handleInputChange}
                      label={"Description"}
                      placeholder={"Enter description"}
                    />
                  </div>
                  <div className="col-span-2 sm:col-span-1">
                    <Input
                      id={"price"}
                      type={"text"}
                      required={true}
                      value={formData.price}
                      onChange={handleInputChange}
                      label={"Price"}
                      placeholder={"Enter a price"}
                    />
                  </div>
                  <div className="col-span-2 sm:col-span-1">
                    <div className="flex flex-col w-full gap-2">
                      <div className="flex justify-between">
                        <label
                          htmlFor="category"
                          className="block text-sm/6 font-medium text-gray-900"
                        >
                          Select Category
                        </label>
                      </div>
                      <select
                        id="categoryId"
                        name="categoryId"
                        value={formData.categoryId}
                        onChange={handleInputChange}
                        className="block w-full rounded-md bg-white px-3 py-1.5 text-base text-gray-900 outline outline-1 -outline-offset-1 outline-gray-300 placeholder:text-gray-400 focus:outline focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-600 sm:text-sm/6"
                      >
                        <option value="" disabled>
                          Select a category
                        </option>
                        {categories.map((category) => (
                          <option key={category.id} value={category.id}>
                            {category.name}
                          </option>
                        ))}
                      </select>
                    </div>
                  </div>
                </div>
                <div className="border-t border-gray-200 pt-4 md:pt-5">
                  <button
                    type="submit"
                    className="me-2 rounded-lg border border-gray-200 bg-indigo-600 px-5 py-2.5 text-sm font-medium text-white hover:bg-indigo-700 focus:z-10 focus:outline-none focus:ring-4 focus:ring-gray-100"
                  >
                    Save information
                  </button>
                  {errors.length > 0 ? (
                    errors.map((error, index) => (
                      <div
                        key={index}
                        className="text-md text-red-500 m-1 mt-3"
                      >
                        {error.description}
                      </div>
                    ))
                  ) : (
                    <></>
                  )}
                </div>
              </form>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default ManageCourses;
