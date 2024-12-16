<template>
    <div>
      <sparkline 
        :data="chartData" 
        type="line" 
        :width="200" 
        :height="50" 
        line-color="#42A5F5" 
        fill-color="#E3F2FD" />
    </div>
  </template>
  
  <script>
  import { Sparkline } from 'vue-sparklines';
  
  export default {
    components: {
      Sparkline
    },
    props: {
      chartData: {
        type: Array,
        required: true
      }
    }
  };
  </script>
  