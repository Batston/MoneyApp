import instance from "./axios";

// Lấy danh sách giao dịch theo ID người dùng
export const getTransactions = async (userId) => {
    try {
        const response = await instance.get(`/Transaction/${userId}`);
        return response.data;
    } catch (error) {
        console.error('Error fetching transactions:', error);
        throw error;
    }
};