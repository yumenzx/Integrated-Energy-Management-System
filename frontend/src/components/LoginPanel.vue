<template>
    <div class="login-panel">
        <div class="left">
            <v-img :src="require('../assets/loginImage.png')" contain height="500px" />
        </div>
        <div class="right">
            <h2>Energy Management System</h2>
            <v-divider></v-divider>
            &nbsp;
            <h2 class="center-horizontal">Login</h2>
            &nbsp;
            <v-text-field v-model="username" label="username" solo>
            </v-text-field>

            <v-text-field v-model="password" label="password" solo type="password">
            </v-text-field>

            <div class="center-horizontal">
                <v-btn depressed color="primary" @click="handleLogin">
                    Authenticate
                </v-btn>
            </div>

            <div class="register-bottom">
                <v-btn depressed color="primary" @click="handleRegister">
                    Register
                </v-btn>
            </div>


        </div>
    </div>
</template>
  
<script>

import router from '@/router';
import { loginUser } from '@/services/authenticationApi.js';


export default {
    name: 'LoginPanel',
    data: function () {
        return {
            username: '',
            password: ''
        }
    },
    methods: {
        verifyValues: function () {
            if (this.username === '' || this.password === '') {
                this.$notifyError('Fill all fields');
                return false;
            }

            return true;
        },
        handleLogin: async function () {
            if (!this.verifyValues())
                return;

            const loginCredentials = {
                username: this.username,
                password: this.password
            };

            const info = await loginUser(loginCredentials);

            switch(info.userRole){
                case "client":
                    router.push('/clientHome');
                    break;
                case "administrator":
                    router.push('/adminHome');
                    break;
                default:
                    //router.push('/login');
                    break;
            }

            //const token = localStorage.getItem('token');
            //const userId = localStorage.getItem('userId');
            //const userRole = localStorage.getItem('userRole');

            //router.push('/clientHome')
        },
        handleRegister: function () {
            //alert(this.username + ' ' + this.password);
            router.push('/register')
        }
    }
}


</script>

<style scoped lang="less">
.login-panel {
    display: flex;
    flex-direction: row;
    top: 50%;
    margin: auto;
    width: 80%;

    .left {
        width: 75%;
        //background-color: red;
    }

    .right {
        width: 35%;
        padding: 20px 30px 0 30px;
        background-color: rgb(210, 210, 240);
        margin: 8px 0 8px 0;

        .center-horizontal {
            display: flex;
            flex-direction: column;
            align-items: center;
            //background-color: white;
        }

        .register-bottom {
            margin-top: 50px;
        }
    }
}
</style>
  