<template>
  <v-app>
    <v-main>
      <router-view></router-view>
      <!-- <Home></Home> -->
    </v-main>
  </v-app>
</template>

<script>
import { useRouter } from 'vue-router';

export default {
    setup() {
        const router = useRouter();

        // Kiểm tra trạng thái khi app load
        const isAuthenticated = localStorage.getItem('auth') !== null;

        if (!isAuthenticated && router.currentRoute.value.path !== '/login') {
            router.push('/login'); // Điều hướng về login nếu chưa đăng nhập
        }
    },
};
</script>
