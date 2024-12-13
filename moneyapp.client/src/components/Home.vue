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
      <v-navigation-drawer v-model="drawer" :location="$vuetify.display.mobile ? 'bottom' : undefined" temporary>
        <v-list>
          <v-list-item v-for="(item, index) in drawerItems" :key="index" @click="onDrawerItemClick(item)">
            <v-list-item-title>{{ item }}</v-list-item-title>
          </v-list-item>
        </v-list>
      </v-navigation-drawer>

      <!-- Main Content -->
      <v-main>
        <v-container>
          <v-row>
            <!-- Layout 1: Báo cáo thống kê -->
            <v-col cols="12" md="6">
              <v-card>
                <v-card-title>Báo cáo thống kê</v-card-title>
                <v-card-text>
                  <!-- Placeholder for chart -->
                  <v-skeleton-loader type="card" />
                  <p class="text-center">Biểu đồ sẽ được hiển thị ở đây.</p>
                </v-card-text>
              </v-card>
            </v-col>

            <!-- Layout 2: Giao dịch gần đây -->
            <v-col cols="12" md="6">
              <v-card>
                <v-card-title>Giao dịch gần đây</v-card-title>
                <v-card-text>
                  <v-list>
                    <v-list-item 
                      v-for="transaction in transactions"
                      :key="transaction.transaction.transactionID"
                      @click="handleTransactionClick(transaction)" 
                    >
                      <v-list-item-content>
                        <div class="d-flex justify-space-between align-center mt-8">
                          <!-- Bên trái: Icon và tiêu đề -->
                          <div class="d-flex align-center">
                            <!-- <v-list-item-avatar>
                              <v-icon color="#00710F">mdi-cash</v-icon> 
                            </v-list-item-avatar> -->
                            <div>
                              <v-list-item-title class="text-h6 font-weight-bold">Tên của categorycategory</v-list-item-title>
                              <v-list-item-subtitle>
                                {{ formatDate(transaction.transaction.transactionDate) }}
                              </v-list-item-subtitle>
                            </div>
                          </div>
                          <!-- Bên phải: Giá tiền -->
                          <strong class="text-right">{{ formatCurrency(transaction.transaction.amount) }}</strong>
                        </div>
                      </v-list-item-content>
                    </v-list-item>
                  </v-list>
                </v-card-text>
              </v-card>
            </v-col>

          </v-row>

          <!-- Layout 3: Danh sách ví -->
          <v-row>
            <v-col cols="12">
              <v-card>
                <v-card-title>Danh sách ví</v-card-title>
                <v-card-text>
                  <v-list>
                    <v-list-item v-for="wallet in wallets" :key="wallet.walletID">
                      <v-list-item-title>{{ wallet.walletName }}</v-list-item-title>
                      <v-list-item-subtitle>
                        Số dư: {{ formatCurrency(wallet.balance) }}
                      </v-list-item-subtitle>
                    </v-list-item>
                  </v-list>
                </v-card-text>
              </v-card>
            </v-col>
          </v-row>
        </v-container>
      </v-main>
    </v-layout>
  </v-app>
</template>

<script>
  import axios from "../utils/axios";
  import { getWallet } from "@/utils/walletApi";
  export default {
    name: "HomeComponent",
    data: () => ({
      drawer: false,
      showTotal: true,
      totalAmount: "", // Giả lập số dư tổng cộng
      transactions: [],
      wallets: [],
      drawerItems: ["Sổ giao dịch", "Ngân sách", "Tài khoản"],
    }),
    mounted() {
      this.getTransactions();
      this.fetchWallets();
    },
    methods: {
      // Lấy danh sách ví
      async fetchWallets() {
        try {
          const data = await getWallet();
          this.wallets = data;
          // Tính tổng số dư của tất cả ví
          this.totalAmount = this.wallets.reduce((total, wallet) => total + wallet.balance, 0);
        } catch (error) {
          this.message = 'Không thể lấy thông tin ví!';
        }
      },
      getTransactions() {
        const userId = localStorage.getItem('userId'); // Lấy userId từ localStorage
        console.log(userId);
        if (userId) {
            axios
              .get(`/Transaction/${userId}`) // Truyền userId vào URL
              .then((res) => {
                console.log("Kết quả API:", res);
                this.transactions = res.data; // Gán kết quả trả về
              })
              .catch((error) => {
                console.error("Lỗi khi lấy dữ liệu giao dịch:", error);
              });
        } else {
            console.error("User ID không tồn tại");
        }
      },

      formatCurrency(amount) {
        return new Intl.NumberFormat("vi-VN", { style: "currency", currency: "VND" }).format(amount);
      },
      formatDate(date) {
        const options = { year: 'numeric', month: 'long', day: 'numeric' };
        return new Date(date).toLocaleDateString('vi-VN', options);
      },
      onDrawerItemClick(item) {
        console.log("Clicked on drawer item:", item);
        // Xử lý điều hướng hoặc hành động tương ứng
      },
    },
  };
</script>

<style scoped>
  .text-center {
    text-align: center;
  }
</style>
