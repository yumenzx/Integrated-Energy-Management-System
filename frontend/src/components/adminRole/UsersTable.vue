<template>
    <div class="users-table">
        <v-card>
            <div class="header">
                <h2>Users</h2>
                <v-divider></v-divider>

                <div class="table-search-add">
                    <v-text-field v-model="search" dense label="search users" solo>
                    </v-text-field>

                    <v-row justify="end">
                        <v-dialog v-model="dialogUserAdd" persistent max-width="600px">
                            <template v-slot:activator="{ on, attrs }">
                                <div class="header-button">
                                    <v-btn color="primary" depresed dark v-bind="attrs" v-on="on">
                                        Add user
                                    </v-btn>
                                </div>
                            </template>
                            <v-card>
                                <v-card-title>
                                    <span class="text-h5">Register new User</span>
                                </v-card-title>
                                <v-card-text>
                                    <v-container>
                                        <v-row>
                                            <v-col cols="12">
                                                <v-text-field label="Name" outlined required dense
                                                    v-model="userName"></v-text-field>
                                            </v-col>
                                            <v-col cols="12">
                                                <v-text-field label="Password" outlined required dense type="password"
                                                    v-model="userPassword"></v-text-field>
                                            </v-col>
                                            <v-col cols="12">
                                                <v-text-field label="Confirm Password" outlined required dense
                                                    type="password" v-model="userPasswordConfirm"></v-text-field>
                                            </v-col>
                                            <v-col cols="12" sm="6">
                                                <v-select :items="['client', 'administrator']" label="Role" outlined dense
                                                    required v-model="userRole"></v-select>
                                            </v-col>
                                        </v-row>
                                    </v-container>
                                </v-card-text>
                                <v-card-actions>
                                    <v-spacer></v-spacer>
                                    <v-btn color="blue darken-1" text @click="handleUserClose">
                                        Close
                                    </v-btn>
                                    <v-btn color="blue darken-1" text @click="handleUserRegister">
                                        Register User
                                    </v-btn>
                                </v-card-actions>
                            </v-card>
                        </v-dialog>
                    </v-row>
                </div>
            </div>

            <v-data-table :headers="headers" :items="users" hide-default-footer :search="search" height="64vh"
                class="elevation-1">
                <template v-slot:[`item.options`]="{ item }">
                    <v-btn icon small depressed @click="deleteUser(item)"><v-icon color="red">
                            mdi-account-remove-outline
                        </v-icon></v-btn>
                    &nbsp;
                    <v-btn icon small depressed @click="updateUser(item)"><v-icon color="primary">
                            mdi-account-edit-outline
                        </v-icon></v-btn>

                </template>
            </v-data-table>
        </v-card>

        <v-row justify="center">
            <v-dialog v-model="dialogUserUpdate" persistent max-width="600px">
                <v-card>
                    <v-card-title>
                        <span class="text-h5">Update user data</span>
                    </v-card-title>
                    <v-card-text>
                        <v-container>
                            <v-row>
                                <v-col cols="12">
                                    <v-text-field label="Name" outlined required dense v-model="userName"></v-text-field>
                                </v-col>
                                <v-col cols="12">
                                    <v-text-field label="New Password" outlined required dense type="password"
                                        v-model="userPassword"></v-text-field>
                                </v-col>
                                <v-col cols="12">
                                    <v-text-field label="Confirm New Password" outlined required dense type="password"
                                        v-model="userPasswordConfirm"></v-text-field>
                                </v-col>
                                <v-col cols="12" sm="6">
                                    <v-select :items="['client', 'administrator']" label="New Role" outlined dense required
                                        v-model="userRole"></v-select>
                                </v-col>
                            </v-row>
                        </v-container>
                    </v-card-text>
                    <v-card-actions>
                        <v-spacer></v-spacer>
                        <v-btn color="blue darken-1" text @click="handleUserClose">
                            Close
                        </v-btn>
                        <v-btn color="blue darken-1" text @click="handleUserUpdate">
                            Update User
                        </v-btn>
                    </v-card-actions>
                </v-card>
            </v-dialog>
        </v-row>
    </div>
