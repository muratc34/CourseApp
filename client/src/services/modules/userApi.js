import publicClient from '../clients/publicClient';
import privateClient from '../clients/privateClient';

const registerEndpoint = "/Users/Register";
const getPutAndDeleteUserEndpoint = (userId) => `/Users/${userId}`;

const userApi = {
    register: async(data) => {
        const response = await publicClient.post(registerEndpoint, data);
        return response;
    },
    updateUser: async(userId, data) => {
        const response = await privateClient.put(getPutAndDeleteUserEndpoint(userId), data);
        return response;
    },
    deleteUser: async(userId) => {
        const response = await privateClient.delete(getPutAndDeleteUserEndpoint(userId));
        return response;
    },
    getUserById: async(userId) => {
        const response = await publicClient.get(getPutAndDeleteUserEndpoint(userId));
        return response;
    }
}

export default userApi;