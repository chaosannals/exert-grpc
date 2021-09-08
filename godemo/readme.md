# godemo

## 安装构造工具

注：同样需要预先下载 protoc 并置于 Path 环境变量可查到的路径下。

```bash
# 安装构造工具 go 插件，确保 GOPATH 下 bin 在 Path 环境变量里。
go install google.golang.org/protobuf/cmd/protoc-gen-go@v1.26
go install google.golang.org/grpc/cmd/protoc-gen-go-grpc@v1.1
```

## 库安装注意

```bash
# 使用下面的命名安装不了
go get google.golang.org/protobuf

# 是通过下面安装的 google 的 protobuf
go get google.golang.org/protobuf/reflect/protoreflect@v1.26.0
go get google.golang.org/protobuf/runtime/protoimpl@v1.26.0

```

## 生成文件。

```bash
protoc --proto_path=../  --go_out=./  --go-grpc_out=./ ../demobook.proto
protoc --proto_path=../  --go_out=./  --go-grpc_out=./ ../demotester.proto
```
