<template>
    <v-app>
      <v-layout>
        <!-- App Bar -->
        <v-app-bar color="#00710F" prominent>
          <v-app-bar-nav-icon variant="text" @click.stop="drawer = !drawer" />
          <v-toolbar-title class="text-h5 font-weight-light">
            Tổng cộng: 
            <span v-if="!showTotal">{{ formatCurrency(totalAmount) }}</span>
            <v-icon 
              @click="showTotal = !showTotal" 
              class="ml-2 cursor-pointer"
              :style="{ transform: showTotal }"
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
          <v-list density="compact">
            <v-list-item
              v-for="(item, index) in drawerItems"
              :key="index"
              :prepend-icon="item.icon"
              :title="item.title"
              :active="isActiveRoute(item.route)"
              active-color="#00710F"
              class="hoverable"
              @click="onDrawerItemClick(item)"
            />
          </v-list>
        </v-navigation-drawer>



        <!-- Main Content -->
        <v-main>
          <v-container fluid>
            <v-row class="py-6">
              <v-col cols="4">
                <v-card class="pa-4" outlined>
                  <div class="d-flex justify-space-between align-start"> <!-- align-start để căn trái -->
                    <!-- Cột bên trái -->
                    <div class="d-flex flex-column align-start"> <!-- align-start để căn trái -->
                      <v-icon color="#00710F" size="40" class="mb-4">mdi-calendar-today</v-icon>
                      <div class="text-h6 font-weight-medium">Today's Transactions</div>
                    </div>
                    
                    <!-- Cột bên phải -->
                    <div class="text-h2 font-weight-light align-self-center pa-4 mr-4" style="color: #FF5722;">{{ todayTransactionsCount }}</div>
                  </div>
                </v-card>
              </v-col>
              <v-col cols="4">
                <v-card class="pa-4" outlined>
                  <div class="d-flex justify-space-between align-start"> <!-- align-start để căn trái -->
                    <!-- Cột bên trái -->
                    <div class="d-flex flex-column align-start"> <!-- align-start để căn trái -->
                      <v-icon color="#D32F2F" size="40" class="mb-2">mdi-calendar-week</v-icon> 
                      <div class="text-h6 font-weight-medium">Weekly Transactions</div>
                    </div>
                    
                    <!-- Cột bên phải -->
                    <div class="text-h2 font-weight-light align-self-center pa-4 mr-4" style="color: #FF5722;">{{ weeklyTransactionsCount }}</div>
                  </div>
                </v-card>
              </v-col>
              <v-col cols="4">
                <v-card class="pa-4" outlined>
                  <div class="d-flex justify-space-between align-start"> <!-- align-start để căn trái -->
                    <!-- Cột bên trái -->
                    <div class="d-flex flex-column align-start"> <!-- align-start để căn trái -->
                      <v-icon color="#1976D2" size="40" class="mb-2">mdi-calendar-month</v-icon> 
                      <div class="text-h6 font-weight-medium">Monthly Transactions</div>
                    </div>
                    
                    <!-- Cột bên phải -->
                    <div class="text-h2 font-weight-light align-self-center pa-4 mr-4" style="color: #FF5722;">{{ monthlyTransactionsCount }}</div>
                  </div>
                </v-card>
              </v-col>
            </v-row>

            <!-- Detailed Report -->
            <v-card class="mt-4" outlined>
              <v-card-title>
                <div class="text-h6 font-weight-bold">Detailed Report</div>
                <v-spacer></v-spacer>
                <v-btn @click="showAddDialog = true" color="secondary" dark class="ml-2">
                  <v-icon left>mdi-plus</v-icon> Add
                </v-btn>
                <!-- <v-btn color="primary" dark class="ml-2">
                  <v-icon left>mdi-export</v-icon> Export
                </v-btn> -->
              </v-card-title>
              <!-- Table Section -->
                <div class="table-container">
                  <table class="custom-table">
                    <thead>
                      <tr>
                        <th class="header-cell">ID</th>
                        <th class="header-cell">Mô tả</th>
                        <th class="header-cell">Tiền</th>
                        <th class="header-cell">Ngày tạo</th>
                        <th class="header-cell">Actions</th>
                      </tr>
                    </thead>
                    <tbody>
                      <tr v-for="(item, index) in transactions" :key="index">
                        <td>{{ item.transaction.transactionID }}</td>
                        <td>{{ item.transaction.description }}</td>
                        <td>{{ item.transaction.amount.toLocaleString() }} VND</td>
                        <td>{{ new Date(item.transaction.transactionDate).toLocaleString() }}</td>
                        <td>
                          <v-icon small color="red" @click="deleteTransactionHandler(item.transaction.transactionID)">mdi-delete</v-icon>
                        </td>
                      </tr>
                    </tbody>
                  </table>
                </div>
            </v-card>
          </v-container>
          <!-- Dialog thêm giao dịch -->
          <v-dialog v-model="showAddDialog" max-width="600">
            <v-card>
              <v-card-title class="text-h6 font-weight-bold">
                Thêm giao dịch mới
              </v-card-title>

              <v-card-text>
                <v-container>
                  <!-- Chọn ví -->
                  <v-row>
                    <v-col cols="12">
                      <v-select
                        v-model="newTransaction.walletID"
                        :item-props="itemProps"
                        :items="wallets"
                        label="Chọn ví"
                        item-value="id"
                        outlined
                        dense
                        required
                      >
                      </v-select>
                    </v-col>
                  </v-row>

                  <!-- Chọn danh mục -->
                  <v-row>
                    <v-col cols="12">
                      <v-select
                        v-model="newTransaction.categoryID"
                        :item-props="itemPropsCategory"
                        :items="categories"
                        label="Chọn danh mục"
                        item-value="id"
                        item-text="name"
                        outlined
                        dense
                        required
                      >
                          <!-- <template v-slot:selection="{ item }">
                            <v-chip :color="item.type === 'Thu' ? 'green' : 'red'" dark>
                              <v-icon left>{{ item.type === 'Thu' ? 'mdi-arrow-up' : 'mdi-arrow-down' }}</v-icon>
                              {{ item.name }}
                            </v-chip> 
                          </template> -->
                      </v-select>
                    </v-col>
                  </v-row>

                  <!-- Nhập số tiền -->
                  <v-row>
                    <v-col cols="12">
                      <v-text-field
                        v-model="newTransaction.amount"
                        label="Số tiền"
                        type="number"
                        outlined
                        dense
                        required
                      ></v-text-field>
                    </v-col>
                  </v-row>

                  <!-- Nhập mô tả -->
                  <v-row>
                    <v-col cols="12">
                      <v-textarea
                        v-model="newTransaction.description"
                        label="Mô tả"
                        outlined
                        dense
                      ></v-textarea>
                    </v-col>
                  </v-row>
                </v-container>
              </v-card-text>

              <v-card-actions>
                <v-spacer></v-spacer>
                <!-- Nút hủy -->
                <v-btn text color="grey" @click="showAddDialog = false">
                  Hủy
                </v-btn>
                <!-- Nút lưu -->
                <v-btn color="primary" dark @click="saveTransaction">
                  Lưu
                </v-btn>
              </v-card-actions>
            </v-card>
            </v-dialog>
        </v-main>
      </v-layout>
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
    </v-app>
