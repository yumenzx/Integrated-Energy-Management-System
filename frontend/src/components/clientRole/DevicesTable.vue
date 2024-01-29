<template>
  <div class="devices-table">
    <v-card>
      <div class="header">
        <h2>Devices</h2>
        <v-text-field v-model="search" dense label="search devices" solo>
        </v-text-field>
      </div>
      <v-divider></v-divider>
      <v-data-table :headers="headers" :items="devices" hide-default-footer height="64vh" :search="search"
        class="elevation-1"></v-data-table>
    </v-card>
  </div>
</template>
  
<script>
import { isTokenExpired } from '@/services/authenticationApi';
import { getDevices } from '@/services/clientRole/clientApi';

export default {
  name: 'DevicesTable',
  data: function () {
    return {
      search: '',
      headers: [
                { text: 'ID', value: 'id', class:"client-devices-table-id", cellClass:"client-devices-table-id" },
                { text: 'Description', value: 'description', class:"client-devices-table-description", cellClass:"client-devices-table-description" },
                { text: 'Address', value: 'address', class:"client-devices-table-address", cellClass:"client-devices-table-address" },
                { text: 'Maximum Hourly Consumption', value: 'maxHourlyConsumption', class:"client-devices-table-max-consumption", cellClass:"client-devices-table-max-consumption" }
            ],
      devices : []
    }
  },
  methods: {
    verifyAuthorization: function () {
            if (isTokenExpired()) {
                alert("tokenul a exiprat");
                return false
            }

            /*const userRole = localStorage.getItem('userRole');
            if (userRole != "administrator") {
                logoutUser();
                alert("Rolul utilizatorului nu este adecvat");
                return false
            }*/
            return true;

    }
  },
  mounted: async function () {
        if (!this.verifyAuthorization())
            return;

        this.devices = await getDevices();
    }
}


</script>

<style scoped lang="less">
@import "@/styles/clientRole/devicesTableStyles.css";

.devices-table {
  background-color: red;

  .header {
    display: flex;
    justify-content: space-between;

    height: 50px;
    padding: 5px 15px 5px 15px;

    .v-text-field {
      max-width: 150px !important;
    }
  }

}
</style>
  