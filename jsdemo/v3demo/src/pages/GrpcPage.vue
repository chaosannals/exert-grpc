<template>
    <div class="grpc-page">
        <input v-model="data.content"/>
        <button @click="onClickSendButton">发送</button>
    </div>
</template>

<script lang="ts" setup>
import { reactive } from 'vue';
import { RpcError, Status } from 'grpc-web';
// module.exports vite 不支持。 换用 vue-cli 的项目应该可以。
// import {
//     SayJsDemoRequest,
//     SayJsDemoReply,
// } from '../../grpc/jsdemo_pb';
import proto from '../../grpc/jsdemo_pb';
import { JsDemoClient } from '../../grpc/JsdemoServiceClientPb';

const data = reactive({
    content: '',
});

const client = new JsDemoClient('http://127.0.0.1:50051', null, null);

const onClickSendButton = () => {
    const req = new proto.SayJsDemoRequest();
    req.setContent(data.content);
    const call = client.say(req, {
        'my-http-header-1': '12312',
    }, (err: RpcError, response: proto.SayJsDemoReply) => {
        console.log('response', err, response);
    });
    call.on('status', (status: Status) => {
        console.log('status', status);
    });
};
</script>