# exert-grpc-phpdemo

## 安装 grpc 的 c 扩展

### Windows 下

[PECL](https://pecl.php.net) 下载指定版本的 DLL

添加 php.ini

```ini
extension=grpc
```

### Linux 下

#### 通过 pecl 安装

```bash
# 安装 protobuf
pecl install protobuf

# 安装 grpc
pecl install grpc

# 或者 指定版本
pecl install grpc-1.39.1
```

#### 通过源码安装

注：需要 gcc 4.9+ 版本

```bash
# 安装 protobuf
wget https://pecl.php.net/get/protobuf-3.17.3.tgz
tar -zxvf protobuf-3.17.3.tgz
cd protobuf-3.17.3

# 多个 php 环境下 phpize 和 php 和 php-config 同个 php 运行时（同目录）。
/www/server/php/73/bin/phpize
./configure --with-php-config=/www/server/php/73/bin/php-config
make
make install

# 会得到路径

# 安装 grpc
wget https://pecl.php.net/get/grpc-1.39.0.tgz
tar -zxvf grpc-1.39.0.tgz
cd grpc-1.39.0

# 多个 php 环境下 phpize 和 php 和 php-config 同个 php 运行时（同目录）。
/www/server/php/73/bin/phpize
./configure --with-php-config=/www/server/php/73/bin/php-config

make
make install

# 会得到路径
```

添加 php.ini

```ini
# 根据 make install 得到的路径填写
extension=protobuf.so

# 根据 make install 得到的路径填写
extension=grpc.so
```

## 生成文件

```bash
# 生成 PHP 文件。
protoc --proto_path=../ --php_out=./lib --grpc_out=generate_server:./lib  --plugin=protoc-gen-grpc=D:\Toolkit\grpc_php_plugin.exe ../demobook.proto

protoc --proto_path=../ --php_out=./lib --grpc_out=generate_server:./lib  --plugin=protoc-gen-grpc=D:\Toolkit\grpc_php_plugin.exe ../demotester.proto
```
