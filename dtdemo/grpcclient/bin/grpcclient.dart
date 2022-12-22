import 'package:grpc/grpc.dart';
import 'package:grpcclient/grpcg/bookdemo.pbgrpc.dart';

void main(List<String> arguments) async {
  final channel = ClientChannel(
    'localhost',
    port: 40041,
    options: ChannelOptions(
      credentials: ChannelCredentials.insecure(),
      codecRegistry:
          CodecRegistry(codecs: const [GzipCodec(), IdentityCodec()]),
    ),
  );
  final stub = BookDemoShopClient(channel);
  try {
    final request = BookRequest(name: "asdfasdf");
    final reply = await stub.find(request,
        options: CallOptions(compression: const GzipCodec()));
    print('reply: ${reply.message}');
  } catch (e) {
    print('catch error $e');
  }
  await channel.shutdown();
}
