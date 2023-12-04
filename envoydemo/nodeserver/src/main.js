import { fileURLToPath } from 'url';
import { dirname, resolve } from 'path';
import grpc from '@grpc/grpc-js';
import protoLoader from '@grpc/proto-loader';

const HERE_DIR = dirname(fileURLToPath(import.meta.url));
const PROTO_PATH = resolve(HERE_DIR, '..', '..', 'echo.proto');

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

const server = new grpc.Server();
server.addService(
    proto['EchoService']['service'],
    {
        echo: (call, callback) => {
            console.log('on echo', call.request.message);
            callback(null, {
                message: `echo: ${call.request.message}`,
            });
        },
    },
);

console.log('start echo server');

server.bindAsync(
    '0.0.0.0:9900',
    grpc.ServerCredentials.createInsecure(),
    () => {
        server.start();
    });