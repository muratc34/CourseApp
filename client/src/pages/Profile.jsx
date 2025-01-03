import React, { useEffect, useState } from "react";
import { useAuth } from "../contexts/AuthContext";
import userApi from "../services/modules/userApi";
import defaultUserPic from "/src/assets/default-user.png";
import LoadingSpinner from "../components/LoadingSpinner";
import { IoClose, IoCloudUploadSharp, IoCreate } from "react-icons/io5";
import orderApi from "../services/modules/orderApi";
import courseApi from "../services/modules/courseApi";
import Pagination from "../components/Pagination";
import axios from "axios";
import config from "../services/configs/config";
import authApi from "../services/modules/authApi";
import CourseCard from "../components/CourseCard";

const Profile = () => {
  const [isLoading, setIsLoading] = useState(true);
  const [isModalOpen, setModalOpen] = useState(false);
  const [user, setUser] = useState({});
  const [userOrders, setUserOrders] = useState([]);
  const [userCourses, setUserCourses] = useState([]);
  const [userPicture, setUserPicture] = useState();
  const [uploadFile, setUploadFile] = useState();
  const [formData, setFormData] = useState({
    firstName: "",
    lastName: "",
    email: "",
    userName: "",
  });
  const [currentPassword, setCurrentPassword] = useState("");
  const [newPassword, setNewPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");

  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const pageSize = 3;

  const authUser = JSON.parse(localStorage.getItem("user"));

  const handleImageUpload = (event) => {
    const file = event.target.files[0];
    if (file) {
      setUploadFile(file);
      setUserPicture(URL.createObjectURL(file));
    }
  };

  const handleInputChange = (event) => {
    const { name, value } = event.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const getUserProfileData = async () => {
    userApi
      .getUserById(authUser.id)
      .then((response) => {
        setUser(response.data);
        setUserPicture(response.data.profilePictureUrl);
        const parts = response.data.fullName.trim().split(/\s+/);
        setFormData({
          firstName: parts[0],
          lastName: parts[1],
          email: response.data.email,
          userName: response.data.userName,
        });
      })
      .catch((error) => {
        console.error("Error fetching user data:", error);
      });
  };

  const getUserOrdersData = async () => {
    orderApi
      .getOrdersByUserId(authUser.id, currentPage, pageSize)
      .then((response) => {
        setUserOrders(response.data.items);
        setTotalPages(response.data.totalPages);
      })
      .catch((error) => {
        console.error("Error fetching orders data:", error);
      });
  };

  const getUserCoursesData = async () => {
    courseApi
      .getUserCoursesByEnrollmentUserId(authUser.id)
      .then((response) => {
        console.log(response)
        setUserCourses(response.data);
      })
      .catch((error) => {
        console.error("Error fetching courses data:", error);
      });
  };

  const getAllData = () => {
    setIsLoading(true);
    Promise.all([
      getUserProfileData(),
      getUserOrdersData(),
      getUserCoursesData(),
    ])
      .then(() => {
        setIsLoading(false);
      })
      .catch((error) => {
        setIsLoading(false);
        console.error("Error fetching all data:", error);
      });
  };

  useEffect(() => {
    getAllData();
  }, []);

  useEffect(() => {
    getUserOrdersData();
  }, [currentPage]);

  const dateFormatter = (unixDate) => {
    const date = new Date(unixDate * 1000);

    return date.toLocaleString("en-US", {
      timeZone: "UTC",
      year: "numeric",
      month: "long",
      day: "numeric",
      hour: "2-digit",
      minute: "2-digit",
      hour12: false,
    });
  };

  const handlePageChange = (page) => {
    setCurrentPage(page);
  };

  const updateUser = () => {
    userApi
      .updateUser(authUser.id, {
        firstName: formData.firstName,
        lastName: formData.lastName,
        email: formData.email,
        userName: formData.userName,
      })
      .then(() => {
        alert("Success update user data");
      })
      .catch((error) => {
        console.error("Error updating profile:", error);
      });
  };

  const uploadUserImg = async () => {
    if (!uploadFile) {
      return;
    }

    const formData = new FormData();
    formData.append("formFile", uploadFile);

    await axios
      .post(`${config.baseURL}/Users/UploadImage/${authUser.id}`, formData, {
        headers: {
          "Content-Type": "multipart/form-data",
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
      })
      .then((response) => {
        console.log("dosya yüklendi", response);
      })
      .catch((error) => {
        console.error(error);
      });
  };

  const saveUserProfile = (e) => {
    e.preventDefault();
    updateUser();
    uploadUserImg();
    setModalOpen(false);
    getAllData();
  };

  const changePassword = (e) => {
    e.preventDefault();
    if(newPassword !== confirmPassword)
    {
      console.log("şifreler uyuşmuyor")
      return;
    }
    authApi.changePassword(authUser.id, {oldPassword: currentPassword, newPassword: newPassword})
    .then(response => {
      console.log(response);
      setCurrentPassword("");
      setNewPassword("");
      setConfirmPassword("");
    }).catch(error => {
      console.error(error);
    })
  }

  if (isLoading) {
    return <LoadingSpinner />;
  }

  return (
    <section className="bg-white py-8 antialiased">
      <div className="mx-auto max-w-screen-lg px-4 2xl:px-0">
        <h2 className="mb-4 text-xl font-semibold text-gray-900 sm:text-2xl md:mb-6">
          General overview
        </h2>
        <div className="py-4 md:py-8">
          <div className="mb-4 grid gap-4 sm:grid-cols-2 sm:gap-8 lg:gap-16">
            <div className="space-y-4">
              <div className="flex space-x-4">
                <img
                  className="h-16 w-16 rounded-full"
                  src={user.profilePictureUrl ?? defaultUserPic}
                  alt={user.fullName}
                />
                <div>
                  <h2 className="flex items-center text-xl font-bold leading-none text-gray-900 sm:text-2xl">
                    {user.fullName}
                  </h2>
                </div>
              </div>
              <dl className="">
                <dt className="font-semibold text-gray-900">Register Date</dt>
                <dd className="text-gray-500">
                  {dateFormatter(user.createdOnUtc)}
                </dd>
              </dl>
              <dl className="">
                <dt className="font-semibold text-gray-900">Email Address</dt>
                <dd className="text-gray-500">{user.email}</dd>
              </dl>
              <dl className="">
                <dt className="font-semibold text-gray-900">Username</dt>
                <dd className="text-gray-500">{user.userName}</dd>
              </dl>
              <button
                type="button"
                onClick={() => setModalOpen(true)}
                className="inline-flex w-full items-center justify-center rounded-lg bg-indigo-700 px-5 py-2.5 text-sm font-medium text-white hover:bg-indigo-800 focus:outline-none focus:ring-4 focus:ring-primary-300 sm:w-auto"
              >
                <IoCreate className="-ms-0.5 me-1.5 h-4 w-4" />
                Edit your data
              </button>
            </div>
            <div className="space-y-4">
              <h2 className="flex items-center text-xl font-bold leading-none text-gray-700 sm:text-2xl">Change Your Password</h2>
              <form onSubmit={(e) => changePassword(e)} className="p-4 md:p-5">
                <div className="mb-2">
                  <div className="flex items-center justify-between">
                    <label
                      htmlFor="currentPassword"
                      className="block text-sm/6 font-medium text-gray-900"
                    >
                      Current Password
                    </label>
                  </div>
                  <div className="mt-2">
                    <input
                      value={currentPassword}
                      onChange={(e) => setCurrentPassword(e.target.value)}
                      name="currentPassword"
                      type="password"
                      required
                      placeholder="Enter your current password"
                      className="block w-full rounded-md bg-white px-3 py-1.5 text-base text-gray-900 outline outline-1 -outline-offset-1 outline-gray-300 placeholder:text-gray-400 focus:outline focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-600 sm:text-sm/6"
                    />
                  </div>
                </div>

                <div className="mb-2">
                  <div className="flex items-center justify-between">
                    <label
                      htmlFor="newPassword"
                      className="block text-sm/6 font-medium text-gray-900"
                    >
                      New Password
                    </label>
                  </div>
                  <div className="mt-2">
                    <input
                      value={newPassword}
                      onChange={(e) => setNewPassword(e.target.value)}
                      name="newPassword"
                      type="password"
                      required
                      placeholder="Enter your current password"
                      className="block w-full rounded-md bg-white px-3 py-1.5 text-base text-gray-900 outline outline-1 -outline-offset-1 outline-gray-300 placeholder:text-gray-400 focus:outline focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-600 sm:text-sm/6"
                    />
                  </div>
                </div>

                <div className="mb-2">
                  <div className="flex items-center justify-between">
                    <label
                      htmlFor="confirmPassword"
                      className="block text-sm/6 font-medium text-gray-900"
                    >
                      Confirm password
                    </label>
                  </div>
                  <div className="mt-2">
                    <input
                      value={confirmPassword}
                      onChange={(e) => setConfirmPassword(e.target.value)}
                      name="confirmPassword"
                      type="password"
                      required
                      placeholder="Enter your current password"
                      className="block w-full rounded-md bg-white px-3 py-1.5 text-base text-gray-900 outline outline-1 -outline-offset-1 outline-gray-300 placeholder:text-gray-400 focus:outline focus:outline-2 focus:-outline-offset-2 focus:outline-indigo-600 sm:text-sm/6"
                    />
                  </div>
                </div>

                <div>
                  <button
                    type="submit"
                    className="flex w-full items-center justify-center rounded-lg bg-indigo-700 mt-5 px-5 py-2.5 text-sm font-medium text-white hover:bg-indigo-800 focus:outline-none focus:ring-4 focus:ring-primary-300"
                  >
                    Change Password
                  </button>
                </div>
              </form>
            </div>
          </div>
        </div>
        <div className="rounded-lg border border-gray-200 bg-gray-50 p-4 md:p-8 min-h-min">
          <h3 className="mb-4 text-xl font-semibold text-gray-900">Orders</h3>
          {userOrders.map((order) => (
            <div
              key={order.id}
              className="flex flex-wrap items-start gap-y-4 gap-x-10 border-b border-gray-200 pb-4 md:pb-5"
            >
              <dl className="w-1/2 sm:w-48">
                <dt className="text-base font-medium text-gray-500">
                  Order ID:
                </dt>
                <dd className="mt-1.5 text-base font-semibold text-gray-900 ">
                  {order.id}
                </dd>
              </dl>

              <dl className="w-1/2 sm:w-1/4 md:flex-1 lg:w-auto">
                <dt className="text-base font-medium text-gray-500">Date:</dt>
                <dd className="mt-1.5 text-base font-semibold text-gray-900">
                  {dateFormatter(order.createdOnUtc)}
                </dd>
              </dl>

              <dl className="w-1/2 sm:w-1/5 md:flex-1 lg:w-auto">
                <dt className="text-base font-medium text-gray-500 ">Price:</dt>
                <dd className="mt-1.5 text-base font-semibold text-gray-900">
                  {order.amount} ₺
                </dd>
              </dl>

              <dl className="w-1/2 sm:w-1/4 sm:flex-1 lg:w-auto">
                <dt className="text-base font-medium text-gray-500 ">
                  Status:
                </dt>
                <dd
                  className={`me-2 mt-1.5 inline-flex shrink-0 items-center rounded px-2.5 py-0.5 text-xs font-medium bg-yellow-100 text-yellow-800`}
                >
                  {order.status}
                </dd>
              </dl>
            </div>
          ))}
          <Pagination
            totalPages={totalPages}
            currentPage={currentPage}
            onPageChange={handlePageChange}
          />
        </div>
        <div className="mt-6 grid grid-cols-1 md:grid-cols-2 gap-x-6 gap-y-10 xl:grid-cols-3 xl:gap-x-8 mb-6 rounded-lg border border-gray-200 p-4 md:p-8 min-h-min mt-10">
          {
            userCourses.map(course => (
              <div key={course.id}>
                <CourseCard course={course}/>
              </div>
            ))
          }
        </div>

        {/* Account Information Modal */}
        {isModalOpen && (
          <div className="max-h-auto left-0 right-0 top-0 z-50 flex max-h-full w-full items-center justify-center overflow-y-auto overflow-x-hidden antialiased fixed inset-0 bg-gray-500/75 transition-opacity">
            <div className="max-h-auto relative max-h-full w-full max-w-lg p-4">
              <div className="relative rounded-lg bg-white shadow">
                <div className="flex items-center justify-between rounded-t border-b border-gray-200 p-4">
                  <h3 className="text-lg font-semibold text-gray-900">
                    Account Information
                  </h3>
                  <button
                    type="button"
                    onClick={() => setModalOpen(false)}
                    className="ms-auto inline-flex h-8 w-8 items-center justify-center rounded-lg bg-transparent text-sm text-gray-400 hover:bg-gray-200 hover:text-gray-900"
                  >
                    <IoClose size={24} />
                  </button>
                </div>
                <form
                  onSubmit={(e) => saveUserProfile(e)}
                  className="p-4 md:p-5"
                >
                  <div className="mb-5 grid grid-cols-1 gap-4 sm:grid-cols-2">
                    <div className="col-span-2">
                      <div className="flex flex-col items-center justify-center">
                        <img
                          className="h-16 w-16 rounded-full"
                          src={userPicture || defaultUserPic}
                          alt={user.fullName}
                        />
                        <label
                          htmlFor="dropzone-file"
                          className="flex flex-row items-center justify-center my-5 font-semibold gap-1 py-2 px-4 cursor-pointer bg-indigo-600 text-white rounded-lg select-none"
                        >
                          <IoCloudUploadSharp className="w-8 h-8 text-white me-2" />
                          Change Avatar
                        </label>
                        <input
                          id="dropzone-file"
                          type="file"
                          accept="image/*"
                          className="hidden"
                          onChange={handleImageUpload}
                        />
                      </div>
                    </div>
                    <div className="col-span-2 sm:col-span-1">
                      <label className="mb-2 block text-sm font-medium text-gray-900">
                        {" "}
                        Your First Name*{" "}
                      </label>
                      <input
                        value={formData.firstName}
                        onChange={handleInputChange}
                        type="text"
                        name="firstName"
                        className="block w-full rounded-lg border border-gray-300 bg-gray-50 p-2.5 text-sm text-gray-900 focus:border-primary-500 focus:ring-primary-500"
                        placeholder="Enter your first name"
                        required
                      />
                    </div>
                    <div className="col-span-2 sm:col-span-1">
                      <label className="mb-2 block text-sm font-medium text-gray-900">
                        {" "}
                        Your Last Name*{" "}
                      </label>
                      <input
                        value={formData.lastName}
                        onChange={handleInputChange}
                        type="text"
                        name="lastName"
                        className="block w-full rounded-lg border border-gray-300 bg-gray-50 p-2.5 text-sm text-gray-900 focus:border-primary-500 focus:ring-primary-500"
                        placeholder="Enter your last name"
                        required
                      />
                    </div>
                    <div className="col-span-2 sm:col-span-1">
                      <label className="mb-2 block text-sm font-medium text-gray-900">
                        {" "}
                        Your Email*{" "}
                      </label>
                      <input
                        value={formData.email}
                        onChange={handleInputChange}
                        type="email"
                        name="email"
                        className="block w-full rounded-lg border border-gray-300 bg-gray-50 p-2.5 text-sm text-gray-900 focus:border-primary-500 focus:ring-primary-500"
                        placeholder="Enter your email here"
                        required
                      />
                    </div>
                    <div className="col-span-2 sm:col-span-1">
                      <label className="mb-2 block text-sm font-medium text-gray-900">
                        {" "}
                        Username*{" "}
                      </label>
                      <input
                        value={formData.userName}
                        onChange={handleInputChange}
                        type="text"
                        name="userName"
                        className="block w-full rounded-lg border border-gray-300 bg-gray-50 p-2.5 text-sm text-gray-900 focus:border-primary-500 focus:ring-primary-500"
                        placeholder="Enter your username here"
                        required
                      />
                    </div>
                  </div>
                  <div className="border-t border-gray-200 pt-4 md:pt-5">
                    <button
                      type="submit"
                      className="me-2 rounded-lg border border-gray-200 bg-indigo-600 px-5 py-2.5 text-sm font-medium text-white hover:bg-indigo-700 focus:z-10 focus:outline-none focus:ring-4 focus:ring-gray-100"
                    >
                      Save information
                    </button>
                  </div>
                </form>
              </div>
            </div>
          </div>
        )}
      </div>
    </section>
  );
};

export default Profile;
