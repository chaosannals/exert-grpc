<template>
  <h1>grpc ts demo</h1>
</template>

<script setup lang="ts">
import { GrpcWebFetchTransport } from "@protobuf-ts/grpcweb-transport";

import { type EchoRequest } from "./grpc/echo";
import { EchoServiceClient } from "./grpc/echo.client";

const request: EchoRequest = {
  message: "1111",
};
request;

let transport = new GrpcWebFetchTransport({
  baseUrl: `http://localhost:8080`,
});
const client = new EchoServiceClient(transport);
const r = client.echo(request);

console.log('response', r);
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
