import publicClient from '../clients/publicClient';
import privateClient from '../clients/privateClient';

const registerEndpoint = "/Users/Register";
const getPutAndDeleteUserEndpoint = (userId) => `/Users/${userId}`;
const uploadAndRemoveUserImageEndpoint = (userId) => `/Users/UploadImage/${userId}`;

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
    },
    uploadUserImage: async(userId, data) => {
        const response = await privateClient.post(uploadAndRemoveUserImageEndpoint(userId), data);
        return response;
    },
    removeUserImage: async(userId) => {
        const response = await privateClient.delete(uploadAndRemoveUserImageEndpoint(userId));
        return response;
    }
}

export default userApi;