# go grpc demo 2

```bash
# Windows 下用 WSL 执行，Msys2 有问题。
# 使用 cert/gen.sh 生成证书，需要根据自身修改。
# 如果客户端用 IP 访问 server-ext.cnf 必须指定它访问的 IP ，不然验证不过。
./cert/gen.sh
```

提供证书

/C=TR is for Country
/ST=ASIA is for State or province
/L=ISTANBUL is for Locality name or city
/O=DEV is for Organization
/OU=TUTORIAL is for Organization Unit
/CN=*.tutorial.dev is for Common Name or domain name
/emailAddress=mert@tutorial.com is for email address

```bash
# 生成服务器证书
openssl req -newkey rsa:4096 -nodes -keyout server-key.pem -out server-req.pem -subj "/C=TR/ST=ASIA/L=ISTANBUL/O=DEV/OU=BLOG/CN=*.xxxx.com/emailAddress=xxx@xx.com"

# 或者
openssl req -newkey rsa:4096 -nodes -keyout server-key.pem -out server-req.pem

# 用 CA 和服务器证书 签出证书
openssl x509 -req -in server-req.pem -CA ca-cert.pem -CAkey ca-key.pem -CAcreateserial -out server-cert.pem -extfile server-ext.conf

# 打印信息
openssl x509 -in server-cert.pem -noout -text


# 给客户端签出证书
openssl req -newkey rsa:4096 -nodes -keyout client-key.pem -out client-req.pem -subj "/C=TR/ST=EUROPE/L=ISTANBUL/O=DEV/OU=CLIENT/CN=*.someclient.com/emailAddress=someclient@gmail.com"

# 或者
openssl req -newkey rsa:4096 -nodes -keyout client-key.pem -out client-req.pem

# 用 CA 给 客户端签出证书
openssl x509 -req -in client-req.pem -days 60 -CA ca-cert.pem -CAkey ca-key.pem -CAcreateserial -out client-cert.pem -extfile client-ext.conf
```

## 依赖

```bash
# 安装工具
go install google.golang.org/protobuf/cmd/protoc-gen-go@v1.34.1
go install google.golang.org/grpc/cmd/protoc-gen-go-grpc@v1.3.0
```

```bash
# PB 库，低版本使用下面的命名安装不了，高版本可以。
go get google.golang.org/protobuf

# GRPC 库，
go get google.golang.org/grpc
```

## 生成

```bash
# 需要在各自目录下生成
protoc --proto_path=../../  --go_out=./  --go-grpc_out=./ ../../demobook.proto
```