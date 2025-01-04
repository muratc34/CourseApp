import privateClient from '../clients/privateClient';

const createOrderEndpoint = "/Orders";
const getAndDeleteOrderEndpoint = (orderId) => `/Orders/${orderId}`;
const getOrderByUserEndpoint = (userId, pageIndex, pageSize) =>`/Orders/Users/${userId}?pageIndex=${pageIndex}&pageSize=${pageSize}`;

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
    getOrdersByUserId: async(userId, pageIndex = 1, pageSize = 12) => {
        const response = await privateClient.get(getOrderByUserEndpoint(userId, pageIndex, pageSize));
        return response;
    } 
}

export default orderApi;