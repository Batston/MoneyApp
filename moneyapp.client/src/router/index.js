import { createRouter, createWebHistory } from 'vue-router';

import LoginComponent from '../components/Login.vue';
import HomeComponent from '../components/Home.vue';
import Transaction from '@/components/Transaction.vue';
import Register from '@/components/Register.vue';
import User from '@/components/User.vue';

const routes = [
    {
        path: '/login',
        component: LoginComponent
    },
    {
        path: '/register',
        component: Register
    },
    {
        path: '/home',
        component: HomeComponent,
        meta: { requiresAuth: true }
    },
    {
        path: '/transaction',
        component: Transaction,
        meta: { requiresAuth: true }
    },
    {
        path: '/info',
        component: User,
        meta: { requiresAuth: true }
    },

    // ❗ catch-all để cuối
    {
        path: '/:pathMatch(.*)*',
        redirect: '/login',
    }
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});


router.beforeEach((to, from, next) => {

    const token = localStorage.getItem("auth");

    const isAuthenticated =
        token && token !== "false" && token !== "null" && token !== "undefined";

    console.log("TO:", to.path);
    console.log("TOKEN:", token);
    console.log("AUTH:", isAuthenticated);

    // chưa login mà vào trang cần auth
    if (to.meta.requiresAuth && !isAuthenticated) {
        return next("/login");
    }

    // đã login mà vào login
    if (to.path === "/login" && isAuthenticated) {
        return next("/home");
    }

    return next();
});

export default router;