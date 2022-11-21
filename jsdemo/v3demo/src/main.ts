import { createApp } from 'vue';
import './style.css';
import App from './App.vue';
import { createMyDemoRouter } from './router';

const app = createApp(App);
const router = createMyDemoRouter();
app.use(router);
app.mount('#app');
