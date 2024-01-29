<template>
    <div class="register-panel">
        <div class="left">
            <v-img :src="require('../assets/loginImage.png')" contain height="500px" />
        </div>
        <div class="right">
            <h2>Energy Management System</h2>
            <v-divider></v-divider>
            &nbsp;
            <h2 class="center-horizontal">Register</h2>
            &nbsp;
            <v-text-field v-model="username" label="username" solo>
            </v-text-field>

            <v-text-field v-model="password" label="password" solo type="password">
            </v-text-field>

            <v-text-field v-model="passwordConfirm" label="password confirm" solo type="password">
            </v-text-field>

            <div class="center-horizontal">
                <v-btn depressed color="primary" @click="handleRegister">
                    Register
                </v-btn>
            </div>

        </div>
    </div>
</template>
  
<script>
import router from '@/router';
import { registerUser } from '@/services/authenticationApi';


export default {
    name: 'RegisterPanel',
    data: function () {
        return {
            username: '',
            password: '',
            passwordConfirm: ''
        }
    },
    methods: {
        verifyValues: function () {
            if (this.username === '' || this.password === '' || this.passwordConfirm === '') {
                alert("Fill all fields");
                return false;
            }
            if (this.password !== this.passwordConfirm) {
                alert("Passwords does not match");
                return false;
            }

            return true;
        },
        handleRegister: async function () {
            if (!this.verifyValues())
                return;

            const registerCredentials = {
                username: this.username,
                password: this.password,
                role: "client"
            };
            const response = await registerUser(registerCredentials);

            if (!response)
                return;

            alert("Registrarea a avut success");
            router.push('/login');
        }
    }
}


</script>

<style scoped lang="less">
.register-panel {
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
    }
}
</style>
  