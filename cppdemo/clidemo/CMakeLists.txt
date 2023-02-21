add_executable(clidemo)

message(STATUS "cli: ${CMAKE_CURRENT_BINARY_DIR}")

# 引入 grpc 依赖库
target_link_libraries(
	clidemo
	PRIVATE
	cppdemo_grpc_proto
	${_REFLECTION}
    ${_GRPC_GRPCPP}
    ${_PROTOBUF_LIBPROTOBUF}
)

# 引入 proto 生成的头文件
target_include_directories(
	clidemo
	PRIVATE
	"${CPPDEMO_OUT_DIR}"
)

target_sources(
	clidemo
	PRIVATE
	"src/main.cpp"
)