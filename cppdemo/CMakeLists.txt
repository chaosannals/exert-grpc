cmake_minimum_required(VERSION 3.24)

set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_CXX_STANDARD 20)

include(FetchContent)

project(cppdemo)

set(BUILD_TESTING OFF)

FetchContent_Declare(
  gRPC
  GIT_REPOSITORY https://github.com/grpc/grpc
  # GIT_REPOSITORY git@github.com:grpc/grpc.git
  GIT_TAG        v1.52.1
)
set(FETCHCONTENT_QUIET OFF)
FetchContent_MakeAvailable(gRPC)

## proto 文件打包成库
set(CPPDEMO_OUT_DIR "${CMAKE_CURRENT_BINARY_DIR}")
get_filename_component(CPPDEMO_PROTO_PATH "./cppdemo.proto" ABSOLUTE)
get_filename_component(CPPDEMO_PROTO_DIR "${CPPDEMO_PROTO_PATH}" PATH)
message(STATUS "${_PROTOBUF_PROTOC} proto: ${CPPDEMO_PROTO_PATH} ${CPPDEMO_PROTO_DIR}")

set(CPPDEMO_PROTO_SRC "${CPPDEMO_OUT_DIR}/cppdemo.pb.cc")
set(CPPDEMO_PROTO_INC "${CPPDEMO_OUT_DIR}/cppdemo.pb.h")
set(CPPDEMO_GRPC_SRC "${CPPDEMO_OUT_DIR}/cppdemo.grpc.pb.cc")
set(CPPDEMO_GRPC_INC "${CPPDEMO_OUT_DIR}/cppdemo.grpc.pb.h")

set(_PROTOBUF_LIBPROTOBUF libprotobuf)
set(_REFLECTION grpc++_reflection)
set(_PROTOBUF_PROTOC $<TARGET_FILE:protoc>)
set(_GRPC_CPP_PLUGIN_EXECUTABLE $<TARGET_FILE:grpc_cpp_plugin>)
set(_GRPC_GRPCPP grpc++)

add_custom_command(
      OUTPUT "${CPPDEMO_PROTO_SRC}" "${CPPDEMO_PROTO_INC}" "${CPPDEMO_GRPC_SRC}" "${CPPDEMO_GRPC_INC}"
      COMMAND ${_PROTOBUF_PROTOC}
      ARGS --grpc_out "${CPPDEMO_OUT_DIR}"
        --cpp_out "${CPPDEMO_OUT_DIR}"
        -I "${CPPDEMO_PROTO_DIR}"
        --plugin=protoc-gen-grpc="${_GRPC_CPP_PLUGIN_EXECUTABLE}"
        "${CPPDEMO_PROTO_PATH}"
      DEPENDS "${CPPDEMO_PROTO_PATH}")

message(STATUS "top: ${CMAKE_CURRENT_BINARY_DIR}")

add_library(
    cppdemo_grpc_proto
    ${CPPDEMO_PROTO_SRC}
    ${CPPDEMO_PROTO_INC}
    ${CPPDEMO_GRPC_SRC}
    ${CPPDEMO_GRPC_INC}
)
target_link_libraries(
    cppdemo_grpc_proto
    ${_REFLECTION}
    ${_GRPC_GRPCPP}
    ${_PROTOBUF_LIBPROTOBUF}
)


add_subdirectory(clidemo)
add_subdirectory(srvdemo)