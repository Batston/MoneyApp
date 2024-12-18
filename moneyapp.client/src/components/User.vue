<template>
    <v-app>
      <v-layout>
        <!-- App Bar -->
        <v-app-bar color="#00710F" prominent app fixed>
          <v-app-bar-nav-icon variant="text" @click.stop="drawer = !drawer" />
          <v-toolbar-title class="text-h5 font-weight-light">
            Tổng cộng: 
            <span v-if="!showTotal">{{ formatCurrency(totalAmount) }}</span>
            <v-icon 
              @click="showTotal = !showTotal" 
              class="ml-2 cursor-pointer"
              :style="{ transform: showTotal  }"
            >
              {{ showTotal ? 'mdi-eye-off' : 'mdi-eye' }}
            </v-icon>
          </v-toolbar-title>
        </v-app-bar>
  
  
        <!-- Navigation Drawer -->
        <!-- <v-navigation-drawer permanent v-model="drawer" :location="$vuetify.display.mobile ? 'bottom' : undefined" temporary>
          <v-list>
            <v-list-item 
              v-for="(item, index) in drawerItems" 
              :key="index" 
              @click="onDrawerItemClick(item)"
              :class="{'v-list-item--active': isActiveRoute(item.route)}"
            >
              <div class="d-flex justify align-center">
                <v-list-item-icon>
                  <v-icon color="#00710F">{{ item.icon }}</v-icon>
                </v-list-item-icon>
                <v-list-item-content>
                  <v-list-item-title class="ma-3">{{ item.title }}</v-list-item-title>
                </v-list-item-content>
              </div>
            </v-list-item>
          </v-list>
        </v-navigation-drawer> -->
        <v-navigation-drawer app permanent v-model="drawer">
          <v-list dense>
            <v-list-item
              v-for="(item, index) in drawerItems"
              :key="index"
              @click="onDrawerItemClick(item)"
              :class="{ 'v-list-item--active': isActiveRoute(item.route) }"
              class="hoverable"
            >
              <div class="d-flex justify align-center">
                <v-list-item-icon>
                  <v-icon color="#00710F">{{ item.icon }}</v-icon>
                </v-list-item-icon>
                <v-list-item-content>
                  <v-list-item-title class="font-weight-light ma-3">{{ item.title }}</v-list-item-title>
                </v-list-item-content>
              </div>
            </v-list-item>
          </v-list>
        </v-navigation-drawer>
  
  
  
  
        <!-- Main Content -->
        <v-main>
            <v-container fluid class="d-flex align-center justify-center" style="height: 100%;">
                <v-card class="ma-3 d-flex align-center justify-center" elevation="2" style="max-width: 600px; width: 100%; height: 80%;">
                    <div style="height: 80%; width: auto;">
                        <v-card-media>
                        <v-img
                            :aspect-ratio="1"
                            class="bg-white mx-auto"
                            src="../assets/moneyLogo.png"
                            width="100"
                            cover
                        ></v-img>
                        </v-card-media>
                        <v-card-title class="text-h3 font-weight-light text-center mt-12">{{ userName }}</v-card-title>
                        <v-card-subtitle class="text-center">{{ email }}</v-card-subtitle>
                        <v-card-actions class="mt-12 d-flex justify-center">
                            <v-btn height="48" block style="background-color: #00710F; color: white;" elevation="2" class="mb-3" @click="handleLogout">
                                LOGOUT
                            </v-btn>
                        </v-card-actions>
                    </div>
                </v-card>
                <!-- dialog message -->
                <v-dialog v-model="dialog" max-width="500px">
                    <v-card>
                    <v-card-title class="headline">Thông báo</v-card-title>
                    <v-card-text>
                        <div class="text-center">
                        <!-- Biểu tượng thay đổi dựa trên trạng thái -->
                        <v-icon :color="dialogType === 'error' ? 'red' : 'success'" size="128">
                            {{ dialogType === 'error' ? 'mdi-alert-circle-outline' : 'mdi-check-circle-outline' }}
                        </v-icon>
                        <p class="text-h5 font-weight-bold">{{ dialogMessage }}</p>
                        </div>
                    </v-card-text>
                    <v-card-actions>
                        <v-spacer></v-spacer>
                        <v-btn color="green darken-1" @click="dialog = false">Đóng</v-btn>
                    </v-card-actions>
                    </v-card>
                </v-dialog>
            </v-container>
        </v-main>
      </v-layout>
    </v-app>
  </template>
  
  <script>
  import instance from '@/utils/axios';
    export default {
      name: "UserComponent",
      data: () => ({
        userName: '',
        email: '',
        drawer: true,
        showTotal: true,
        IdNguoiDung: '',
        totalAmount: "", // Giả lập số dư tổng cộng
        drawerItems: [
          { title: "Dashboard", icon: "mdi-view-dashboard", route: '/home' },
          { title: "Sổ giao dịch", icon: "mdi-book", route: '/transaction' },
          { title: "Tài khoản", icon: "mdi-account", route: '/info' },
        ],
        dialog: false,
        dialogMessage: '',
        dialogType: "",
        disablebtn: true,
      }),
      mounted() {
        this.fetchUserInfo();
      },
      methods: {
        showMessage(message, type) {
            this.dialogMessage = message;
            this.dialogType = type; // 'success' hoặc 'error'
            this.dialog = true; // Mở dialog
        },
        fetchUserInfo() {
        instance.get('/User/info')
            .then(response => {
                this.userName = response.data.username 
                this.email = response.data.email 
            })
            .catch(error => {
                console.error('There was a problem with the fetch operation:', error);
            });
        },
        handleLogout() {
            localStorage.removeItem('auth'); // Xóa token
            this.showMessage('Lougout success', 'success');
            this.$router.push('/login'); // Chuyển hướng đến trang đăng nhập
        },
        isActiveRoute(route) {
            return this.$route.path === route;
        },
        onDrawerItemClick(item) {
            // Sử dụng Vue Router để điều hướng
            this.$router.push(item.route);
        }, 
      },
  }
  </script>
  
  <style scoped>
  .user-logo {
    width: 100px; /* Kích thước logo */
    height: 100px; /* Kích thước logo */
    border-radius: 50%; /* Hình tròn */
    object-fit: cover; /* Cắt hình ảnh đúng kích thước */
    margin: 0 auto; /* Căn giữa */
    }
    .v-card {
    max-width: 300px; /* Độ rộng tối đa của card */
    text-align: center; /* Căn giữa văn bản */
    }
    .text-center {
      text-align: center;
    }
    .hoverable:hover {
    background-color: #E8F5E9;
    transition: background-color 0.3s ease;
    cursor: pointer;
    }
    .v-list-item--active {
      background-color: #A5D6A7 !important;
      color: #00710F;
    }
    body {
      background-color: #F7FAFC;
    }
    .v-card {
      transition: all 0.3s ease;
    }
    .v-card:hover {
      transform: translateY(-5px);
      box-shadow: 0 10px 20px rgba(0, 0, 0, 0.1);
    }
  </style>
  