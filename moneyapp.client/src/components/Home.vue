<template>
  <v-app>
      <v-layout>
        <v-app-bar
          color="#00710F"
          prominent
        >
          <v-app-bar-nav-icon variant="text" @click.stop="drawer = !drawer"></v-app-bar-nav-icon>
          <v-toolbar-title>Tổng cộng : 100000000</v-toolbar-title>
        </v-app-bar>
  
        <v-navigation-drawer
          v-model="drawer"
          :location="$vuetify.display.mobile ? 'bottom' : undefined"
          temporary
        >
          <v-list
            :items="items"
          ></v-list>
        </v-navigation-drawer>
  
        <v-main style="height: 500px;">
          <v-card-text>
            The navigation drawer will appear from the bottom on smaller size screens.
          </v-card-text>
        </v-main>
      </v-layout>
  </v-app>
</template>

<script>
export default {
  name: 'HomeComponent',
  data:() => ({
    drawer: false
  }),
  methods: {
    
  },
};
</script>
