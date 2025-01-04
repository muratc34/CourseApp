import React, { useEffect, useState } from "react";
import courseApi from "../services/modules/courseApi";
import defaultCourseImg from "/src/assets/default-course-img.png";
import { FaEdit, FaPlusCircle, FaTrashAlt } from "react-icons/fa";
import LoadingSpinner from "../components/LoadingSpinner";
import categoryApi from "../services/modules/categoryApi";
import { IoClose, IoWarningOutline } from "react-icons/io5";
import Input from "../components/Input";
import { IoIosCloudUpload } from "react-icons/io";
import axios from "axios";
import config from "../services/configs/config";
import { ToastContainer, toast } from "react-toastify";

const ManageCourses = () => {
  const authUser = JSON.parse(localStorage.getItem("user"));
  const [isLoading, setIsLoading] = useState(true);
  const [courses, setCourses] = useState([]);
  const [categories, setCategories] = useState([]);
  const [isOpenAddModal, setIsOpenAddModal] = useState(false);
  const [isOpenUpdateModal, setIsOpenUpdateModal] = useState(false);
  const [isOpenDeleteModal, setIsOpenDeleteModal] = useState(false);
  const [errors, setErrors] = useState([]);
  const [isUploading, setIsUploading] = useState(false);
  const [isChangeUploading, setIsChangeUploading] = useState(false);
  const [formData, setFormData] = useState({
    name: "",
    description: "",
    price: "",
    categoryId: "",
    instructorId: "",
  });
  const [updateFormData, setUpdateFormData] = useState({
    name: "",
    description: "",
    price: "",
    categoryId: "",
    instructorId: "",
  });
  const [updateCourseId, setUpdateCourseId] = useState();
  const [deleteCourseId, setDeleteCourseId] = useState();
  const [file, setFile] = useState(null);
  const [changeImgFile, setChangeImgFile] = useState(null);

  const handleFileChange = (e) => {
    const selectedFile = e.target.files[0];
    if (selectedFile) {
      setFile(selectedFile);
    }
  };

  const handleChangeImg = (e, course) => {
    setIsChangeUploading((prev) => ({ ...prev, [course.id]: true }));
    const selectedFile = e.target.files[0];
    const formData = new FormData();
    formData.append("formFile", selectedFile);
    setChangeImgFile(selectedFile);
    uploadCourseImage(course, formData, "updateImg");
  };

  const handleRemoveFile = () => {
    setFile(null);
  };

  const uploadCourseImage = (course, formData, type) => {
    axios
      .post(`${config.baseURL}/Courses/UploadImage/${course.id}`, formData, {
        headers: {
          "Content-Type": "multipart/form-data",
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
      })
      .then(() => {
        setIsUploading(false);
        setIsChangeUploading((prev) => ({ ...prev, [course.id]: false }));
        setIsOpenAddModal(false);
        getInstructorCourses();
        if (type === "updateImg") {
          toast.success("Course image updated successfully!");
        } else if (type === "createCourse") {
          toast.success("New course created successfully!");
        }
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

  const handleAdd = (e) => {
    e.preventDefault();
    courseApi
      .createCourse({
        name: formData.name,
        description: formData.description,
        price: formData.price,
        categoryId: formData.categoryId,
        instructorId: authUser.id,
      })
      .then((response) => {
        setIsUploading(true);
        if (file) {
          const formData = new FormData();
          formData.append("formFile", file);
          uploadCourseImage(response.data, file, "createCourse");
        }
        setFormData({
          name: "",
          description: "",
          price: "",
          categoryId: "",
          instructorId: "",
        });
      })
      .catch((err) => {
        console.log(err);
        setErrors(err.errors);
      })
      .finally(() => {
        setIsOpenAddModal(false);
        getInstructorCourses();
      });
  };

  const handleInputChange = (event) => {
    const { name, value } = event.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
    setErrors([]);
  };

  const handleUpdateInputChange = (event) => {
    const { name, value } = event.target;
    setUpdateFormData((prev) => ({ ...prev, [name]: value }));
    setErrors([]);
  };

  const handleCloseUpdateModal = () => {
    setIsOpenUpdateModal(false);
    setUpdateFormData({
      name: "",
      description: "",
      price: "",
      categoryId: "",
      instructorId: "",
    });
  };

  const handleUpdateModal = (course) => {
    setUpdateCourseId(course.id);
    setIsOpenUpdateModal(true);
    setUpdateFormData({
      name: course.name,
      description: course.description,
      price: course.price,
      categoryId: course.categoryId,
      instructorId: course.instructorId,
    });
  };

  const handleUpdate = (e) => {
    e.preventDefault();
    courseApi
      .updateCourse(updateCourseId, {
        name: updateFormData.name,
        description: updateFormData.description,
        price: updateFormData.price,
        categoryId: updateFormData.categoryId,
        instructorId: authUser.id,
      })
      .then(() => {
        setUpdateFormData({
          name: "",
          description: "",
          price: "",
          categoryId: "",
          instructorId: "",
        });
        getInstructorCourses();
        setIsOpenUpdateModal(false);
      })
      .catch((err) => {
        setErrors(err.errors);
      });
  };

  const deleteCourse = () => {
    if (deleteCourseId) {
      courseApi
        .deleteCourse(deleteCourseId)
        .then(() => {
          toast.success("Course deleted successfully!");
          getInstructorCourses();
        })
        .catch((err) => {
          toast.error("Course can not deleted!");
        })
        .finally(() => {
          setIsOpenDeleteModal(false);
        });
    }
  };

  const handleOpenDeleteModal = (course) => {
    setIsOpenDeleteModal(true);
    setDeleteCourseId(course.id);
  };
  const handleCloseDeleteModal = () => {
    setIsOpenDeleteModal(false);
    setDeleteCourseId(null);
  };

  if (isLoading) {
    return <LoadingSpinner />;
  }
  return (
    <div className="w-7xl my-24 mx-8">
      <div className="w-full flex justify-between items-center mb-5 border-b border-gray-200 pb-4">
        <h2 className="text-4xl font-bold text-gray-700">
          Manage Your Courses
        </h2>
        <ToastContainer
          position="bottom-center"
          autoClose={3000}
          hideProgressBar={false}
          newestOnTop={false}
          closeOnClick
          rtl={false}
        />
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
            <div className="col-span-1 flex justify-center items-center group relative">
              {isChangeUploading[course.id] ? (
                <LoadingSpinner className={"min-h-20 md:min-h-20"} />
              ) : (
                <>
                  <img
                    alt={course.name}
                    src={course.imageUrl ?? defaultCourseImg}
                    className="h-20 md:h-40 w-full object-cover flex-none rounded-lg bg-gray-50 group-hover:opacity-75 transition-opacity"
                  />
                  <label
                    htmlFor={`updateImg-${course.id}`}
                    className="absolute inset-0 flex justify-center items-center bg-black bg-opacity-50 opacity-0 group-hover:opacity-100 transition-opacity rounded-lg cursor-pointer"
                  >
                    <FaEdit size={24} className="text-white" />
                  </label>
                  <input
                    id={`updateImg-${course.id}`}
                    type="file"
                    accept="image/*"
                    className="hidden"
                    onChange={(e) => {
                      handleChangeImg(e, course);
                    }}
                  />
                </>
              )}
            </div>
            <div className="col-span-2 w-full">
              <p className="text-md md:text-lg font-bold text-gray-800 text-wrap">
                {course.name}
              </p>
              <p className="mt-1 text-sm md:text-md text-gray-500">
                {course.description}
              </p>
              <p className="mt-1 text-md md:text-lg font-bold text-gray-500">
                {course.price} ₺
              </p>
            </div>
            <div className="col-span-1 flex flex-col justify-center items-center gap-5">
              <button
                onClick={() => handleUpdateModal(course)}
                className="flex justify-center items-center gap-2 p-2.5 bg-yellow-500 hover:bg-yellow-400 rounded-lg shadow-md text-white font-bold w-32"
              >
                <FaEdit size={24} />
                <span>Update</span>
              </button>
              <button
                onClick={() => handleOpenDeleteModal(course)}
                className="flex justify-center items-center gap-2 p-2.5 bg-red-500 hover:bg-red-400 rounded-lg shadow-md text-white font-bold w-32"
              >
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
              {isUploading ? (
                <LoadingSpinner />
              ) : (
                <>
                  <div className="flex items-center justify-center w-full p-5">
                    <div className="relative flex items-center justify-center w-full h-64 border-2 border-gray-300 border-dashed rounded-lg bg-gray-50 hover:bg-gray-100">
                      {file ? (
                        <div className="relative w-full h-full flex items-center justify-center">
                          <img
                            src={URL.createObjectURL(file)}
                            alt="Preview"
                            className="absolute w-full h-full object-cover rounded-lg"
                          />
                          <button
                            className="absolute top-2 right-2 bg-red-500 text-white rounded-full p-1 opacity-100 transition-opacity"
                            onClick={handleRemoveFile}
                          >
                            <IoClose size={24} />
                          </button>
                        </div>
                      ) : (
                        <label
                          htmlFor="dropzone"
                          className="flex flex-col items-center justify-center w-full h-full cursor-pointer"
                        >
                          <IoIosCloudUpload
                            size={40}
                            className="text-gray-500"
                          />
                          <p className="mb-2 text-sm text-gray-500 dark:text-gray-400">
                            <span className="font-semibold">Click </span> or{" "}
                            <span className="font-semibold">drag and drop</span>{" "}
                            to upload an image
                          </p>
                          <p className="text-xs text-gray-500 dark:text-gray-400">
                            (Not required. You can leave it blank.)
                          </p>
                          <input
                            id="dropzone"
                            type="file"
                            accept="image/*"
                            className="hidden"
                            onChange={handleFileChange}
                          />
                        </label>
                      )}
                    </div>
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
                        Create
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
                </>
              )}
            </div>
          </div>
        </div>
      )}

      {/* Update Course Modal */}
      {isOpenUpdateModal && (
        <div className="max-auto left-0 right-0 top-0 z-50 flex max h-full w-full items-center justify-center overflow-y-auto overflow-x-hidden antialiased fixed inset-0 bg-gray-500/75 trasition-opacity">
          <div className="max-h-auto relative max-h-full w-full max-w-lg p-4">
            <div className="relative rounded-lg bg-white shadow">
              <div className="flex items-center justify-between rounded-t border-b border-gray-200 p-4">
                <h3 className="text-lg font-semibold text-gray-900">
                  Update Course
                </h3>
                <button
                  type="button"
                  onClick={handleCloseUpdateModal}
                  className="ms-auto inline-flex h-8 w-8 items-center justify-center rounded-lg bg-transparent text-sm text-gray-400 hover:bg-gray-200 hover:text-gray-900"
                >
                  <IoClose size={24} />
                </button>
              </div>
              <div className="flex items-center w-full p-5">
                <form
                  onSubmit={(e) => handleUpdate(e)}
                  className="p-4 md:p-5 w-full"
                >
                  <div className="mb-5 flex flex-col w-full gap-4">
                    <Input
                      id={"name"}
                      type={"text"}
                      required={true}
                      value={updateFormData.name}
                      onChange={handleUpdateInputChange}
                      label={"Title"}
                      placeholder={"Enter title"}
                    />

                    <Input
                      id={"description"}
                      type={"description"}
                      required={true}
                      value={updateFormData.description}
                      onChange={handleUpdateInputChange}
                      label={"Description"}
                      placeholder={"Enter description"}
                    />
                    <div className="flex justfiy-between items-center gap-5">
                      <Input
                        id={"price"}
                        type={"text"}
                        required={true}
                        value={updateFormData.price}
                        onChange={handleUpdateInputChange}
                        label={"Price"}
                        placeholder={"Enter a price"}
                      />
                      <div className="w-full">
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
                            value={updateFormData.categoryId}
                            onChange={handleUpdateInputChange}
                            className="block w-full rounded-md bg-white px-3 py-2 text-base text-gray-900 outline outline-1 -outline-offset-1 outline-gray-300 placeholder:text-gray-400 focus:outline focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-600 sm:text-sm/6"
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
                  </div>
                  <div className="border-t border-gray-200 pt-4 md:pt-5">
                    <button
                      type="submit"
                      className="me-2 rounded-lg border border-gray-200 bg-indigo-600 px-5 py-2.5 text-sm font-medium text-white hover:bg-indigo-700 focus:z-10 focus:outline-none focus:ring-4 focus:ring-gray-100"
                    >
                      Save
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
        </div>
      )}

      {/* Delete Course Modal */}
      {isOpenDeleteModal && (
        <div className="max-h-auto left-0 right-0 top-0 z-50 flex max-h-full w-full items-center justify-center overflow-y-auto overflow-x-hidden antialiased fixed inset-0 bg-gray-500/75 transition-opacity">
          <div className="max-h-auto relative max-h-full w-full max-w-lg p-4">
            <div className="relative rounded-lg bg-white shadow">
              <div className="flex items-center justify-between rounded-t border-b border-gray-200 p-4">
                <h3 className="text-lg font-semibold text-gray-900">
                  Delete Course
                </h3>
                <button
                  type="button"
                  onClick={() => handleCloseDeleteModal()}
                  className="ms-auto inline-flex h-8 w-8 items-center justify-center rounded-lg bg-transparent text-sm text-gray-400 hover:bg-gray-200 hover:text-gray-900"
                >
                  <IoClose size={24} />
                </button>
              </div>
              <div className="relative transform overflow-hidden rounded-lg bg-white text-left shadow-xl transition-all sm:my-2 sm:w-full sm:max-w-lg">
                <div className="bg-white px-4 pb-4 pt-5 sm:p-6 sm:pb-4">
                  <div className="sm:flex sm:items-start">
                    <div className="mx-auto flex size-16 shrink-0 items-center justify-center rounded-full bg-red-100 sm:mx-0 sm:size-10">
                      <IoWarningOutline size={32} className="text-red-600 p-0 sm:p-0.5" />
                    </div>
                    <div className="mt-3 text-center sm:ml-4 sm:mt-0 sm:text-left">
                      <h3
                        className="text-base font-semibold text-gray-900"
                        id="modal-title"
                      >
                        Delete course
                      </h3>
                      <div className="mt-2">
                        <p className="text-sm text-gray-500">
                          Are you sure you want to delete your course? All of
                          your data will be permanently removed. This action
                          cannot be undone.
                        </p>
                      </div>
                    </div>
                  </div>
                </div>
                <div className="bg-gray-50 px-4 py-3 sm:flex sm:flex-row-reverse sm:px-6">
                  <button
                    onClick={deleteCourse}
                    type="button"
                    class="inline-flex w-full justify-center rounded-md bg-red-600 px-3 py-2 text-sm font-semibold text-white shadow-sm hover:bg-red-500 sm:ml-3 sm:w-auto"
                  >
                    Delete
                  </button>
                  <button
                    onClick={() => handleCloseDeleteModal()}
                    type="button"
                    className="mt-3 inline-flex w-full justify-center rounded-md bg-white px-3 py-2 text-sm font-semibold text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 hover:bg-gray-50 sm:mt-0 sm:w-auto"
                  >
                    Cancel
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default ManageCourses;
