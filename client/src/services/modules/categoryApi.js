import publicClient from '../clients/publicClient';

const getCategoriesEndpoint = "/Categories";

const categoryApi = {
    getCategories: async() => {
        const response = await publicClient.get(getCategoriesEndpoint);
        return response;
    }
}

export default categoryApi;