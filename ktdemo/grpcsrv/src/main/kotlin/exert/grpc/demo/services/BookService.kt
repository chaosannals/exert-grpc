package exert.grpc.demo.services

import exert.grpc.ktdemo.proto.DemoBookGrpcKt
import exert.grpc.ktdemo.proto.DemoBookRequest
import exert.grpc.ktdemo.proto.DemoBookReply
import exert.grpc.ktdemo.proto.demoBookReply

// 同步继承 *Grpc.*ImplBase
// 异步继承 *GrpcKt.*CoroutineImplBase
open class BookService : DemoBookGrpcKt.DemoBookCoroutineImplBase() {
    override suspend fun peekBookName(request: DemoBookRequest) : DemoBookReply {
        var r = demoBookReply {
            name = "book name by id: ${request.id}"
        }
        return r
    }
}