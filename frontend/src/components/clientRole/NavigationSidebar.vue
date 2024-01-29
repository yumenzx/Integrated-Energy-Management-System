<template>
    <div class="navigation-sidebar">
        <v-card
    class="mx-auto"
    height="100vh"
    width="250"
  >
    <v-navigation-drawer
      absolute
      width="100%"
      permanent
    >
      <v-list>
        <v-list-item link @click="handleContentChange('devicesTable')">
          <v-list-item-icon>
            <v-icon>mdi-devices</v-icon>
          </v-list-item-icon>
          <v-list-item-content>
            <v-list-item-title>Devices</v-list-item-title>
          </v-list-item-content>
        </v-list-item>

        <v-list-item link @click="handleContentChange('chart')">
          <v-list-item-icon>
            <v-icon>mdi-chart-line</v-icon>
          </v-list-item-icon>
          <v-list-item-content>
            <v-list-item-title>Charts</v-list-item-title>
          </v-list-item-content>
        </v-list-item>

        <v-list-item link @click="handleContentChange('chat')">
          <v-list-item-icon>
            <v-icon>mdi-chat-outline</v-icon>
          </v-list-item-icon>
          <v-list-item-content>
            <v-list-item-title>Chat</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </v-list>
    </v-navigation-drawer>
  </v-card>
    </div>
</template>
  
<script>



export default {
    name: 'NavigationSiderbar',
    props:{
        handleChange: Function
    },
    methods:{
        handleContentChange: function (newContent) {
            this.handleChange(newContent);
        }
    }
}


</script>

<style scoped lang="less">

.navigation-sidebar{
    position: fixed;
}

</style>
  