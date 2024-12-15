import instance from "../utils/axios"

// API để lấy userId dựa trên token
export const fetchUserId = async () => {
    try {
        const response = await instance.get('/user'); // Giả sử endpoint của bạn là /user
        return response.data.userId; // Trả về userId từ API
    } catch (error) {
        console.error('Error fetching userId:', error);
        throw error;
    }
};
