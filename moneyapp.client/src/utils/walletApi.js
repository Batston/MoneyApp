import instance from "./axios";

// Lấy ví của người dùng
export const getWallet = async () => {
    try {
        const response = await instance.get('/wallet');
        return response.data;
    } catch (error) {
        console.error('Error getting wallet:', error);
        throw error;
    }
};

// Thêm ví mới
export const addWallet = async (newWallet) => {
    try {
        const response = await instance.post('/Wallet', newWallet);
        return response.data;
    } catch (error) {
        console.error('Error adding wallet:', error);
        throw error;
    }
};

// Cập nhật ví
export const updateWallet = async (walletId, updateWallet) => {
    try {
        const response = await instance.put(`/wallet/${walletId}`, updateWallet);
        return response.data;
    } catch (error) {
        console.error('Error updating wallet:', error);
        throw error;
    }
};

// Xóa ví
export const deleteWallet = async (walletId) => {
    try {
        const response = await instance.delete(`/Wallet/${walletId}`);
        return response.data;
    } catch (error) {
        console.error('Error deleting wallet:', error);
        throw error;
    }
};
