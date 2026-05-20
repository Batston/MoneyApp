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
          <h3 class="font-weight-bold">Log In</h3>
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
            v-model="userPass"
            label="Password"
            outlined
            dense
            variant="underlined"
            type="password"
            class="mb-4"
          ></v-text-field>
          <v-btn height="48" :loading="isloading" block color="#00710F" elevation="2" class="mb-3" @click="userLogin">
            LOGIN
          </v-btn>
        </v-form>
        <!-- <div class="d-flex justify-space-between">
          <v-btn text small @click="forgotPassword" style="color: #00710F;">Forgot Password</v-btn>
          <v-btn text small to="/register" style="color: #00710F;">Register</v-btn>
        </div> -->
        <div class="d-flex justify-center">
          <p class="small fw-bold mt-2 pt-1 mb-0">Don't have an account? <a style="color: #00710F;" href="#!"
            class="link-danger" @click="$router.push('/register')">Register</a></p>
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
      showRegisterDialog: false,
      
      fullname: "",
      usernameRegister: "",
      email: "",
      passwordRegister: "",

      userName: "",
      userPass: "",
      loginError: "",
      isloading: false,
    }),
  
    methods: {
      userLogin() {
        this.isloading = true
        setTimeout(() => (this.isloading = false), 3000)

        axios
          .post("/User/login", {
            username: this.userName,
            password: this.userPass,
          })
          .then((response) => {
            console.log(response.data);
            localStorage.setItem("auth", response.data.token);
            this.$router.replace("/home");
          })
          .catch((error) => {
            console.log("LOGIN ERROR: ", error.response);

    this.loginError =
        error.response?.data?.message ||
        error.response?.data?.errors ||
        "Đăng nhập thất bại!";
          });
      },
      // getUserId() {
      //   axios.get("/user")
      //     .then((res) => {
      //       console.log(res.data.userId);
      //       localStorage.setItem("userId", res.data.userId);
      //     })
      //     .catch((error) => {
      //       console.error("Lỗi lấy User ID:", error.response?.data || error.message);
      //     });
      // },
      forgotPassword() {
        console.log("Forgot password clicked");
      },
      registerUser() {
        this.isloading = true
        setTimeout(() => (this.isloading = false), 3000)

        axios
          .post("/User/register", {
              "userId": 0,
              "username": this.usernameRegister, 
              "passwordHash": this.passwordRegister,
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