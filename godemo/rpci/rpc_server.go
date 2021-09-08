package rpci

import (
	"context"
	"fmt"
	"log"

	"github.com/chaosannals/exert-grpc-godemo/gopb"
)

// Book
type DemoBookServer struct {
	gopb.UnimplementedDemoBookServer
}

func (s *DemoBookServer) GetName(ctx context.Context, in *gopb.DemoBookRequest) (*gopb.DemoBookReply, error) {
	log.Printf("book getname: %v", in.GetId())
	return &gopb.DemoBookReply{
		Name: fmt.Sprintf("book - %d", in.GetId()),
	}, nil
}

// Tester
type DemoTesterServer struct {
	gopb.UnimplementedDemoTesterServer
}

func (s *DemoTesterServer) GetName(ctx context.Context, in *gopb.DemoTesterGetNameRequest) (*gopb.DemoTesterGetNameReply, error) {
	log.Printf("tester getname: %v", in.GetId())
	return &gopb.DemoTesterGetNameReply{
		Code: 0,
		Name: fmt.Sprintf("tester - %d", in.GetId()),
	}, nil
}
func (s *DemoTesterServer) GetInfo(ctx context.Context, in *gopb.DemoTesterGetInfoRequest) (*gopb.DemoTesterGetInfoReply, error) {
	log.Printf("tester getinfo: %v", in.GetId())
	return &gopb.DemoTesterGetInfoReply{
		Code: 0,
		Id:   in.GetId(),
		Name: fmt.Sprintf("tester - %d", in.GetId()),
	}, nil
}
