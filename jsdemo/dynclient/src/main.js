import { fileURLToPath } from 'url';
import { dirname, resolve } from 'path';
import grpc from '@grpc/grpc-js';
import protoLoader from '@grpc/proto-loader';

// 这是个 node 端示例有 node 库依赖，不可在 浏览器端 运行。
const HERE_DIR = dirname(fileURLToPath(import.meta.url));
const PROTO_PATH = resolve(HERE_DIR, '..', '..', 'jsdemo.proto');

const packageDefinition = protoLoader.loadSync(
    PROTO_PATH,
    {
        keepCase: true,
        longs: String,
        enums: String,
        defaults: true,
        oneofs: true,
    },
);

const proto = grpc.loadPackageDefinition(packageDefinition);

//console.log(proto);

const client = new proto.JsDemo(
    '127.0.0.1:50051',
    grpc.credentials.createInsecure()
);

client.Say({
    content: "12312312423asdf'",
}, (err, response) => {
    console.log(err, response);
});
