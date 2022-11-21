# Vue 3 GRPC WEB DEMO

依赖 protoc 命令。

依赖 [下载 protoc-gen-grpc-web](https://github.com/grpc/grpc-web/releases)
重命名后放到 PATH 目录里面，因为文件带了版本号。

```bash
# 生成 JS 的是 google closure 的包模式，只能通过浏览器原生的方式引入获取对象后再由 vue 去调用。
protoc -I=../ jsdemo.proto --js_out=import_style=commonjs:./grpc --grpc-web_out=import_style=commonjs,mode=grpcwebtext:./grpc

# 生成 TS.D 目前 TS 属于实验性功能。生成的是 google closure 的包模式，无法直接 ES import
protoc -I=../ jsdemo.proto --js_out=import_style=commonjs,binary:./grpc --grpc-web_out=import_style=typescript,mode=grpcweb:./grpc
```

```bash
# npm install ts-protoc-gen
protoc -I=../ --ts_out=service=true:./igrpc --js_out=import_style=commonjs,binary:./igrpc jsdemo.proto
```