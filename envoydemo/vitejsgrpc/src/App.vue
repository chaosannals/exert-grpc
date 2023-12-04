<template>
  <div class="app">
    <h1>APP</h1>
    <p>1</p>
  </div>
</template>

<script setup>
import * as pb from './grpc/echo_pb';
// import * as grpc from './grpc/echo_grpc_web_pb';
import { onMounted } from 'vue';

// console.log('pb', pb, 'grpc', grpc);

onMounted(() => {
  // console.log('pb', pb, 'grpc', grpc);
});

</script>

<style scoped>
.logo {
  height: 6em;
  padding: 1.5em;
  will-change: filter;
  transition: filter 300ms;
}
.logo:hover {
  filter: drop-shadow(0 0 2em #646cffaa);
}
.logo.vue:hover {
  filter: drop-shadow(0 0 2em #42b883aa);
}
</style>