</template>
  
<script>
  // import axios from '@/utils/axios';
  import { getTransactions, addTransaction, deleteTransaction } from '@/utils/transactionApi';
  import { getWallet } from '@/utils/walletApi';
  import { getCategories } from '@/utils/categoryApi';
  export default {
    name: 'TransactionCompnent',
    data: () => ({
      showAddDialog: false, // Hiển thị dialog
      wallets: [], // Lưu danh sách ví
      categories: [],
      transactions: [],
      transactions11: [],
      drawer: true,
      showTotal: true,
      drawerItems: [
        { title: "Dashboard", icon: "mdi-view-dashboard", route: '/home' },
        { title: "Sổ giao dịch", icon: "mdi-book", route: '/transaction' },
        { title: "Tài khoản", icon: "mdi-account", route: '/info' },
      ],

      newTransaction: {
        walletID: null,
        categoryID: null,
        amount: null,
        description: "",
      },
      dialogType: "",
      dialog: false,
      dialogMessage: '',
      todayTransactionsCount: 0,
      weeklyTransactionsCount: 0,
      monthlyTransactionsCount: 0,
    }),
    methods: {
      calculateTransactionCounts() {
          const today = new Date();
          const startOfWeek = new Date(today);
          startOfWeek.setDate(today.getDate() - today.getDay()); // Lấy ngày đầu tuần (Chủ nhật)
          const startOfMonth = new Date(today.getFullYear(), today.getMonth(), 1);

          // Lấy ngày hôm nay dưới dạng YYYY-MM-DD (bỏ qua giờ)
          const todayString = today.toISOString().split('T')[0];

          // Kiểm tra giao dịch hôm nay
          this.todayTransactionsCount = this.transactions11.filter(transactionDate => {
            const transactionDateObj = new Date(transactionDate); // Chuyển đổi chuỗi thành đối tượng Date
            const transactionDateString = transactionDateObj.toISOString().split('T')[0]; // Chỉ lấy ngày
            return transactionDateString === todayString;
          }).length;

          // Kiểm tra giao dịch trong tuần này
          this.weeklyTransactionsCount = this.transactions11.filter(transactionDate => {
            const transactionDateObj = new Date(transactionDate); // Chuyển đổi chuỗi thành đối tượng Date
            return transactionDateObj >= startOfWeek && transactionDateObj <= today;
          }).length;

          // Kiểm tra giao dịch trong tháng này
          this.monthlyTransactionsCount = this.transactions11.filter(transactionDate => {
            const transactionDateObj = new Date(transactionDate); // Chuyển đổi chuỗi thành đối tượng Date
            return transactionDateObj >= startOfMonth && transactionDateObj <= today;
          }).length;

          console.log('Today:', this.todayTransactionsCount);
          console.log('This Week:', this.weeklyTransactionsCount);
          console.log('This Month:', this.monthlyTransactionsCount);
      },


      showMessage(message, type) {
        this.dialogMessage = message;
        this.dialogType = type; // 'success' hoặc 'error'
        this.dialog = true; // Mở dialog
      },
      async saveTransaction() {
        try {
          // Kiểm tra dữ liệu đầu vào
          if (
            !this.newTransaction.walletID ||
            !this.newTransaction.categoryID ||
            !this.newTransaction.amount
          ) {
            alert("Vui lòng nhập đầy đủ thông tin.");
            return;
          }

          // Gọi API thêm giao dịch
          await addTransaction(this.newTransaction);

          // Tải lại danh sách giao dịch
          await this.loadTransaction();

          // Hiển thị thông báo
          this.showMessage('Đã tạo thành công!', 'success');

          // Reset dữ liệu form
          this.newTransaction = {
            walletID: null,
            categoryID: null,
            amount: null,
            description: "",
          };
          this.showAddDialog = false;
        } catch (error) {
          console.error("Lỗi khi tạo giao dịch:", error);
          this.showMessage('Có lỗi xảy ra!', 'error');
        }
      },
      // xoa giao dichdich
      async deleteTransactionHandler(transactionId) {
        try {
          // Gọi API xóa giao dịch với transactionId
          await deleteTransaction(transactionId);

          // Tải lại danh sách giao dịch
          await this.loadTransaction();

          // Hiển thị thông báo
          this.showMessage('Đã xóa thành công!', 'success');
        } catch (error) {
          console.error("Lỗi khi xóa giao dịch:", error);
          this.showMessage('Có lỗi xảy ra!', 'error');
        }
      },
      async loadTransaction() {
        try {
          const transactions = await getTransactions();  // Gọi API lấy giao dịch
          console.log(transactions)
          console.log("header",this.tableHeaders)
          this.transactions = transactions;
          this.transactions11 = transactions.map(transaction => transaction.transaction.transactionDate);

          this.calculateTransactionCounts();

          console.log('transaction11: ',this.transactions11);
        } catch (error) {
          console.log("error loading transactions", error);
        }
      },
      async fetchWallets() {
        try {
          const data = await getWallet();
          // this.wallets = data;
          // Tính tổng số dư của tất cả ví
          console.log('data',data)
          this.wallets = data.map(wallet => ({
            id: wallet.walletID,
            name: wallet.walletName,
            balance: wallet.balance,
          }));
          console.log(this.wallets)
          this.totalAmount = data.reduce((total, wallet) => total + wallet.balance, 0);
        } catch (error) {
          this.message = 'Không thể lấy thông tin ví!';
        }
      },
      async fetchCategories() {
        try {
          const data = await getCategories();
          console.log(data)
          this.categories = data.map(category => ({
            id: category.categoryId,
            name: category.categoryName,
            type: category.type,
          }));
        } catch (error) {
          this.message = 'Không thể lấy thông tin ví!';
        }
      },
      formatCurrency(amount) {
        return new Intl.NumberFormat("vi-VN", { style: "currency", currency: "VND" }).format(amount);
      },
      isActiveRoute(route) {
        return this.$route.path === route;
      },
      onDrawerItemClick(item) {
        // Sử dụng Vue Router để điều hướng
        this.$router.push(item.route);
      },
      itemProps (item) {
        return {
          title: `${item.name} - Số dư: ${item.balance.toLocaleString()} VND`,
        }
      },
      itemPropsCategory(item) {
        return {
          title: `${item.name} - ${item.type === 'Thu' ? 'Loại: Thu' : 'Loại: Chi'}`,
          color: item.type === 'Thu' ? 'green' : 'red', // Màu sắc cho loại thu và chi
          icon: item.type === 'Thu' ? 'mdi-arrow-up' : 'mdi-arrow-down', // Biểu tượng mũi tên
        };
      },
    },
    mounted() {
      this.loadTransaction();
      this.fetchWallets();
      this.fetchCategories();
      this.calculateTransactionCounts();
    },
  };
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
  .table-container {
    overflow-x: auto; /* Cho phép cuộn ngang nếu bảng quá rộng */
    margin-top: 16px;
  }

  .custom-table {
    width: 100%;
    border-collapse: collapse;
    background-color: #ffffff;
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
    border-radius: 8px;
  }

  .custom-table th,
  .custom-table td {
    padding: 12px;
    text-align: left;
    border-bottom: 1px solid #ddd;
  }

  .custom-table th {
    background-color: #f4f6f8;
    color: #444;
    font-weight: bold;
  }

  .custom-table tr:hover {
    background-color: #f1f1f1;
  }

  .header-cell {
    min-width: 120px;
  }

  /* Thêm hiệu ứng khi di chuột vào các ô hành động */
  .custom-table td:last-child {
    text-align: center;
  }

  .custom-table td:last-child .v-icon {
    cursor: pointer;
    transition: color 0.3s ease;
  }

  .custom-table td:last-child .v-icon:hover {
    color: #ff5252;
  }
</style>