import instance from "./axios";

// Lấy danh sách giao dịch
export const getTransactions = async () => {
    try {
        const response = await instance.get("/Transaction");
        return response.data;
    } catch (error) {
        console.error("Error fetching transactions:", error);
        throw error;
    }
};

// Thêm giao dịch mới
export const addTransaction = async (transaction) => {
    try {
        const response = await instance.post("/Transaction", transaction);
        return response.data;
    } catch (error) {
        console.error("Error adding transaction:", error);
        throw error;
    }
};

// Sửa giao dịch
export const updateTransaction = async (transaction) => {
    try {
        const response = await instance.put("/Transaction", transaction);
        return response.data;
    } catch (error) {
        console.error("Error updating transaction:", error);
        throw error;
    }
};

// Xóa giao dịch
export const deleteTransaction = async (transactionId) => {
    try {
        const response = await instance.delete(`/Transaction/${transactionId}`);
        return response.data;
    } catch (error) {
        console.error("Error deleting transaction:", error);
        throw error;
    }
};
