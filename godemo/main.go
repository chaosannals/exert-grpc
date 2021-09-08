package main

import (
	"log"
	"net"
	"time"

	"github.com/chaosannals/exert-grpc-godemo/gopb"
	"github.com/chaosannals/exert-grpc-godemo/rpci"
	"google.golang.org/grpc"
)

const (
	address = "localhost:50051"
	port    = ":50051"
)

func main() {
	lis, err := net.Listen("tcp", port)
	if err != nil {
		log.Fatalf("failed to listen: %v", err)
	}
	s := grpc.NewServer()
	gopb.RegisterDemoBookServer(s, &rpci.DemoBookServer{})
	gopb.RegisterDemoTesterServer(s, &rpci.DemoTesterServer{})
	log.Printf("server listening at %v", lis.Addr())
	go func() {
		log.Printf("start client wait 5 second.")
		time.Sleep(time.Duration(5) * time.Second)
		for i := 0; i <= 10; i += 1 {
			log.Printf("start client call demobook")
			err := rpci.CallDemoBook(address, 1000+int64(i))
			if err != nil {
				log.Printf("client call error %v", err)
			}
			time.Sleep(time.Duration(2) * time.Second)
		}
	}()
	log.Printf("start server")
	if err := s.Serve(lis); err != nil {
		log.Fatalf("failed to server: %v", err)
	}
}
