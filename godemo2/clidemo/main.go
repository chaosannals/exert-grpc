package main

import (
	"context"
	"crypto/tls"
	"crypto/x509"
	"fmt"
	"log"
	"os"
	"path/filepath"
	"time"

	"github.com/chaosannals/grpcdemo/clidemo/gopb"
	"google.golang.org/grpc"
	"google.golang.org/grpc/credentials"
)

func main() {
	exeDir := filepath.Dir(os.Args[0]) // run 会导致飘到临时目录。
	pwd, err := os.Getwd()             // 定位是 cmd 当前
	if err != nil {
		log.Fatal(err)
	}
	fmt.Printf("exeDir: %s\n pwd: %s\n", exeDir, pwd)
	caPath := filepath.Join(pwd, "ca-cert.pem")
	cerPath := filepath.Join(pwd, "client-cert.pem")
	keyPath := filepath.Join(pwd, "client-key.pem")
	fmt.Printf("caPath: %s\ncerPath: %s\nkeyPath: %s\n", caPath, cerPath, keyPath)
	caBytes, err := os.ReadFile(caPath)
	if err != nil {
		log.Fatal(err)
	}

	certPool := x509.NewCertPool()
	if !certPool.AppendCertsFromPEM(caBytes) {
		log.Fatal(fmt.Errorf("CA 添加失败"))
	}

	clientCert, err := tls.LoadX509KeyPair(cerPath, keyPath)
	if err != nil {
		log.Fatal(err)
	}

	tlsConf := &tls.Config{
		Certificates: []tls.Certificate{clientCert},
		RootCAs:      certPool,
	}
	tlsCreds := credentials.NewTLS(tlsConf)

	// TODO 网上示例，一直失败，好像废弃了这个接口
	fmt.Println("start")
	conn, err := grpc.Dial(
		"127.0.0.1:12345",
		grpc.WithTransportCredentials(tlsCreds),
	)
	if err != nil {
		log.Fatal(err)
	}
	defer conn.Close()
	fmt.Println("conn")

	client := gopb.NewDemoBookClient(conn)
	fmt.Println("new client")

	ctx, cancel := context.WithTimeout(context.Background(), 4*time.Second)
	defer cancel()
	fmt.Println("timeout set")
	r, err := client.GetName(ctx, &gopb.DemoBookRequest{Id: 123456})
	if err != nil {
		log.Fatal(err)
	}

	log.Printf("GetName %s", r.GetName())

}
