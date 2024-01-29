<template>
  <div class="navigation-sidebar">
    <v-card class="mx-auto" height="100vh" width="250">
      <v-navigation-drawer absolute width="100%" permanent>
        <v-list>
          <v-list-item link @click="handleContentChange('usersTable')">
            <v-list-item-icon>
              <v-icon>mdi-account-group</v-icon>
            </v-list-item-icon>
            <v-list-item-content>
              <v-list-item-title>Users</v-list-item-title>
            </v-list-item-content>
          </v-list-item>

          <v-list-item link @click="handleContentChange('devicesTable')">
            <v-list-item-icon>
              <v-icon>mdi-devices</v-icon>
            </v-list-item-icon>
            <v-list-item-content>
              <v-list-item-title>Devices</v-list-item-title>
            </v-list-item-content>
          </v-list-item>

          <v-divider></v-divider>
          <div class="messages-section">
            User Messages
          </div>

          <v-list-item v-for="(item, index) in messagesList" :key="index" link @click="handleContentChange('chat',item.userId)">
            <v-list-item-icon>
              <v-icon>mdi-chat-plus-outline</v-icon>
            </v-list-item-icon>
            <v-list-item-content>
              <v-list-item-title>{{ item.name }}</v-list-item-title>
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
  props: {
    handleChange: Function,
    messagesList: Array
  },
  methods: {
    handleContentChange: function (newContent, userId) {
      this.handleChange(newContent, userId);
    }
  }
}


</script>

<style scoped lang="less">
.navigation-sidebar {
  position: fixed;

  .messages-section {
    display: flex;
    justify-content: center;
    margin: 10px 0 10px 0;
  }
}
</style>
  