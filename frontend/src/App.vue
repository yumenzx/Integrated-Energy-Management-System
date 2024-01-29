<template>
  <v-app>
    <notifications></notifications>
    <v-main>
      <router-view />
    </v-main>
  </v-app>
</template>

<script>

export default {
  name: 'App',
  data: function() {
    return {}
  },
  mounted() {
    document.title = "Energy Management System";
    
  }

};
</script>
