<template>
    <div class="improbable-grpc-page">
        <input v-model="data.content" />
        <button @click="onClickSendButton">发送</button>
    </div>
</template>

<script lang="ts" setup>
import { reactive } from 'vue';
import { grpc } from "@improbable-eng/grpc-web";
import { JsDemo } from '../../igrpc/jsdemo_pb_service';
import { SayJsDemoRequest, SayJsDemoReply } from '../../igrpc/jsdemo_pb';

// 生成物同样因为 vite 不支持 require 无法直接使用。
const data = reactive({
    content: '',
});

const onClickSendButton = () => {
    const req = new SayJsDemoRequest();
    req.setContent(data.content);
    grpc.invoke(JsDemo.Say, {
        request: req,
        host: "//",
        onMessage: (message: SayJsDemoReply) => {
            console.log("message: ", message.toObject());
        },
        onEnd: (code: grpc.Code, msg: string | undefined, trailers: grpc.Metadata) => {
            if (code == grpc.Code.OK) {
                console.log("all ok")
            } else {
                console.log("hit an error", code, msg, trailers);
            }
        }
    });
};
</script>