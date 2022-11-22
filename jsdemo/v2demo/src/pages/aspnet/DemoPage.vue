<template>
    <div class="aspnet-demo-page">
        <div>AspMvcDemo：使用跨域，通过。</div>
        <input v-model="text" />
        <button @click="onClickSend">发送</button>
    </div>
</template>

<script>
import { AspMvcBookDemoClient } from '@/grpc/BookDemo_grpc_web_pb';
import { AspMvcBookListBooksRequest } from '@/grpc/BookDemo_pb';

const client = new AspMvcBookDemoClient('https://localhost:7160', null, null);

export default {
    data: () => {
        return {
            text: '',
        };
    },
    methods: {
        onClickSend() {
            const req = new AspMvcBookListBooksRequest();
            req.setContent(this.text);
            const call = client.listBooks(req, {
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
