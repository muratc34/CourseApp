import privateClient from '../clients/privateClient';

const createOrderEndpoint = "/Orders";
const getAndDeleteOrderEndpoint = (orderId) => `/Orders/${orderId}`;
const getOrderByUserEndpoint = (userId) =>`/Orders/Users/${userId}`;

const orderApi = {
    createOrder: async(data) =>{
        const response = await privateClient.post(createOrderEndpoint, data);
        return response;
    },
    deleteOrder: async(orderId) => {
        const response = await privateClient.delete(getAndDeleteOrderEndpoint(orderId));
        return response;
    },
    getOrderById: async(orderId) => {
        const response = await privateClient.get(getAndDeleteOrderEndpoint(orderId));
        return response;
    },
    getOrdersByUserId: async(userId) => {
        const response = await privateClient.get(getOrderByUserEndpoint(userId));
        return response;
    } 
}

export default orderApi;