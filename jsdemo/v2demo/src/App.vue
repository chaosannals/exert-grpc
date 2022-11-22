<template>
  <div id="app">
    <router-view />
  </div>
</template>

<script>
import { awaitTask } from './myasync';

export default {
  name: 'App',
  // created 和 beforeMount 这些的异步方法并不会被 vue2 等待。
  async created() {
    console.log(new Date(), '*****');
    await awaitTask;
    console.log(new Date(), '123');
  },
  async beforeMount() {
    console.log(new Date(), 'mmmmm');
    await awaitTask;
    console.log(new Date(), '321');
  }
}
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
