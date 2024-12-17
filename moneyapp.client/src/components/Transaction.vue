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
                  <v-icon color="#1976D2" size="40" class="mb-2">mdi-account-plus</v-icon>
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
                <v-btn color="secondary" dark class="ml-2">
                  <v-icon left>mdi-import</v-icon> Import
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
                <template v-slot:[`item.transactionDate`]="{ item }">
                  <span>{{ new Date(item.transaction.transactionDate).toLocaleString() }}</span>
                </template>

                <template v-slot:[`item.amount`]="{ item }">
                  <span>{{ item.transaction.amount.toLocaleString() }} VND</span>
                </template>

                <!-- Hiển thị các hành động -->
                <template v-slot:[`item.actions`]="{ item }">
                  <v-icon small color="blue" @click="editTransaction(item.transaction)">mdi-pencil</v-icon>
                  <v-icon small color="red" @click="deleteTransaction(item.transaction.transactionID)">mdi-delete</v-icon>
                </template>
              </v-data-table>
            </v-card>
          </v-container>
        </v-main>
      </v-layout>
    </v-app>
</template>
  
<script>
  // import axios from '@/utils/axios';
  import { getTransactions } from '@/utils/transactionApi';
  export default {
    name: 'TransactionCompnent',
    data: () => ({
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
        { text: 'Transaction ID', align: 'start', key: 'transactionID', value: 'transaction.transactionID' },
        { text: 'Wallet ID', align: 'start', key: 'walletID', value: 'transaction.walletID' },
        { text: 'Category ID', align: 'start', key: 'categoryID', value: 'transaction.categoryID' },
        { text: 'Transaction Date', align: 'start', key: 'transactionDate', value: 'transaction.transactionDate' },
        { text: 'Amount', align: 'start', key: 'amount', value: 'transaction.amount' },
        { text: 'Description', align: 'start', key: 'description', value: 'transaction.description' },
        { text: 'Actions', align: 'center', key: 'actions' },
      ],
    }),
    methods: {
      async loadTransaction() {
        try {
          const transactions = await getTransactions();  // Gọi API lấy giao dịch
          this.transactions = transactions;
        } catch (error) {
          console.log("error loading transactions", error);
        }
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
      this.loadTransaction();
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