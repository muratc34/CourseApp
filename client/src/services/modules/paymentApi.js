import privateClient from '../clients/privateClient';

const createPaymentEndpoint = "/Payments";
const confirmPaymentEndpoint = "/Payments/CheckoutConfirm";

const paymentApi = {
    createPayment: async(data) => {
        const response = await privateClient.post(createPaymentEndpoint, data);
        return response;
    },
    confirmPayment: async(data) => {
        const response = await privateClient.post(confirmPaymentEndpoint, data);
        return response;
    }
}

export default paymentApi;