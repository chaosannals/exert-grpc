package exert.grpc.demo

import io.grpc.Server
import io.grpc.ServerBuilder
import exert.grpc.demo.services.BookService

open class Server(private val port: Int) {
    val server: Server = ServerBuilder
        .forPort(port)
        .addService(BookService())
        .build()

    fun start() {
        server.start()
        println("Server started, listening on $port")
        Runtime.getRuntime().addShutdownHook(
            Thread {
                println("*** shutting down gRPC server since JVM is shutting down")
                this@Server.stop()
                println("*** server shut down")
            }
        )
    }

    private fun stop() {
        server.shutdown()
    }

    fun blockUntilShutdown() {
        server.awaitTermination()
    }
}