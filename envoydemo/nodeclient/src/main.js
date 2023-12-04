import { fileURLToPath } from "url";
import { dirname, resolve } from "path";
import grpc from "@grpc/grpc-js";
import protoLoader from "@grpc/proto-loader";

// node 客户端示例，用于验证没有经过代理时服务器的有效性。
const HERE_DIR = dirname(fileURLToPath(import.meta.url));
const PROTO_PATH = resolve(HERE_DIR, "..", "..", "echo.proto");

const packageDefinition = protoLoader.loadSync(PROTO_PATH, {
  keepCase: true,
  longs: String,
  enums: String,
  defaults: true,
  oneofs: true,
});

const proto = grpc.loadPackageDefinition(packageDefinition);

const client = new proto.EchoService(
  "127.0.0.1:9900",
  grpc.credentials.createInsecure(),
);

function echo() {
  client.Echo(
    {
      message: "12312312423asdf'",
    },
    (err, response) => {
      console.log(err, response);
    }
  );
}

// setTimeout(echo, 1000);
echo();
