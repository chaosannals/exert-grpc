package rpci

import (
	"context"
	"log"
	"time"

	"github.com/chaosannals/exert-grpc-godemo/gopb"
	"google.golang.org/grpc"
)

func CallDemoBook(address string, bid int64) error {
	conn, err := grpc.Dial(address, grpc.WithInsecure(), grpc.WithBlock())
	if err != nil {
		log.Printf("not connect: %v", address)
		return err
	}
	defer conn.Close()
	c := gopb.NewDemoBookClient(conn)

	ctx, cancel := context.WithTimeout(context.Background(), time.Second)
	defer cancel()

	r, err := c.GetName(ctx, &gopb.DemoBookRequest{Id: bid})
	if err != nil {
		log.Printf("not getname: %v", address)
		return err
	}
	log.Printf("GetName %s", r.GetName())
	return nil
}
