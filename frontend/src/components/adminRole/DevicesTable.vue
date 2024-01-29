<template>
    <div class="devices-table">
        <v-card>
            <div class="header">
                <h2>Devices</h2>
                <v-divider></v-divider>

                <div class="table-search-add">
                    <v-text-field v-model="search" dense label="search devices" solo>
                    </v-text-field>

                    <v-row justify="end">
                        <v-dialog v-model="dialogDeviceAdd" persistent max-width="600px">
                            <template v-slot:activator="{ on, attrs }">
                                <div class="header-button">
                                    <v-btn color="primary" depresed dark v-bind="attrs" v-on="on">
                                        Add device
                                    </v-btn>
                                </div>
                            </template>
                            <v-card>
                                <v-card-title>
                                    <span class="text-h5">Register new Device</span>
                                </v-card-title>
                                <v-card-text>
                                    <v-container>
                                        <v-row>
                                            <v-col cols="12">
                                                <v-text-field label="Description" outlined required dense
                                                    v-model="deviceDescription"></v-text-field>
                                            </v-col>
                                            <v-col cols="12">
                                                <v-text-field label="Address" outlined required dense
                                                    v-model="deviceAddress"></v-text-field>
                                            </v-col>
                                            <v-col cols="12">
                                                <v-text-field label="Maximum Hourly Consumption" outlined required dense
                                                    v-model="deviceConsumption"></v-text-field>
                                            </v-col>
                                        </v-row>
                                    </v-container>
                                </v-card-text>
                                <v-card-actions>
                                    <v-spacer></v-spacer>
                                    <v-btn color="blue darken-1" text @click="handleDeviceClose">
                                        Close
                                    </v-btn>
                                    <v-btn color="blue darken-1" text @click="handleDeviceRegister">
                                        Register Device
                                    </v-btn>
                                </v-card-actions>
                            </v-card>
                        </v-dialog>
                    </v-row>
                </div>
            </div>

            <v-data-table :headers="headers" :items="devices" hide-default-footer :search="search" height="64vh"
                class="elevation-1">
                <template v-slot:[`item.options`]="{ item }">
                    <v-btn icon small depressed @click="deleteDevice(item)"><v-icon color="red">
                            mdi-delete-outline
                        </v-icon>
                    </v-btn>
                    &nbsp;
                    <v-btn icon small depressed @click="updateDevice(item)"><v-icon color="primary">
                            mdi-cog-outline
                        </v-icon>
                    </v-btn>
                    &nbsp;
                    <v-btn v-if="item.ownerId" icon small depressed @click="unmapDevice(item)"><v-icon color="orange">
                            mdi-link-variant-remove
                        </v-icon>
                    </v-btn>
                    <v-btn v-else icon small depressed @click="mapDevice(item)"><v-icon color="green">
                            mdi-link-variant-plus
                        </v-icon>
                    </v-btn>
                </template>
            </v-data-table>
        </v-card>

        <v-row justify="center">
            <v-dialog v-model="dialogDeviceUpdate" persistent max-width="600px">
                <v-card>
                    <v-card-title>
                        <span class="text-h5">Update device data</span>
                    </v-card-title>
                    <v-card-text>
                        <v-container>
                            <v-row>
                                <v-col cols="12">
                                    <v-text-field label="Description" outlined required dense
                                        v-model="deviceDescription"></v-text-field>
                                </v-col>
                                <v-col cols="12">
                                    <v-text-field label="New Address" outlined required dense
                                        v-model="deviceAddress"></v-text-field>
                                </v-col>
                                <v-col cols="12">
                                    <v-text-field label="Maximum Hourly Consumption" outlined required dense
                                        v-model="deviceConsumption"></v-text-field>
                                </v-col>
                            </v-row>
                        </v-container>
                    </v-card-text>
                    <v-card-actions>
                        <v-spacer></v-spacer>
                        <v-btn color="blue darken-1" text @click="handleDeviceClose">
                            Close
                        </v-btn>
                        <v-btn color="blue darken-1" text @click="handleDeviceUpdate">
                            Update Device
                        </v-btn>
                    </v-card-actions>
                </v-card>
            </v-dialog>
        </v-row>

        <v-row justify="center">
            <v-dialog v-model="dialogDeviceMap" persistent max-width="600px">
                <v-card>
                    <v-card-title>
                        <span class="text-h5">Map device to user</span>
                    </v-card-title>
                    <v-card-text>
                        <v-container>
                            <v-row>
                                <v-col cols="12">
                                    <v-select :items="userIds" label="User ID" outlined dense required
                                        v-model="deviceMapUserId"></v-select>
                                </v-col>
                            </v-row>
                        </v-container>
                    </v-card-text>
                    <v-card-actions>
                        <v-spacer></v-spacer>
                        <v-btn color="blue darken-1" text @click="handleDeviceClose">
                            Close
                        </v-btn>
                        <v-btn color="blue darken-1" text @click="handleDeviceMap">
                            Map Device
                        </v-btn>
                    </v-card-actions>
                </v-card>
            </v-dialog>
        </v-row>
    </div>
