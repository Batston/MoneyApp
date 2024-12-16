<template>
    <v-app>
    <v-layout>
      <!-- App Bar -->
      <v-app-bar color="#00710F" prominent>
        <v-app-bar-nav-icon variant="text" @click.stop="drawer = !drawer" />
        <v-toolbar-title>
          Tổng cộng: 
          <span v-if="!showTotal">{{ formatCurrency(totalAmount) }}</span>
          <v-icon @click="showTotal = !showTotal">{{ showTotal ? 'mdi-eye-off' : 'mdi-eye' }}</v-icon>
        </v-toolbar-title>
      </v-app-bar>

      <!-- Navigation Drawer -->
      <v-navigation-drawer permanent v-model="drawer" :location="$vuetify.display.mobile ? 'bottom' : undefined" temporary>
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
      </v-navigation-drawer>



      <!-- Main Content -->
      <v-main>
        <v-container fluid>
          
        </v-container>
      </v-main>
    </v-layout>
  </v-app>
</template>
  
<script>
  import axios from '@/utils/axios';
  export default {
    name: 'TransactionCompnent',
    data: () => ({
        drawer: true,
        showTotal: true,
        drawerItems: [
            { title: "Dashboard", icon: "mdi-view-dashboard", route: '/home' },
            { title: "Sổ giao dịch", icon: "mdi-book", route: '/transaction' },
            { title: "Ngân sách", icon: "mdi-bank", route: '/budget' },
            { title: "Tài khoản", icon: "mdi-account", route: '/account' },
        ],
    }),
    methods: {
      fetchTransactions() {
        // Lấy danh sách transaction (giả sử API trả về JSON)
        axios
          .get(`/api/transactions?userId=${this.userId}`)
          .then((response) => {
            this.transactions = response.data;
          })
          .catch((error) => {
            console.error("Error fetching transactions:", error);
          });
      },
      addTransaction() {
        // Gửi yêu cầu thêm transaction
        axios
          .post(`/api/transactions`, {
            ...this.newTransaction,
            userId: this.userId,
          })
          .then(() => {
            this.fetchTransactions(); // Cập nhật danh sách
            this.showAddDialog = false; // Đóng dialog
          })
          .catch((error) => {
            console.error("Error adding transaction:", error);
          });
      },
      deleteTransaction(id) {
        // Xóa transaction
        axios
          .delete(`/api/transactions/${id}`)
          .then(() => {
            this.fetchTransactions(); // Cập nhật danh sách
          })
          .catch((error) => {
            console.error("Error deleting transaction:", error);
          });
      },
      isActiveRoute(route) {
        return this.$route.path === route;
      },
      onDrawerItemClick(item) {
        // Sử dụng Vue Router để điều hướng
        this.$router.push(item.route);
      },
    },
    mounted() {
      this.fetchTransactions();
    },
  };
</script>
<style scoped>
  .text-center {
    text-align: center;
  }
  .v-list-item--active {
    border-width: 1px;
    border-color: #00be19 !important;  /* Thêm màu nền khi chọn */
    color: #000 !important;  /* Thêm màu chữ khi chọn */
  }
</style>