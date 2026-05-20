import axios from "axios";
import router from "@/router";

const instance = axios.create({
    baseURL: "https://localhost:7029",
});

instance.interceptors.request.use(
    function (config) {
        const auth = localStorage.getItem("auth");
        if (auth) {
            config.headers.Authorization = "Bearer " + auth;
        }
        return config;
    },
    function (error) {
        return Promise.reject(error);
    }
);

instance.interceptors.response.use(
    function (response) {
        return response;
    },
    function (error) {
        if (
            error.response &&
            (error.response.status === 401 || error.response.status === 403)
        ) {
            localStorage.removeItem("auth");
            if (router.currentRoute.value.path !== "/login") {
                router.replace("/login");
            }
        }
        return Promise.reject(error);
    }
);

export default instance;