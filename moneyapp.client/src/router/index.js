import { createRouter, createWebHistory } from 'vue-router';
import LoginComponent from '../components/Login.vue';
import HomeComponent from '../components/Home.vue';
import Transaction from '@/components/Transaction.vue';
import Register from '@/components/Register.vue';
import User from '@/components/User.vue';

const routes = [
    {
        path: '/register',
        name: 'RegisterComponent',
        component: Register,
    },
    {
        path: '/info',
        name: 'UserComponent',
        component: User,
        meta: { requiresAuth: true }
    },
    {
        path: '/transaction',
        name: 'TransactionComponent',
        component: Transaction,
        meta: { requiresAuth: true }
    },
    {
        path: '/home',
        name: 'HomeComponent',
        component: HomeComponent,
        meta: { requiresAuth: true }
    },
    {
        path: '/login',
        name: 'LoginComponent',
        component: LoginComponent
    },
    {
        path: '/:pathMatch(.*)*',
        redirect: '/login',
    }
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

// Middleware kiểm tra đăng nhập
router.beforeEach((to, from, next) => {
    const isAuthenticated = localStorage.getItem('auth'); // Hoặc kiểm tra token từ store

    if (to.meta.requiresAuth && !isAuthenticated) {
        // Nếu cần đăng nhập mà chưa đăng nhập, điều hướng về trang login
        alert('Bạn cần đăng nhập trước khi truy cập trang này');
        next('/login');
    }
    else {
        // Nếu đã đăng nhập hoặc không cần bảo mật, cho phép điều hướng
        next();
    }
});

export default router;