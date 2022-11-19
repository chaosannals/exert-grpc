import { fileURLToPath } from 'url';
import { dirname, resolve } from 'path';
import grpc from '@grpc/grpc-js';
import protoLoader from '@grpc/proto-loader';

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

const server = new grpc.Server();
server.addService(
    proto['JsDemo']['service'],
    {
        say: (call, callback) => {
            callback(null, {
                code: 0,
                tip: `say from server: ${call.request.content}`,
            });
        },
    },
);
server.bindAsync(
    '127.0.0.1:50051',
    grpc.ServerCredentials.createInsecure(),
    () => {
        server.start();
    });
