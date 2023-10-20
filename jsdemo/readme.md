# grpc js demo

- dynclient nodejs 客户端
- dynserver nodejs 服务端
- jswb grpc-web 打包项目，生成物输出到 v3demo/publish
- v3demo vue 3 vite ts 前端项目，目前无法直接使用，没有集成到脚手架的 grpc 代理。只能通过 nginx grpc 代理，通过原生项目使用 grpc web。

GRPC 不能直接在浏览器使用客户端，所以有了 grpc-web（ts 前端项目的 GRPC 的变种，需要特定的服务端与此类浏览器端库对接）。比如 grpcweb（是个 golang 后端项目，名字中间没有减号） grpcwebproxy (也是 golang 后端项目)这2个项目。
