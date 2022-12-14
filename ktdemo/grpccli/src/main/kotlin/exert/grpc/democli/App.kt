package exert.grpc.democli

import kotlin.random.Random
import java.util.concurrent.TimeUnit
import io.grpc.ManagedChannelBuilder

class App {

}

suspend fun main(args: Array<String>) {
    println("client start.")
    val port = 40441
    val channel = ManagedChannelBuilder
        .forAddress("localhost", port)
        .usePlaintext()
        .build()
    val client = Client(channel)
    
    var r = Random(System.nanoTime())
    while (true) {
        TimeUnit.SECONDS.sleep(1L)
        try {
            client.peekBookName(r.nextInt(100).toLong())
        }
        catch (e: Exception) {
            print("exception:")
            println(e)
        }
    }
}