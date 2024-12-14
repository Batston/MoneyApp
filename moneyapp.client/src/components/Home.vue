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
              <v-card hover elevation="1">
                <v-card-title style="border-bottom: 1px solid #00710F; padding-bottom: 8px;">Báo cáo thống kê</v-card-title>
                <v-card-text>
                  <!-- Placeholder for chart -->
                  <v-skeleton-loader type="card" />
                  <p class="text-center">Biểu đồ sẽ được hiển thị ở đây.</p>
                </v-card-text>
              </v-card>
            </v-col>

            <!-- Layout 2: Giao dịch gần đây -->
            <v-col cols="12" md="6">
              <v-card hover elevation="1">
                <v-card-title style="border-bottom: 1px solid #00710F; padding-bottom: 8px;">Giao dịch gần đây</v-card-title>
                <v-card-text>
                  <v-list>
                    <v-list-item 
                      v-for="transaction in transactions"
                      :key="transaction.transaction.transactionID"
                      @click="handleTransactionClick(transaction)" 
                    >
                      <v-list-item-content>
                        <div class="d-flex justify-space-between align-center mt-4">
                          <!-- Bên trái: Icon và tiêu đề -->
                          <div>
                            <div class="d-flex align-center">
                              <v-icon color="#00710F" size="x-large">mdi-cash</v-icon> 
                              <v-list-item-title class="text-h6 font-weight-bold ml-2">Tên của categorycategory</v-list-item-title>
                            </div>
                            <v-list-item-subtitle>
                              {{ formatDate(transaction.transaction.transactionDate) }}
                            </v-list-item-subtitle>
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
              <v-card hover elevation="1">
                <div class="d-flex align-center" style="border-bottom: 1px solid #00710F; padding-bottom: 8px;">
                  <v-card-title class="mr-auto">Danh sách ví</v-card-title>
                  <v-btn class="text-right" color="#00710F" style="padding: 8px 16px; margin-right: 16px;" @click="openAddWallet">
                    Thêm ví
                  </v-btn>
                </div>
                <v-card-text>
                  <v-list class="d-flex" style="flex-wrap: wrap;"> <!-- Cho phép các item xếp thành hàng ngang -->
                    <v-list-item class="d-flex align-center" v-for="wallet in wallets" :key="wallet.walletID" style="margin-right: 16px;">
                      <div class="d-flex">
                        <v-icon size="x-large" color="#00710F">mdi-wallet</v-icon>
                        <v-list-item-title class="ml-2">{{ wallet.walletName }}</v-list-item-title>
                      </div>
                      <v-list-item-subtitle class="ml-2">
                        Số dư: {{ formatCurrency(wallet.balance) }}
                      </v-list-item-subtitle>
                    </v-list-item>
                  </v-list>
                </v-card-text>
              </v-card>
            </v-col>
          </v-row>
          <!-- Dialog Thêm ví -->
          <v-dialog v-model="showAddWallet" max-width="500px">
            <v-card>
              <v-card-title class="headline">Thêm ví mới</v-card-title>
              <v-card-text>
                <v-text-field v-model="newWallet.name" label="Tên ví" required></v-text-field>
                <v-text-field v-model="newWallet.balance" label="Số dư" type="c" required></v-text-field>
              </v-card-text>
              <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="green darken-1" @click="handleAddWallet">Thêm</v-btn>
                <v-btn color="red darken-1" @click="showAddWallet = false">Hủy</v-btn>
              </v-card-actions>
            </v-card>
          </v-dialog>
        </v-container>
      </v-main>
    </v-layout>
  </v-app>
</template>

<script>
  import axios from "../utils/axios";
  import { getWallet, addWallet } from "@/utils/walletApi";
  import { getTransactions } from "@/utils/transactionApi";

  export default {
    name: "HomeComponent",
    data: () => ({
      showAddWallet: false,
      drawer: false,
      showTotal: true,
      IdNguoiDung: '',
      totalAmount: "", // Giả lập số dư tổng cộng
      transactions: [],
      wallets: [],
      newWallet: {
        userId: '',
        name: '',
        balance: ''
      },
      drawerItems: ["Sổ giao dịch", "Ngân sách", "Tài khoản"],
    }),
    mounted() {
      this.fetchTransactions();
      this.fetchWallets();
    },
    methods: {
      openAddWallet() {
        this.showAddWallet = true;
      },
      // Xử lý thêm ví mới
      async handleAddWallet() {
        console.log('Auth token:', localStorage.getItem('auth'));

        try {
          if (this.newWallet.name && this.newWallet.balance) {
            this.newWallet.userId = localStorage.getItem('userId');
            console.log('newwallet: ',this.newWallet.userId)
            // Gọi hàm addWallet từ API
            const response = await addWallet({
              userId: this.newWallet.userId,
              WalletName: this.newWallet.name,
              balance: parseFloat(this.newWallet.balance)
            });

            // Nếu thêm ví thành công, thêm vào danh sách và đóng dialog
            this.wallets.push(response); // Thêm ví mới vào danh sách ví
            this.dialog = false; // Đóng dialog
            this.newWallet.name = '';
            this.newWallet.balance = '';
          } else {
            alert('Vui lòng nhập đầy đủ thông tin ví!');
          }
        } catch (error) {
          console.error('Error adding wallet:', error);
          alert('Có lỗi khi thêm ví. Vui lòng thử lại.');
        }
      },
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
      async fetchTransactions() {
        try {
          const res = await axios.get("/User");
          console.log("UserId:", res.data.userId);
          this.IdNguoiDung = res.data.userId; // Gán vào biến trong component

          if (this.IdNguoiDung) {
            console.log("Fetching transactions for userId:", this.IdNguoiDung);
            const response = await getTransactions(this.IdNguoiDung); // Gọi API từ transactionApi.js
            console.log("Kết quả API:", response);
            this.transactions = response; // Gán kết quả trả về vào danh sách giao dịch
          } else {
            console.error("User ID không tồn tại");
          }
        } catch (error) {
          console.error("Lỗi khi lấy UserId hoặc dữ liệu giao dịch:", error.response?.data || error.message);
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
}
</script>

<style scoped>
  .text-center {
    text-align: center;
  }
</style>
