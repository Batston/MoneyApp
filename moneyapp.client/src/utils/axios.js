import axios from "axios";

// URL của trang 
const page = "http://localhost:8080";

const instance = axios.create({
    baseURL: "http://localhost:5039",
});

// Interceptor để cập nhật Authorization header trước mỗi request
instance.interceptors.request.use(
    function (config) {
        // Lấy token từ localStorage
        const auth = localStorage.getItem("auth");

        // Nếu có token, thêm vào header Authorization
        if (auth) {
            config.headers.Authorization = "Bearer " + auth;
        }

        return config;
    },
    function (error) {
        return Promise.reject(error);
    }
);

// Interceptor để xử lý lỗi phản hồi
instance.interceptors.response.use(
    function (response) {
        return response;
    },
    function (error) {
        // Nếu gặp lỗi 401 hoặc 403 (unauthorized/forbidden), chuyển hướng đến trang login
        if (
            error.response &&
            (error.response.status === 401 || error.response.status === 403)
        ) {
            if (location.pathname !== "/login") {
                location.href = `${page}/login`;
            }
        }
        return Promise.reject(error);
    }
);

export default instance;
