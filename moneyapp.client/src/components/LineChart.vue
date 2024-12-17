<template>
    <div>
      <!-- Sử dụng Line component từ vue-chartjs -->
      <Line :data="chartData" :options="chartOptions" />
    </div>
  </template>
  
  <script>
  import { Line } from 'vue-chartjs';
  import { Chart as ChartJS, Title, Tooltip, Legend, LineElement, CategoryScale, LinearScale, PointElement } from 'chart.js';
  
  export default {
    name: 'LineChart',
    components: {
      Line
    },
    props: {
      chartData: {
        type: Object,
        required: true
      },
      chartOptions: {
        type: Object,
        required: true
      }
    },
    mounted() {
      // Đảm bảo đăng ký tất cả các thành phần của Chart.js
      ChartJS.register(Title, Tooltip, Legend, LineElement, CategoryScale, LinearScale, PointElement);
    }
  };
  </script>
  
  <style scoped>
  /* Thêm các style nếu cần */
  </style>
  