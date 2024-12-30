import publicClient from '../clients/publicClient';
import privateClient from '../clients/privateClient';

const loginEndpoint = "/Authentication/Login";
const changePasswordEndpoint = (userId) => `/Authentication/ChangePassword/${userId}`;
const createTokenByRefreshTokenEndpoint = "/Authentication/CreateTokenByRefreshToken";
const exterminateRefreshTokenEndpoint = "/Authentication/ExterminateRefreshToken";
const confirmEmailEndpoint = "/Authentication/ConfirmEmail";
const resendEmailConfirmationTokenEndpoint = "/Authentication/ResendEmailConfirmationToken";

const authApi = {
    login: async (data) => {
        const response = await publicClient.post(loginEndpoint, data);
        return response;
    },
    changePassword: async(userId, data) => {
        const response = await privateClient.patch(changePasswordEndpoint(userId), data);
        return response;
    },
    createTokenByRefreshToken: async(data) => {
        const response = await publicClient.post(createTokenByRefreshTokenEndpoint, data);
        return response;
    },
    exterminateRefreshToken: async(data) => {
        const response = await publicClient.post(exterminateRefreshTokenEndpoint, data);
        return response;
    },
    confirmEmail: async(data) => {
        const response = await privateClient.post(confirmEmailEndpoint, data);
        return response;
    },
    resendEmailConfirmationToken: async(data) => {
        const response = await privateClient.post(resendEmailConfirmationTokenEndpoint, data);
        return response;
    }
}
export default authApi;