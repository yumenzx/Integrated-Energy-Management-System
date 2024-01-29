import Vue from 'vue'
import App from './App.vue'
import router from './router'
import vuetify from './plugins/vuetify'
import axios from 'axios'
import VueAxios from 'vue-axios'
import VueApexCharts from 'vue-apexcharts'

import NotifyPlugin from 'vue-easy-notify'
import 'vue-easy-notify/dist/vue-easy-notify.css'

Vue.config.productionTip = false

Vue.use(VueAxios, axios)

Vue.use(VueApexCharts)
Vue.component('apex-chart', VueApexCharts)

Vue.use(NotifyPlugin)

new Vue({
  router,
  vuetify,
  render: h => h(App)
}).$mount('#app')

/*
this.$notifyInfo('This is info messsage.');
this.$notifySuccess('This is success messsage.');
this.$notifyWarning('This is warning messsage!');
this.$notifyError('This is error messsage!');
*/