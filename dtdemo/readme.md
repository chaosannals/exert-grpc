# Dart Demo

```bash
# 需要手动下载和配置 protoc

# 全局安装 grpc 扩展例如（C:\Users\<UserName>\AppData\Local\Pub\Cache\bin）这个路径没有在PATH 里
# 所以 protoc 会找不到(protoc-gen-dart)，自己复制到 PATH 路径下，或者路径加到 PATH 里面。
dart pub global activate protoc_plugin

# 生成服务端代码，grpcserver/lib/grpcg 目录也不会自己创建，自己建。。。
protoc --dart_out=grpc:grpcserver/lib/grpcg -I./ bookdemo.proto

# 生成客户端代码，grpcclient/lib/grpcg 目录也不会自己创建，自己建。。。
protoc --dart_out=grpc:grpcclient/lib/grpcg -I./ bookdemo.proto
```