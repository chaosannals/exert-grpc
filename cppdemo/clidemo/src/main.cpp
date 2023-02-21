#include <iostream>
#include <memory>
#include <string>
#include <numeric>
#include <format>
#include <grpcpp/grpcpp.h>
#include<cppdemo.pb.h>
#include<cppdemo.grpc.pb.h>

using grpc::Channel;
using grpc::ChannelArguments;
using grpc::ClientContext;
using grpc::Status;
using cppd::cppdemo;
using cppd::ack_request;
using cppd::ack_reply;

class cppdemo_client {
	std::unique_ptr<cppdemo::Stub> stub;
public:
	cppdemo_client(std::shared_ptr<Channel> channel)
		:stub(cppdemo::NewStub(channel)) {
	}
	std::string ack() {
		ack_request request;
		request.set_id(1);
		request.set_msg("aaaaa");

		ack_reply reply;

		ClientContext context;
		context.set_compression_algorithm(GRPC_COMPRESS_DEFLATE); // 使用默认压缩算法

		Status status = stub->ack(&context, request, &reply);

		if (status.ok()) {
			return reply.msg()[0];
		}
		else {
			return std::format(
				"grpc failed: {} {}",
				(int)status.error_code(),
				status.error_message()
			);
		}
	}
};

int main() {
	ChannelArguments args;
	args.SetCompressionAlgorithm(GRPC_COMPRESS_GZIP); // 使用 GZ 压缩
	cppdemo_client client(
		grpc::CreateCustomChannel(
			"localhost:50051",
			grpc::InsecureChannelCredentials(),
			args
		)
	);
	auto r = client.ack();
	std::cout << "r: " << r << std::endl;
	return 0;
}