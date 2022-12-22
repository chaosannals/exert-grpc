import 'package:grpc/grpc.dart';
import 'package:grpcserver/grpcserver.dart';

void main(List<String> arguments) async {
  final server = Server([BookDemoService()], const <Interceptor>[],
      CodecRegistry(codecs: const [GzipCodec(), IdentityCodec()]));
  await server.serve(port: 40041);
  print('server listen ${server.port}');
}
