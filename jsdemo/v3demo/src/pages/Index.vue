<template>
    <div class="index-page">
        <input v-model="data.content"/>
        <button @click="onClickSendButton">发送</button>
    </div>
</template>

<script lang="ts" setup>
import { reactive } from 'vue';

const data = reactive({
    content: '',
});

// 请求发送出去了,但是 vite 不支持 HTTP2 grpc 代理，无法转发到 grpc node server 端。
// public/grpcdemo.html 启用 nginx 代理到 node server 端会出现 { code: 2, message: "Incomplete response" } 的问题。
// 代理只能使用官方示例的 envoy 代理服务器。
// GRPC WEB 官方示意： 浏览器（grpcweb） -> grpc代理（envoy）-> GRPC 服务（node c++ C# java）
// 可见 GRPC web 需要通过特殊代理服务器处理。
const onClickSendButton = async () => {
    //@ts-ignore
    const jswb = new window.Jswb('http://demo.grpcweb.com/');
    // const jswb = new window.Jswb();
    const r = await jswb.request(data.content);
    console.log(r);
};
</script>