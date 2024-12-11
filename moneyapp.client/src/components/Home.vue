<template>
  <div class="text-center my-3">
    <div class="mb-2">
      <b>Danh sách học sinh và điểm môn học</b>
    </div>
  </div>

  <div v-if="errorMessage !== ''" class="mt-2 mb-2 text-center" :style="{color: colorMessage}">
    <b>{{ errorMessage }}</b>
  </div>

  <!-- Bảng hiển thị danh sách học sinh và điểm môn học -->
  <v-table class="mx-auto" style="width: 90%">
    <thead>
      <tr>
        <th>Mã HS</th>
        <th class="text-left">Họ</th>
        <th class="text-left">Tên</th>
        <th>Ngày sinh</th>
        <th>Mã lớp</th>
        <th>Tên lớp</th>
        <th>Mã môn</th>
        <th>Tên môn</th>
        <th>Phân ban</th>
        <th>Điểm</th>
        <th>Sửa</th>
      </tr>
    </thead>
    <tbody>
      <tr v-for="item in dataHocSinh" :key="item.MaHS + '-' + item.MaMon">
        <td>{{ item.MaHS }}</td>
        <td>{{ item.HoHS }}</td>
        <td>{{ item.TenHS }}</td>
        <td>{{ item.NgaySinh }}</td>
        <td>{{ item.MaLop }}</td>
        <td>{{ item.TenLop }}</td>
        <td>{{ item.MaMon }}</td>
        <td>{{ item.TenMon }}</td>
        <td>{{ item.PhanBan }}</td>
        <td>{{ item.Diem }}</td>
        <td>
          <v-icon @click="openDialogUpdateHS(item)">mdi-pencil</v-icon>
        </td>
      </tr>
    </tbody>
  </v-table>

 <!-- Hộp thoại sửa điểm học sinh -->
<v-dialog width="500" scrollable v-model="showDialogUpdateHS">
  <template v-slot:default="{ isActive }">
    <v-card prepend-icon="mdi-school" :title="dialogUpdateTitleHS">
      <v-divider class="mb-3"></v-divider>

      <v-card-text class="px-4">
        <!-- Chỉnh sửa điểm của môn học -->
        <v-text-field v-model="objHocSinh.Diem" label="Điểm" type="number" variant="outlined"></v-text-field>
      </v-card-text>

      <v-divider></v-divider>

      <v-card-actions>
        <v-btn text="Đóng" @click="isActive.value = false"></v-btn>
        <v-spacer></v-spacer>
        <v-btn color="surface-variant" text="Lưu" variant="flat" @click="saveUpdateActionHS()"></v-btn>
      </v-card-actions>
    </v-card>
  </template>
</v-dialog>

</template>

<script>
import axios from '../utils/axios';

export default {
  name: 'HocSinhComponent',
  data: () => ({
    dataHocSinh: [],  // Dữ liệu học sinh và điểm
    showDialogUpdateHS: false,
    dialogUpdateTitleHS: "",
    objHocSinh: {
      MaHS: "",
      HoHS: "",
      TenHS: "",
      NgaySinh: "",
      MaLop: "",
      TenLop: "",
      MaMon: "",
      TenMon: "",
      PhanBan: "",
      Diem: ""
    },
    errorMessage: "",
    colorMessage: "blue"
  }),
  mounted() {
    this.getHocSinh();
  },
  methods: {
    getHocSinh() {
      axios.get('/hocsinh')
        .then((res) => {
          console.log(res);  // Kiểm tra cấu trúc của res
          this.dataHocSinh = res.data.data;
        })
        .catch((error) => {
          console.error("Lỗi khi lấy dữ liệu học sinh:", error);
        });
    },
    openDialogUpdateHS(obj) {
    this.showDialogUpdateHS = true;
    this.dialogUpdateTitleHS = "Chỉnh sửa điểm học sinh";

    // Chỉ lấy thông tin học sinh và môn học cần sửa điểm
    this.objHocSinh = {
      MaHS: obj.MaHS,
      MaMonHoc: obj.MaMon,  // Tên môn học (nếu cần hiển thị cho người dùng)
      Diem: obj.Diem  // Điểm môn học
    };
  },

  saveUpdateActionHS() {
  if (this.objHocSinh.Diem === "") {
    this.errorMessage = "Điểm không được để trống!";
    this.colorMessage = "red";
    return;
  }

  console.log("Dữ liệu gửi đi:", this.objHocSinh);

  axios.post('diem/update', this.objHocSinh)
    .then((response) => {
      console.log("Phản hồi từ API:", response.data); // Log phản hồi API
      this.showDialogUpdateHS = false;
      this.getHocSinh();  // Cập nhật lại danh sách học sinh sau khi sửa điểm
      this.errorMessage = response.data.message;
      this.colorMessage = "blue";
    })
    .catch((error) => {
      console.error("Lỗi khi gọi API:", error.response); // Log lỗi từ API
      this.errorMessage = error.response?.data?.message || "Đã xảy ra lỗi!";
      this.colorMessage = 'red';
      this.showDialogUpdateHS = false;
    });
}

  }
}
</script>
