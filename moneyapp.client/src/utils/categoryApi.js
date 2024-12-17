import instance from "./axios";
// Lấy danh sách danh mục của người dùng
export const getCategories = async (id = 0) => {
    try {
        const response = await instance.get(`/Category/${id}`);
        return response.data;
    } catch (error) {
        console.error('Error getting categories:', error);
        throw error;
    }
};