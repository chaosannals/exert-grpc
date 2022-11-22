import Vue from 'vue'
import App from './App.vue'
import { router } from './router.js';
import loader from './loader.js';


Vue.config.productionTip = false
Vue.use(loader);

new Vue({
  render: h => h(App),
  router: router,
}).$mount('#app');
