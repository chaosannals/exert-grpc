# web grpc envoy 代理

官方只提供了 webpack 打包生成物的示例。
vitejsgrpc 会缺少打包工具，而出现各种缺少文件等问题。
vitetsgrpc 通过第三方提供的 protoc 插件 @protobuf-ts/grpcweb-transport 生成

所以 vitegrpc 目前不可用。只有 webpackgrpc 可用。

## 操作

```bash
# nodeserver 下需要
npm ci

# nodeclient 下需要(可选)
npm ci

# 启动镜像
docker compose up -d

# nodeclient 下 可以验证 nodeserver 是否启动(可选)
npm run echo

# webpackgrpc 下生成前端文件
npm run build

# webpackgrpc 打开前端开发服务器
npm run serve
```

## envoy

- 8080 端口 暴露的 web
- 9901 端口 envoy 管理后台

conf/envoy.yaml 配置文件
