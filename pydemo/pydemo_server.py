
import logging
import sys
import socket
import contextlib
import multiprocessing
import grpc
import time
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
        options=(('grpc.so_reuseport', 1),)
    )
    add_GreeterServicer_to_server(Greeter(), server)
    server.add_insecure_port(bind_address)
    server.start()
    try:
        while True:
            time.sleep(WAIT_SECONDS)
    except KeyboardInterrupt:
        server.stop(None)

@contextlib.contextmanager
def get_port(port):
    '''
    '''
    
    sock = socket.socket(socket.AF_INET6, socket.SOCK_STREAM)
    # 目前只有 Linux 下支持 端口复用。
    sock.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEPORT, 1)
    if sock.getsockopt(socket.SOL_SOCKET, socket.SO_REUSEPORT) == 0:
        raise RuntimeError('Failed to set SO_REUSEPORT.')
    sock.bind(('', port))
    try:
        yield sock.getsockname()[1]
    finally:
        sock.close()

def main():
    with get_port(0) as port:
        bind_address = f'localhost:{port}'
        workers = []
        for _ in range(multiprocessing.cpu_count()):
            worker = multiprocessing.Process(
                target=serve,
                args=(bind_address,)
            )
            worker.start()
            workers.append(worker)
        for worker in workers:
            worker.join()

if __name__ == '__main__':
    handler = logging.StreamHandler(sys.stdout)
    formatter = logging.Formatter('[PID %(process)d] %(message)s')
    handler.setFormatter(formatter)
    LOG.addHandler(handler)
    LOG.setLevel(logging.INFO)
    main()