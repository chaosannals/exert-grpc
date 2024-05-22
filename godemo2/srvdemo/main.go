package main

import (
	"context"
	// "crypto/rand"
	"crypto/tls"
	"crypto/x509"
	"fmt"
	"log"
	"net"
	"os"
	"path/filepath"

	// "time"

	"github.com/chaosannals/grpcdemo/srvdemo/gopb"
	"google.golang.org/grpc"
	"google.golang.org/grpc/credentials"
)

type MyServer struct {
	gopb.UnimplementedDemoBookServer
}

func (s *MyServer) GetName(ctx context.Context, in *gopb.DemoBookRequest) (*gopb.DemoBookReply, error) {
	log.Printf("book getname: %v", in.GetId())
	return &gopb.DemoBookReply{
		Name: fmt.Sprintf("book - %d", in.GetId()),
	}, nil
}

// 用 go run srvdemo 运行。
func main() {
	fmt.Println("start")
	exeDir := filepath.Dir(os.Args[0]) // run 会导致飘到临时目录。
	pwd, err := os.Getwd()             // 定位是 cmd 当前
	if err != nil {
		log.Fatal(err)
	}

	fmt.Printf("exeDir: %s\n pwd: %s\n", exeDir, pwd)
	// caPath := filepath.Join(pwd, "ca-cert.pem")
	// cerPath := filepath.Join(pwd, "server-cert.pem")
	// keyPath := filepath.Join(pwd, "server-key.pem")
	caPath := filepath.Join(pwd, "cert/ca-cert.pem")
	cerPath := filepath.Join(pwd, "cert/server-cert.pem")
	keyPath := filepath.Join(pwd, "cert/server-key.pem")
	fmt.Printf("caPath: %s \ncerPath: %s\nkeyPath: %s\n", caPath, cerPath, keyPath)
	caBytes, err := os.ReadFile(caPath)
	if err != nil {
		log.Fatal(err)
	}

	certPool := x509.NewCertPool()
	if !certPool.AppendCertsFromPEM(caBytes) {
		log.Fatal(err)
	}

	crt, err := tls.LoadX509KeyPair(cerPath, keyPath)
	if err != nil {
		log.Fatal(err)
	}

	tlsConf := &tls.Config{
		Certificates: []tls.Certificate{crt},
		ClientAuth:   tls.RequireAndVerifyClientCert, // 要求客户端带证书
		ClientCAs:    certPool,
	}
	// tlsConf.Time = time.Now
	// tlsConf.Rand = rand.Reader
	tlsCreds := credentials.NewTLS(tlsConf)
	server := grpc.NewServer(grpc.Creds(tlsCreds))
	gopb.RegisterDemoBookServer(server, &MyServer{})
	listener, err := net.Listen("tcp", ":12345")

	if err != nil {
		log.Fatal(err)
	}
	if err := server.Serve(listener); err != nil {
		log.Fatal(err)
	}
}
