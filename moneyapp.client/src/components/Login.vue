<template>
    <div class="full-screen-container">
      <!-- Header với màu chủ đạo -->
      <div class="header"></div>
      
      <!-- Content màu trắng -->
      <div class="content"></div>
  
      <!-- Form đăng nhập đặt giữa màn hình -->
      <v-sheet class="login-form" elevation="4">
        <div class="text-center mb-4">
          <img src="/logo.png" alt="Logo" style="width: 50px; height: 50px;" />
          <h2 class="font-weight-bold mt-2" style="color: #00710F;">Money Lover</h2>
        </div>
        <div class="text-center mb-4">
          <h3 class="font-weight-bold">Log In</h3>
        </div>
        <v-form>
          <v-text-field
            v-model="userName"
            label="Email"
            outlined
            dense
            class="mb-4"
          ></v-text-field>
          <v-text-field
            v-model="userPass"
            label="Password"
            outlined
            dense
            type="password"
            class="mb-4"
          ></v-text-field>
          <v-btn block color="primary" elevation="0" class="mb-3" @click="userLogin" style="background-color: #00710F; color: white;">
            LOGIN
          </v-btn>
        </v-form>
        <div class="d-flex justify-space-between">
          <v-btn text small @click="forgotPassword" style="color: #00710F;">Forgot Password</v-btn>
          <v-btn text small to="/register" style="color: #00710F;">Register</v-btn>
        </div>
        <div v-if="loginError" class="text-center mt-4 mb-2 error--text">{{ loginError }}</div>
      </v-sheet>
    </div>
  </template>
  
  <script>
  import axios from "../utils/axios";
  
  export default {
    name: "LoginComponent",
    data: () => ({
      userName: "",
      userPass: "",
      loginError: "",
    }),
  
    methods: {
      userLogin() {
        axios
          .post("/user/login", {
            UserName: this.userName,
            PassWord: this.userPass,
          })
          .then((response) => {
            console.log(response.data);
            localStorage.setItem("auth", response.data.token);
            location = "/home";
          })
          .catch((error) => {
            this.loginError = error.response.data.message;
          });
      },
      forgotPassword() {
        console.log("Forgot password clicked");
      },
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
  </style>
  