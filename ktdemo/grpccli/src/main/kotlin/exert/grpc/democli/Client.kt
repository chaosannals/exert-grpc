package exert.grpc.democli

import java.io.Closeable
import java.util.concurrent.TimeUnit
import io.grpc.ManagedChannel
import exert.grpc.ktdemo.proto.DemoBookGrpcKt
import exert.grpc.ktdemo.proto.demoBookRequest

open class Client(private val channel: ManagedChannel) : Closeable {
    private val stub: DemoBookGrpcKt.DemoBookCoroutineStub = DemoBookGrpcKt.DemoBookCoroutineStub(channel)

    suspend fun peekBookName(bid: Long) {
        val request = demoBookRequest { id = bid }
        val response = stub.peekBookName(request)
        println("from server: ${response.name}")
    }

    override fun close() {
        channel.shutdown().awaitTermination(5, TimeUnit.SECONDS)
    }
}