</template>
  
<script>
//import { reactive } from 'vue';
import { logoutUser, isTokenExpired } from '@/services/authenticationApi';
import { getUsers, updateUser, deleteUser, registerUser } from '@/services/adminRole/adminApi';

export default {
    name: 'UsersTable',
    data: function () {
        return {
            search: '',
            userRole: 'Client',
            userId: '',
            userName: '',
            userPassword: '',
            userPasswordConfirm: '',
            dialogUserAdd: false,
            dialogUserUpdate: false,
            headers: [
                { text: 'ID', value: 'id', class: "admin-users-table-id", cellClass: "admin-users-table-id" },
                { text: 'Name', value: 'username', class: "admin-users-table-name", cellClass: "admin-users-table-name" },
                { text: 'Role', value: 'role', class: "admin-users-table-role", cellClass: "admin-users-table-role" },
                { text: 'Options', value: 'options', sortable: false, class: "admin-users-table-options", cellClass: "admin-users-table-options" }
            ],
            users: []

        }
    },
    methods: {
        verifyValues: function () {
            if (this.userName === '' || this.userPassword === '' || this.userPasswordConfirm === '') {
                alert("Fill all fields");
                return false;
            }

            if (this.userPassword !== this.userPasswordConfirm) {
                alert("Passwords does not match");
                return false;
            }
            return true;
        },
        verifyAuthorization: function () {
            if (isTokenExpired()) {
                alert("tokenul a exiprat");
                return false
            }

            const userRole = localStorage.getItem('userRole');
            if (userRole != "administrator") {
                logoutUser();
                alert("Rolul utilizatorului nu este adecvat");
                return false
            }
            return true;
        },
        handleUserRegister: async function () {
            if (!this.verifyValues())
                return;
            if (!this.verifyAuthorization())
                return;

            const userCredentials = {
                username: this.userName,
                password: this.userPassword,
                role: this.userRole
            };

            await registerUser(userCredentials);
            this.users = await getUsers();
            
            this.dialogUserAdd = false;
        },
        handleUserUpdate: async function () {
            if (!this.verifyValues())
                return;
            if (!this.verifyAuthorization())
                return;

            const administratorId = localStorage.getItem("userId");
            const newCredentials = {
                adminId: administratorId,
                userId: this.userId,
                newUsername: this.userName,
                newPassword: this.userPassword,
                newRole: this.userRole
            };

            await updateUser(newCredentials);
            this.users = await getUsers();
            this.dialogUserUpdate = false;
        },
        handleUserClose: function () {
            this.userName = this.userPassword = this.userPasswordConfirm = '';
            this.userRole = 'Client';
            this.dialogUserAdd = this.dialogUserUpdate = false;
        },
        updateUser: function (user) {
            this.userId = user.id;
            this.userName = user.username;
            this.userPassword = this.userPasswordConfirm = user.password;
            this.userRole = user.role;
            this.dialogUserUpdate = true;
        },
        deleteUser: async function (user) {
            if (!this.verifyAuthorization())
                return;
            await deleteUser(user.id);
            this.users = await getUsers();
        }
    },
    mounted: async function () {
        if (!this.verifyAuthorization())
            return;
        
        const u = await getUsers();
        this.users = u;
    }
}


</script>

<style scoped lang="less">
@import "@/styles/adminRole/usersTableStyles.css";

.users-table {
    background-color: red;

    .header {
        height: 100px;
        padding: 5px 5px 5px 10px;


        .table-search-add {

            margin-top: 10px;


            display: flex;
            flex-direction: row;
            justify-content: space-between;
            align-items: baseline;

            .header-button {
                margin-right: 20px;
            }

            .v-text-field {
                max-width: 250px !important;
            }
        }


    }
}
</style>
  