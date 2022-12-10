package exert.grpc.javademo;

import exert.grpc.javademo.proto.DemoBookReply;
import exert.grpc.javademo.proto.DemoBookRequest;
import exert.grpc.javademo.proto.DemoBookGrpc.DemoBookImplBase;
import io.grpc.stub.StreamObserver;

public class BookService extends DemoBookImplBase {
    @Override
    public void getName(DemoBookRequest request, StreamObserver<DemoBookReply> responseObserver) {
        DemoBookReply reply = DemoBookReply
            .newBuilder()
            .setName("aaaaaaa")
            .build();
        responseObserver.onNext(reply);
        responseObserver.onCompleted();
    }
}
