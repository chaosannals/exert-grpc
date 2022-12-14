package exert.grpc.clidemo;

import io.grpc.ManagedChannel;
import io.grpc.ManagedChannelBuilder;

import java.util.Random;

import exert.grpc.javademo.proto.DemoBookGrpc;
import exert.grpc.javademo.proto.DemoBookRequest;
import exert.grpc.javademo.proto.DemoBookReply;

public class App {
    public static void main(String[] args) {
        int port = 40041;
        String host = "localhost";
        try {
            ManagedChannel channel = ManagedChannelBuilder
                .forAddress(host, port)
                .usePlaintext()
                .build();
            DemoBookGrpc.DemoBookBlockingStub stub = DemoBookGrpc
                .newBlockingStub(channel);
            
            Random random = new Random();
            while (true) {
                try {
                    Thread.sleep(1000);
                    long id = random.nextLong();
                    DemoBookRequest request = DemoBookRequest
                        .newBuilder()
                        .setId(id)
                        .build();
                    DemoBookReply reply = stub.getName(request);
                    System.out.println(reply.getName());
                } catch (Exception e) {
                    System.out.println(e);
                }
            }

        } catch (Exception e) {
            System.out.println(e);
        }
    }
}
