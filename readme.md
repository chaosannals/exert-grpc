# exert-grpc

## 下载 protoc

[下载地址](https://github.com/protocolbuffers/protobuf/releases)

下载到 protoc 文件，放置于 Path 环境变量路径内。

## 构建 protoc 插件

工具：

1. git
2. cmake
3. visual studio 2019

注：通过 vs2019 的构建命令行（x64 Native Tools Command）进行操作。

```bash
# 指定 v1.39.1 版本 clone 代码
git clone --recursive -b v1.39.1 https://github.com/grpc/grpc

# 进到 代码 目录
cd grpc

# 初始化 git 子模块
git submodule update --init

# 创建构建目录
mkdir .build

# 进入构建目录
cd .build

# 用 Cmake 生成 VS2019 项目
cmake .. -G "Visual Studio 16 2019"

# 执行项目的构建
cmake --build . --config Release
```
