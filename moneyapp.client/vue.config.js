const { defineConfig } = require('@vue/cli-service')
const webpack = require('webpack')

module.exports = defineConfig({
  transpileDependencies: true,

  pluginOptions: {
    vuetify: {
      // https://github.com/vuetifyjs/vuetify-loader/tree/next/packages/vuetify-loader
    }
  },

  configureWebpack: {
    plugins: [
      new webpack.DefinePlugin({
        __VUE_OPTIONS_API__: JSON.stringify(true),             // bật Options API nếu bạn dùng
        __VUE_PROD_DEVTOOLS__: JSON.stringify(false),          // tắt devtools production
        __VUE_PROD_HYDRATION_MISMATCH_DETAILS__: JSON.stringify(false) // tắt cảnh báo hydration
      })
    ]
  }
})
