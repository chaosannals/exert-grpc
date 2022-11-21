# grpc web 

```bash
protoc -I=../ jsdemo.proto --js_out=import_style=commonjs,binary:./src --grpc-web_out=import_style=typescript,mode=grpcweb:./src
```