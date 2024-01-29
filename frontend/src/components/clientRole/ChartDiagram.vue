<template>
    <div class="chart-diagram">
        <apex-chart type="line" :options="options" :series="series" width="800px"></apex-chart>
        <div class="date-pick">
            <v-menu ref="menu" v-model="menu" :close-on-content-click="false" :return-value.sync="date"
                transition="scale-transition" offset-y min-width="auto">
                <template v-slot:activator="{ on, attrs }">
                    <v-text-field v-model="date" label="Picker in menu" prepend-icon="mdi-calendar" readonly v-bind="attrs"
                        v-on="on"></v-text-field>
                </template>
                <v-date-picker v-model="date" no-title scrollable>
                    <v-spacer></v-spacer>
                    <v-btn text color="primary" @click="menu = false">
                        Cancel
                    </v-btn>
                    <v-btn text color="primary" @click="$refs.menu.save(date)">
                        OK
                    </v-btn>
                </v-date-picker>
            </v-menu>
            <v-btn depressed color="primary" @click="handleGenerateChart">
                Generate Chart
            </v-btn>
        </div>
    </div>
</template>
  
<script>

import { getChartData } from '@/services/clientRole/clientApi';

export default {
    name: 'ChartDiagram',
    data: function () {
        return {
            date: (new Date(Date.now() - (new Date()).getTimezoneOffset() * 60000)).toISOString().substr(0, 10),
            menu: false,
            options: {

                xaxis: {
                    categories: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23]
                }
            },
            series: [{
                name: 'kw/h',
                data: []
            }]
        }
    },
    methods: {
        handleGenerateChart: async function () {
            const measurements = await getChartData(this.date);

            this.series = [{
                data: measurements
            }]
        }
    }
}
</script>

<style scoped lang="less">
.chart-diagram {
    display: flex;
    flex-direction: column;
    justify-content: center;
    //align-items: center;
    height: 100%;
    width: 100%;

    .date-pick {
        display: flex;
        flex-direction: row;
        align-items: center;
    }
}
</style>
  