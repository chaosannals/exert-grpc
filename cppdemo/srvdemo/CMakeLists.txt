add_executable(srvdemo)

message(STATUS "srv: ${CMAKE_CURRENT_BINARY_DIR}")

target_link_libraries(
	srvdemo
	PRIVATE
	cppdemo_grpc_proto
	${_REFLECTION}
    ${_GRPC_GRPCPP}
    ${_PROTOBUF_LIBPROTOBUF}
)

# 引入 proto 生成的头文件
target_include_directories(
	srvdemo
	PRIVATE
	"${CPPDEMO_OUT_DIR}"
)

target_sources(
	srvdemo
	PRIVATE
	"src/main.cpp"
)