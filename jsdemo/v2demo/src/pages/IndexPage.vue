<template>
    <div class="index-page">
        <div>dynserver：跨域没处理无法直连</div>
        <input v-model="text" />
        <button @click="onClickSend">发送</button>
    </div>
</template>

<script>
import { JsDemoClient } from '@/grpc/jsdemo_grpc_web_pb';
import { SayJsDemoRequest } from '@/grpc/jsdemo_pb';

const client = new JsDemoClient('http://127.0.0.1:50051', null, null);

export default {
    data: () => {
        return {
            text: '',
        };
    },
    methods: {
        onClickSend() {
            const req = new SayJsDemoRequest();
            req.setContent(this.text);
            const call = client.say(req, {
                'my-http-header-1': '12312',
            }, (err, response) => {
                console.log('response', err, response);
            });
            call.on('status', (status) => {
                console.log('status', status);
            });
        }
    }
};
</script>
