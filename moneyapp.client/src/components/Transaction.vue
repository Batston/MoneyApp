<template>
    <v-app>
      <v-layout>
        <!-- App Bar -->
        <v-app-bar color="#00710F" prominent>
          <v-app-bar-nav-icon variant="text" @click.stop="drawer = !drawer" />
          <v-toolbar-title class="text-h5 font-weight-medium">
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
                  <v-list-item-title class="font-weight-medium ma-3">{{ item.title }}</v-list-item-title>
                </v-list-item-content>
              </div>
            </v-list-item>
          </v-list>
        </v-navigation-drawer>



        <!-- Main Content -->
        <v-main>
          <v-container fluid>
            <!-- Overview Section -->
            <v-row class="py-6">
              <v-col cols="4">
                <v-card class="pa-4" outlined>
                  <v-icon color="#00710F" size="40" class="mb-2">mdi-cart</v-icon>
                  <div class="text-h6 font-weight-bold">Turnover</div>
                  <div class="text-h5">$92,405</div>
                  <div class="text-caption text-success">+5.39% period of change</div>
                </v-card>
              </v-col>
              <v-col cols="4">
                <v-card class="pa-4" outlined>
                  <v-icon color="#D32F2F" size="40" class="mb-2">mdi-cash</v-icon>
                  <div class="text-h6 font-weight-bold">Profit</div>
                  <div class="text-h5">$32,218</div>
                  <div class="text-caption text-success">+5.39% period of change</div>
                </v-card>
              </v-col>
              <v-col cols="4">
                <v-card class="pa-4" outlined>
                  <v-icon color="#1976D2" size="40" class="mb-2">mdi-account-plus </v-icon>
                  <div class="text-h6 font-weight-bold">New Customer</div>
                  <div class="text-h5">298</div>
                  <div class="text-caption text-success">+6.84% period of change</div>
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
                <v-btn color="primary" dark class="ml-2">
                  <v-icon left>mdi-export</v-icon> Export
                </v-btn>
              </v-card-title>
              <v-data-table 
                :headers="tableHeaders"
                :items="transactions"
                class="elevation-1 mt-2"
                dense
              >
                <!-- Hiển thị thông tin về giao dịch -->
                <!-- Slot cho ID -->
                <template v-slot:[`item.transaction.transactionID`]="{ item }">
                  <span>{{ item.transaction.transactionID }}</span>
                </template>

                <!-- Slot cho Mô tả -->
                <template v-slot:[`item.transaction.description`]="{ item }">
                  <span>{{ item.transaction.description }}</span>
                </template>

                <!-- Slot cho Số tiền -->
                <template v-slot:[`item.transaction.amount`]="{ item }">
                  <span>{{ item.transaction.amount.toLocaleString() }} VND</span>
                </template>

                <!-- Slot cho Ngày -->
                <template v-slot:[`item.transaction.transactionDate`]="{ item }">
                  <span>{{ new Date(item.transaction.transactionDate).toLocaleString() }}</span>
                </template>
                <!-- Hiển thị các hành động -->
                <template v-slot:[`item.actions`]="{ item }">
                  <!-- <v-icon small color="blue" @click="editTransaction(item.transaction)">mdi-pencil</v-icon> -->
                  <v-icon small color="red" @click="deleteTransactionHandler(item.transaction.transactionID)">mdi-delete</v-icon>
                </template>
              </v-data-table>
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
      drawer: true,
      showTotal: true,
      drawerItems: [
        { title: "Dashboard", icon: "mdi-view-dashboard", route: '/home' },
        { title: "Sổ giao dịch", icon: "mdi-book", route: '/transaction' },
        { title: "Ngân sách", icon: "mdi-bank", route: '/budget' },
        { title: "Tài khoản", icon: "mdi-account", route: '/account' },
      ],
      tableHeaders: [
        { text: 'ID', align: 'start', value: 'transaction.transactionID' },
        { text: 'Mô tả', align: 'start', value: 'transaction.description' },
        { text: 'Tiền', align: 'start', value: 'transaction.amount' },
        { text: 'Ngày tạo', align: 'start', value: 'transaction.transactionDate' },
        { text: 'Actions', align: 'center', value: 'actions' },
      ],

      newTransaction: {
        walletID: null,
        categoryID: null,
        amount: null,
        description: "",
      },
      dialog: false,
      dialogMessage: '',
    }),
    methods: {
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
          this.dialogMessage = 'Tạo giao dịch thành công';
          this.dialog = true;

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
          this.dialogMessage = "Có lỗi xảy ra khi tạo giao dịch!";
          this.dialog = true;
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
          this.dialogMessage = "Xóa giao dịch thành công!";
          this.dialog = true;
        } catch (error) {
          console.error("Lỗi khi xóa giao dịch:", error);
          this.dialogMessage = "Có lỗi xảy ra khi xóa giao dịch!";
          this.dialog = true;
        }
      },
      async loadTransaction() {
        try {
          const transactions = await getTransactions();  // Gọi API lấy giao dịch
          console.log(transactions)
          console.log("header",this.tableHeaders)
          this.transactions = transactions;
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
    },
  };
</script>
<style scoped>
  text-center {
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