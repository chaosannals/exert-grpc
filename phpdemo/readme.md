# exert-grpc-phpdemo

## 安装 grpc 的 c 扩展

### Windows 下

[PECL](https://pecl.php.net) 下载指定版本的 DLL

添加 php.ini

```ini
extension=grpc
```

### Linux 下

```bash
# 安装 pecl 后通过 pecl 安装。
pecl install grpc

# 或者 指定版本
pecl install grpc-1.39.1
```

添加 php.ini

```ini
extension=grpc.so
```

## 生成文件

```bash
# 生成 PHP 文件。
protoc --proto_path=../ --php_out=./lib --grpc_out=generate_server:./lib  --plugin=protoc-gen-grpc=D:\Toolkit\grpc_php_plugin.exe ../demobook.proto

protoc --proto_path=../ --php_out=./lib --grpc_out=generate_server:./lib  --plugin=protoc-gen-grpc=D:\Toolkit\grpc_php_plugin.exe ../demotester.proto
```
