import privateClient from '../clients/privateClient';

const createPaymentEndpoint = "/Payments";

const paymentApi = {
    createPayment: async(data) => {
        const response = await privateClient.post(createPaymentEndpoint, data)
        return response;
    }
}

export default paymentApi;