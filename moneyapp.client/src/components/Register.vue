<template>
    <div class="full-screen-container">
      <!-- Header với màu chủ đạo -->
      <div class="header"></div>
      
      <!-- Content màu trắng -->
      <div class="content"></div>
  
      <!-- Form đăng nhập đặt giữa màn hình -->
      <v-sheet class="login-form" elevation="4">
        <div class="text-center mb-4">
          <!-- <img src="../moneyLogo.png" alt="Logo" style="width: 500px; height: 500px;" /> -->
          <v-img
            :aspect-ratio="1"
            class="bg-white mx-auto"
            src="../assets/moneyLogo.png"
            width="100"
            cover
          ></v-img>
          <h2 class="font-weight-bold mt-2" style="color: #00710F;">Money Management</h2>
        </div>
        <div class="text-center mb-4">
          <h3 class="font-weight-bold">Register</h3>
        </div>
        <v-form>
          <v-text-field
            v-model="userName"
            label="Username"
            outlined
            dense
            variant="underlined"
            class="mb-4"
          ></v-text-field>
          <v-text-field
            v-model="email"
            label="Email"
            outlined
            dense
            type="email"
            variant="underlined"
            class="mb-4"
          ></v-text-field>
          <v-text-field
            v-model="userPass"
            label="Password"
            outlined
            dense
            variant="underlined"
            type="password"
            class="mb-4"
          ></v-text-field>
          <v-text-field
            v-model="confirmPass"
            label="Confirm your password"
            outlined
            dense
            variant="underlined"
            type="password"
            class="mb-4"
          ></v-text-field>
          <v-btn height="48" :loading="isloading" block color="#00710F" elevation="2" class="mb-3" @click="registerUser">
            Register
          </v-btn>
        </v-form>
        <!-- <div class="d-flex justify-space-between">
          <v-btn text small @click="forgotPassword" style="color: #00710F;">Forgot Password</v-btn>
          <v-btn text small to="/register" style="color: #00710F;">Register</v-btn>
        </div> -->
        <div class="d-flex justify-center">
          <p class="small fw-bold mt-2 pt-1 mb-0">Have an account? <a style="color: #00710F;" href="#!"
            class="link-danger" @click="$router.push('/login')">Login</a></p>
        </div>
        <div v-if="loginError" class="text-center mt-4 mb-2 error--text">{{ loginError }}</div>
      </v-sheet>
    </div>
  </template>
  
  <script>
  import axios from "../utils/axios";
  
  export default {
    name: "RegisterComponent",
    data: () => ({
      fullname: "tri",
      email: "",
      userName: "",
      userPass: "",
      confirmPass: "",
      loginError: "",
      isloading: false,
    }),
  
    methods: {
      registerUser() {
        // Kiểm tra các trường đã nhập
        if (!this.userName || !this.userPass || !this.confirmPass || !this.email) {
            this.loginError = "Vui lòng nhập đầy đủ thông tin!";
            return;
        }

        // Kiểm tra định dạng email
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
            if (!emailRegex.test(this.email)) {
            this.loginError = "Email không hợp lệ!";
            return;
        }

        // Kiểm tra xác nhận mật khẩu
        if (this.userPass !== this.confirmPass) {
            this.loginError = "Mật khẩu xác nhận không khớp!";
        return;
        }
        this.loginError = "";
        this.isloading = true
        setTimeout(() => (this.isloading = false), 3000)

        console.log(this.fullname);
        console.log(this.email);
        console.log(this.userName);
        console.log(this.userPass);
        axios
          .post("/User/register", {
              "userId": 0,
              "username": this.userName, 
              "passwordHash": this.userPass,
              "fullName": this.fullname,
              "email": this.email,
          })
          .then((response) => {
            console.log(response.data);
            alert("Đăng ký thành công!");
            this.showRegisterDialog = false;
          })
          .catch((error) => {
            this.loginError = error.response.data.message;
          });
      }
    },
  };
  </script>
  
  <style scoped>
  /* Container toàn màn hình */
  .full-screen-container {
    position: relative;
    width: 100%;
    height: 100vh;
    display: flex;
    flex-direction: column;
  }
  
  /* Header màu chủ đạo */
  .header {
    flex: 1;
    background-color: #00710F;
  }
  
  /* Content màu trắng */
  .content {
    flex: 1;
    background-color: white;
  }
  
  /* Form đăng nhập chính giữa màn hình */
  .login-form {
    position: absolute;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%);
    width: 400px;
    padding: 24px;
    border-radius: 12px;
    background-color: white;
    box-shadow: 0px 4px 6px rgba(0, 0, 0, 0.1);
  }
  .register {
    font-size: 14px;
    text-decoration: underline;
  }
  </style>
  