import logging
import grpc
import pydemo_pb2
import pydemo_pb2_grpc

def run():
    with grpc.insecure_channel('127.0.0.1:50051') as channel:
        stub = pydemo_pb2_grpc.GreeterStub(channel)
        response = stub.SayHello(pydemo_pb2.HelloRequest(name='you'))
        print(response.message)

if __name__ == '__main__':
    logging.basicConfig()
    run()