<template>
    <div class="navigation-header">
        <v-card height="50px" width="100%">

            <div class="left-side">
                <h2>Energy Management System - {{panelName}}</h2>
            </div>

            <div class="right-side">
                <v-btn depressed color="primary" @click="handleLogout">
                    Log out
                </v-btn>
            </div>

        </v-card>

    </div>
</template>
  
<script>
//import router from '@/router';
import { logoutUser } from '@/services/authenticationApi.js';

export default {
    name: 'NavigationHeader',
    props:{
        panelName: String
    },
    methods: {
        handleLogout: function () {
            logoutUser();
        }
    }
}
</script>
  
<style scoped lang="less">
.navigation-header {
    display: flex;
    flex-direction: row;
    justify-content: space-between;
    align-items: center;
    position: fixed;
    width: 100%;

    .left-side {
        width: 50%;
        display: inline-flex;

        padding-left: 20px;
        padding-top: 6px;
    }

    .right-side {
        width: 50%;

        display: inline-flex;
        justify-content: flex-end;

        padding-right: 20px;
    }
}
</style>