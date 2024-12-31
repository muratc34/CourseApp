import publicClient from '../clients/publicClient';
import privateClient from '../clients/privateClient';

const createAndGetEndpoint = "/Courses";
const getUpdateAndDeleteCourseEndpoint = (courseId) => `/Courses/${courseId}`;
const getCoursesByCategoryIdEndpoint = (categoryId) => `/Courses/Categories/${categoryId}`;
const getUserCoursesByUserIdEndpoint = (userId) => `/Courses/Users/${userId}`;

const courseApi = {
    createCourse: async(data) => {
        const response = await privateClient.post(createAndGetEndpoint, data);
        return response;
    },
    getCourses: async() => {
        const response = await publicClient.get(createAndGetEndpoint);
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
    }
}
export default courseApi;