</template>
  
<script>
import { getDevices, registerDevice, updateDevice, deleteDevice, mapDevice, unMapDevice, getUsers } from '@/services/adminRole/adminApi';
import { logoutUser, isTokenExpired } from '@/services/authenticationApi';

export default {
    name: 'DevicesTable',
    data: function () {
        return {
            search: '',
            deviceId: '',
            ownerId: '',
            deviceDescription: '',
            deviceAddress: '',
            deviceConsumption: '',
            deviceMapUserId: '',
            dialogDeviceAdd: false,
            dialogDeviceUpdate: false,
            dialogDeviceMap: false,
            headers: [
                { text: 'ID', value: 'id', class: "admin-devices-table-id", cellClass: "admin-devices-table-id" },
                { text: 'Description', value: 'description', class: "admin-devices-table-description", cellClass: "admin-devices-table-description" },
                { text: 'Address', value: 'address', class: "admin-devices-table-address", cellClass: "admin-devices-table-address" },
                { text: 'Maximum Hourly Consumption', value: 'maxHourlyConsumption', class: "admin-devices-table-max-consumption", cellClass: "admin-devices-table-max-consumption" },
                { text: 'Owner', value: 'ownerId', class: "admin-devices-table-owner", cellClass: "admin-devices-table-owner" },
                { text: 'Options', value: 'options', sortable: false, class: "admin-devices-table-options", cellClass: "admin-devices-table-options" }
            ],
            devices: [],
            userIds: []
        }
    },
    methods: {
        verifyValues: function () {
            if (this.deviceDescription === '' || this.deviceAddress === '' || this.deviceConsumption === '') {
                alert("Fill all fields");
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
        handleDeviceRegister: async function () {
            if (!this.verifyValues())
                return;
            if (!this.verifyAuthorization())
                return;

            const deviceData = {
                description: this.deviceDescription,
                address: this.deviceAddress,
                maxHourlyConsumption: this.deviceConsumption
            };

            await registerDevice(deviceData);
            this.devices = await getDevices();
            this.dialogDeviceAdd = false;
        },
        handleDeviceUpdate: async function () {
            if (!this.verifyValues())
                return;
            if (!this.verifyAuthorization())
                return;

            const newDatas = {
                deviceId: this.deviceId,
                newDescription: this.deviceDescription,
                newAddress: this.deviceAddress,
                newConsumption: this.deviceConsumption
            };

            await updateDevice(newDatas);
            this.devices = await getDevices();
            this.dialogDeviceUpdate = false;
        },
        handleDeviceMap: async function () {
            if (this.deviceMapUserId === '') {
                alert("Fill or select User ID");
                return;
            }
            if (!this.verifyAuthorization())
                return;

            const usrId = this.deviceMapUserId;
     
            if(!this.userIds.includes(usrId)){
                alert("User ID value not found");
                return;
            }

            const mapData = {
                deviceId: String(this.deviceId),
                ownerId: String(usrId)
            };

            await mapDevice(mapData);
            this.devices = await getDevices();
            this.dialogDeviceMap = false;
            this.deviceMapUserId = '';
        },
        handleDeviceClose: function () {
            this.deviceMapUserId = '';
            this.deviceDescription = this.deviceAddress = this.deviceConsumption = '';
            this.dialogDeviceAdd = this.dialogDeviceUpdate = this.dialogDeviceMap = false;
        },
        updateDevice: function (device) {
            this.deviceId = device.id;
            this.deviceDescription = device.description;
            this.deviceAddress = device.address;
            this.deviceConsumption = device.maxHourlyConsumption;
            this.dialogDeviceUpdate = true;
        },
        deleteDevice: async function (device) {
            if (!this.verifyAuthorization())
                return;
            await deleteDevice(device.id);
            this.devices = await getDevices();
        },
        mapDevice: async function (device) {
            const users = await getUsers();
            this.userIds = users.map(u => u.id);
            this.deviceId = device.id;
            this.dialogDeviceMap = true;
        },
        unmapDevice: async function (device) {
            if (!this.verifyAuthorization())
                return;

            await unMapDevice(String(device.id));
            this.devices = await getDevices();
        }
    },
    mounted: async function () {
        if (!this.verifyAuthorization())
            return;

        const d = await getDevices();
        this.devices = d;
    }
}


</script>

<style scoped lang="less">
@import "@/styles/adminRole/devicesTableStyles.css";

.devices-table {
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
  