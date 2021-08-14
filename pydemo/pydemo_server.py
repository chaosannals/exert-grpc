
import logging
import sys
import multiprocessing
import grpc
from datetime import timedelta
from concurrent import futures
from pydemo_pb2_grpc import GreeterServicer, add_GreeterServicer_to_server
from pydemo_pb2 import HelloReply

LOG = logging.getLogger(__name__)
WAIT_SECONDS = timedelta(days=1).total_seconds()

class Greeter(GreeterServicer):

    def SayHello(self, request, context):
        return HelloReply(message=f'Hello, {request.name}')

def serve(bind_address):
    LOG.info('Start new serve')

    server = grpc.server(
        futures.ThreadPoolExecutor(max_workers=multiprocessing.cpu_count(),),
    )
    add_GreeterServicer_to_server(Greeter(), server)
    server.add_insecure_port(bind_address)
    server.start()
    server.wait_for_termination()

if __name__ == '__main__':
    handler = logging.StreamHandler(sys.stdout)
    formatter = logging.Formatter('[PID %(process)d] %(message)s')
    handler.setFormatter(formatter)
    LOG.addHandler(handler)
    LOG.setLevel(logging.INFO)
    serve('0.0.0.0:50051')