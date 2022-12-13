package exert.grpc.demo

class App {}

fun main(args: Array<String>) {

    args.map {
        println(it)
    }
    println("start server.")
    var port = System.getenv("PORT")?.toInt() ?: 40441
    var server = Server(port)
    server.start()
    server.blockUntilShutdown()
}