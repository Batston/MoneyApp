<template>
  <v-app>
    <v-layout>
      <!-- App Bar -->
      <v-app-bar color="#00710F" prominent app fixed>
        <v-app-bar-nav-icon variant="text" @click.stop="drawer = !drawer" />
        <v-toolbar-title class="text-h5 font-weight-bold">
          Tổng cộng: 
          <span v-if="!showTotal">{{ formatCurrency(totalAmount) }}</span>
          <v-icon 
            @click="showTotal = !showTotal" 
            class="ml-2 cursor-pointer"
            :style="{ transform: showTotal ? 'scale(1.2)' : 'scale(1)' }"
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
                <v-list-item-title class="font-weight-bold ma-3">{{ item.title }}</v-list-item-title>
              </v-list-item-content>
            </div>
          </v-list-item>
        </v-list>
      </v-navigation-drawer>




      <!-- Main Content -->
      <v-main>
        <v-container fluid style="height: 100%">
          <v-row>
            <!-- Layout 1: Báo cáo thống kê -->
            <v-col cols="12" md="6">
              <v-card style="max-height: 300px" class="rounded-lg elevation-2" hover>
                <v-card-title class="text-h6 font-weight-bold text-primary" style="border-bottom: 1px solid #00710F;">
                  Báo cáo thống kê
                </v-card-title>
                <v-card-text>
                  <v-skeleton-loader v-if="!wallets.length || !transactions.length" type="card" class="rounded-lg" />
                  <div v-else>
                    <line-chart :chartData="chartData" :chartOptions="chartOptions" />
                  </div>
                </v-card-text>
              </v-card>
            </v-col>

            <!-- Layout 2: Giao dịch gần đây -->
            <v-col cols="12" md="6">
              <v-card class="rounded-lg elevation-2" hover>
                <v-card-title class="text-h6 font-weight-bold text-primary" style="border-bottom: 1px solid #00710F;">
                  Giao dịch gần đây
                </v-card-title>
                <v-card-text>
                  <v-list style="max-height: 200px; overflow-y: auto;"> <!-- Cho phép cuộn nếu danh sách dài -->
                    <v-list-item
                      v-for="transaction in transactions"
                      :key="transaction.transaction.transactionID"
                      class="rounded-lg hoverable"
                    >
                      <div class="d-flex justify-space-between align-center">
                        <div class="d-flex align-center">
                          <v-icon color="#00710F" size="40">mdi-cash</v-icon>
                          <div class="ml-3">
                            <span class="text-h6 font-weight-bold">{{ transaction.transaction.description }}</span>
                            <div class="text-caption text-grey">{{ formatDate(transaction.transaction.transactionDate) }}</div>
                          </div>
                        </div>
                        <span class="text-h6 font-weight-bold">{{ formatCurrency(transaction.transaction.amount) }}</span>
                      </div>
                    </v-list-item>
                  </v-list>
                </v-card-text>
              </v-card>

            </v-col>

          </v-row>

          <!-- Layout 3: Danh sách ví -->
          <v-row>
            <v-col cols="12">
              <v-card class="rounded-lg elevation-2" hover>
                <!-- Tiêu đề và Button -->
                <v-card-title
                  class="text-h6 font-weight-bold text-primary d-flex align-center"
                  style="border-bottom: 2px solid #00710F;"
                >
                  Danh sách ví
                  <v-spacer></v-spacer>
                  <!-- Button thêm ví -->
                  <v-btn
                    rounded="lg"
                    color="#00710F"
                    class="mr-2"
                    style="padding: 8px 16px;"
                    @click="openAddWallet"
                  >
                    <v-icon left>mdi-plus</v-icon> Thêm ví
                  </v-btn>
                  <!-- Button xóa ví -->
                  <v-btn
                    :disabled="disablebtn"
                    rounded="lg"
                    color="#00710F"
                    class="mr-2"
                    style="padding: 8px 16px;"
                    @click="handleDeleteWallet(IdWallet)"
                  >
                    <v-icon left>mdi-delete</v-icon> Xóa ví
                  </v-btn>
                  <!-- Button cập nhật ví -->
                  <v-btn
                    :disabled="disablebtn"
                    rounded="lg"
                    color="#00710F"
                    style="padding: 8px 16px;"
                    @click="updateWallet"
                  >
                    <v-icon left>mdi-wrench</v-icon> Cập nhật
                  </v-btn>
                </v-card-title>

                <!-- Danh sách ví -->
                <v-card-text>
                  <v-row>
                    <!-- Lặp qua từng ví -->
                    <v-col v-for="wallet in wallets" :key="wallet.walletID" cols="12" sm="6" md="4">
                      <v-card
                        hover
                        class="rounded-lg text-center pa-3"
                        @click="selectWallet(wallet)"
                      >
                        <!-- Icon ví -->
                        <v-icon size="50" color="#00710F">mdi-wallet</v-icon>
                        <!-- Thông tin ví -->
                        <div class="mt-2">
                          <h3 class="text-h6 font-weight-bold">{{ wallet.walletName }}</h3>
                          <p class="text-subtitle1">Số dư: {{ formatCurrency(wallet.balance) }}</p>
                        </div>
                      </v-card>
                    </v-col>
                  </v-row>
                </v-card-text>
              </v-card>

            </v-col>
          </v-row>
          <!-- Dialog Thêm ví -->
          <v-dialog v-model="showAddWallet" max-width="500px" transition="dialog-bottom-transition">
            <v-card>
              <v-card-title class="headline">Thêm ví mới</v-card-title>
              <v-card-text>
                <v-text-field v-model="newWallet.name" label="Tên ví" required></v-text-field>
                <v-text-field v-model="newWallet.balance" label="Số dư" type="number" required></v-text-field>
              </v-card-text>
              <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn color="green darken-1" @click="handleAddWallet">Thêm</v-btn>
                <v-btn color="red darken-1" @click="showAddWallet = false">Hủy</v-btn>
              </v-card-actions>
            </v-card>
          </v-dialog>

          <!-- dialog message -->
          <v-dialog v-model="dialog" max-width="500px">
            <v-card>
              <v-card-title class="headline">Thông báo</v-card-title>
              <v-card-text>
                <div class="text-center">
                  <v-icon color="success" size="128">mdi-check-circle-outline</v-icon>
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
  import LineChart from './LineChart.vue';
  import axios from "../utils/axios";
  import { getWallet, addWallet, deleteWallet, } from "@/utils/walletApi";
  import { getTransactions } from "@/utils/transactionApi";

  export default {
    components: {
      LineChart
    },
    name: "HomeComponent",
    data: () => ({
      showAddWallet: false,
      drawer: true,
      showTotal: true,
      IdNguoiDung: '',
      IdWallet: '',
      totalAmount: "", // Giả lập số dư tổng cộng
      transactions: [],
      wallets: [],
      newWallet: {
        userId: '',
        name: '',
        balance: ''
      },
      drawerItems: [
        { title: "Dashboard", icon: "mdi-view-dashboard", route: '/home' },
        { title: "Sổ giao dịch", icon: "mdi-book", route: '/transaction' },
        { title: "Ngân sách", icon: "mdi-bank", route: '/budget' },
        { title: "Tài khoản", icon: "mdi-account", route: '/account' },
      ],
      dialog: false,
      dialogMessage: '',
      disablebtn: true,
      chartData: {
        labels: ['Tháng 1', 'Tháng 2', 'Tháng 3'],  // Ví dụ tháng
        datasets: [
          {
            label: 'Tổng số tiền',
            data: [1000, 2000, 1500],  // Dữ liệu tổng số tiền ví mỗi tháng
            fill: false,
            borderColor: '#00710F',
            tension: 0.1
          }
        ]
      },
      chartOptions: {
        responsive: true,
        maintainAspectRatio: false
      }
    }),
    mounted() {
      this.fetchTransactions();
      this.fetchWallets();
      this.$nextTick(() => {
        document.addEventListener('click', this.handleDocumentClick);
      });
    },
    methods: {
      handleDocumentClick(event) {
        // Kiểm tra xem walletList có tồn tại không trước khi truy cập $el
        const walletListElement = this.$refs.walletList ? this.$refs.walletList.$el : null; 
        if (walletListElement) {
          const isClickInside = walletListElement.contains(event.target); // Kiểm tra nếu click bên trong ví
          if (!isClickInside) {
            // Nếu không click vào ví, disable nút
            this.disablebtn = true;
            console.log("Click ra ngoài, nút đã bị disable");
          }
        }
      },
      beforeDestroy() {
        // Xóa sự kiện khi component bị hủy
        document.removeEventListener('click', this.handleDocumentClick);
      },
      openAddWallet() {
        this.showAddWallet = true;
      },
      // chọn ví
      selectWallet(wallet) {
        this.disablebtn = false,
        console.log('Ví đã chọn:', wallet);
        this.IdWallet = wallet.walletID;
      },
      // Xử lý thêm ví mới
      async handleAddWallet() {
        console.log('Auth token:', localStorage.getItem('auth'));
        
        try {
          if (this.newWallet.name && this.newWallet.balance) {
            if (!Number.isFinite(parseFloat(this.newWallet.balance)) || parseFloat(this.newWallet.balance) < 0) {
                alert('Số dư phải là một số dương.');
                return;
            }
            this.newWallet.userId = this.IdNguoiDung;
            console.log('newwalletid: ',this.newWallet.userId)
            console.log('ten vi: ',this.newWallet.name)
            console.log('so du: ',this.newWallet.balance)
            // Gọi hàm addWallet từ API
            await addWallet({
              userId: this.newWallet.userId,
              WalletName: this.newWallet.name,
              balance: parseFloat(this.newWallet.balance)
            });

            // Nếu thêm ví thành công, thêm vào danh sách và đóng dialog
            // this.wallets.push(response); // Thêm ví mới vào danh sách ví
            // Gọi lại API lấy danh sách ví sau khi thêm thành công
            await this.fetchWallets();
            this.dialogMessage = 'Thêm ví thành công!';
            this.dialog = true;

            this.showAddWallet = false; // Đóng dialog
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
      // xoa vi
      async handleDeleteWallet(walletId) {
        console.log('Auth token:', localStorage.getItem('auth'));
        
        try {
            if (walletId) {
                // Gọi hàm deleteWallet từ API với walletId
                await deleteWallet(walletId);

                // Nếu xóa ví thành công, gọi lại API để lấy danh sách ví cập nhật
                await this.fetchWallets();
                this.dialogMessage = 'Xóa ví thành công!';
                this.dialog = true;
            } else {
                alert('Không tìm thấy ví để xóa!');
            }
        } catch (error) {
            console.error('Error deleting wallet:', error);
            alert('Có lỗi khi xóa ví. Vui lòng thử lại.');
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
      // Kiểm tra xem đường dẫn hiện tại có trùng với route của item không
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
