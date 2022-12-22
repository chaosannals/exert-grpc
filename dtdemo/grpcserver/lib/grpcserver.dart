import 'package:fixnum/fixnum.dart';
import 'package:grpc/grpc.dart';
import 'package:grpcserver/grpcg/bookdemo.pbgrpc.dart';

class BookDemoService extends BookDemoShopServiceBase {
  @override
  Future<BookReply> find(ServiceCall call, BookRequest request) async {
    var reply = BookReply();
    reply.code = Int64(0);
    reply.message = request.name;
    return reply;
  }
}

int calculate() {
  return 6 * 7;
}
