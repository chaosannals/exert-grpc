@echo off

echo %~dp0

@rem proto 目录
echo %1

protoc --ts_out=src/grpc --plugin=protoc-gen-ts=%~dp0/node_modules/.bin/protoc-gen-ts.cmd -I=%1 %1*.proto
