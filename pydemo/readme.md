# PYDemo

```bash
# 工具
python -m pip install grpcio
python -m pip install grpcio-tools
```

```bash
# 只输出 pb 旧，这个版本会把 数据结构输出到 *_pb2.py 文件。
python -m grpc_tools.protoc -I. --python_out=. ./pydemo.proto

# 只输出 pb 新，新版本必须指定并输出一个 *_pb2.pyi 的文件，结构在这个里面。
python -m grpc_tools.protoc -I. --python_out=. --pyi_out=. ./pydemo.proto

# 输出 pb 和 grpc
python -m grpc_tools.protoc -I. --python_out=. --grpc_python_out=. ./pydemo.proto
```