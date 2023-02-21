#include <iostream>
#include <grpcpp/grpcpp.h>
#include <cppdemo.pb.h>
#include <cppdemo.grpc.pb.h>

using grpc::Server;
using grpc::ServerBuilder;
using grpc::ServerContext;
using grpc::Status;
using cppd::cppdemo;
using cppd::ack_request;
using cppd::ack_reply;

class cppdemo_service : public cppdemo::Service {
	Status ack(ServerContext* context, const ack_request* request, ack_reply* response) {
		context->set_compression_algorithm(GRPC_COMPRESS_DEFLATE);
		response->add_msg(request->msg());
		response->add_msg("bbbb");
		response->set_code(0);
		return Status::OK;
	}
};

int main() {
	std::string server_address("0.0.0.0:50051");
	cppdemo_service service;

	ServerBuilder builder;
	builder.SetDefaultCompressionAlgorithm(GRPC_COMPRESS_GZIP); // 是用 GZIP 算法，客户端要匹配
	builder.AddListeningPort(server_address, grpc::InsecureServerCredentials());
	builder.RegisterService(&service);

	std::unique_ptr<Server> server(builder.BuildAndStart());

	std::cout << "listen: " << server_address << std::endl;

	server->Wait();
	return 0;
}