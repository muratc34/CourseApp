import publicClient from '../clients/publicClient';
import privateClient from '../clients/privateClient';

const createEndpoint = "/Courses";
const getEndpoint = (pageIndex, pageSize) => `/Courses?pageIndex=${pageIndex}&pageSize=${pageSize}`;
const getUpdateAndDeleteCourseEndpoint = (courseId) => `/Courses/${courseId}`;
const getCoursesByCategoryIdEndpoint = (categoryId) => `/Courses/Categories/${categoryId}`;
const getUserCoursesByUserIdEndpoint = (userId) => `/Courses/Users/${userId}`;
const uploadAndRemoveCourseImageEndpoint = (courseId) => `/Courses/UploadImage/${userId}`;

const courseApi = {
    createCourse: async(data) => {
        const response = await privateClient.post(createEndpoint, data);
        return response;
    },
    getCourses: async(pageIndex = 1, pageSize = 12) => {
        const response = await publicClient.get(getEndpoint(pageIndex, pageSize));
        return response;
    },
    updateCourse: async(courseId, data) => {
        const response = await privateClient.put(getUpdateAndDeleteCourseEndpoint(courseId), data);
        return response;
    },
    deleteCourse: async(courseId) => {
        const response = await privateClient.delete(getUpdateAndDeleteCourseEndpoint(courseId));
        return response;
    },
    getCourseById: async(courseId) => {
        const response = await publicClient.get(getUpdateAndDeleteCourseEndpoint(courseId));
        return response;
    },
    getCoursesByCategoryId: async(categoryId) => {
        const response = await publicClient.get(getCoursesByCategoryIdEndpoint(categoryId));
        return response;
    },
    getUserCoursesByUserId: async(userId) => {
        const response = await privateClient.get(getUserCoursesByUserIdEndpoint(userId));
        return response;
    },
    uploadCourseImage: async(courseId, data) => {
        const response = await privateClient.post(uploadAndRemoveCourseImageEndpoint(courseId), data);
        return response;
    },
    removeCourseImage: async(courseId) => {
        const response = await privateClient.delete(uploadAndRemoveCourseImageEndpoint(courseId));
        return response;
    }
}
export default courseApi;