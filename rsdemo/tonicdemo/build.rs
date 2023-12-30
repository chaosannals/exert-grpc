
fn main() {
    println!("start build proto.");

    // tonic_build::configure()
    //     .type_attribute("")

    tonic_build::compile_protos("../proto/echo.proto").unwrap();

    build_json_codec_service();
}

fn build_json_codec_service() {
    let greeter_service = tonic_build::manual::Service::builder()
        .name("Echo")
        .package("json.echo")
        .method(
            tonic_build::manual::Method::builder()
                .name("say_hello")
                .route_name("SayHello")
                .input_type("crate::common::HelloRequest")
                .output_type("crate::common::HelloResponse")
                .codec_path("crate::common::JsonCodec")
                .build(),
        )
        .build();

    tonic_build::manual::Builder::new().compile(&[greeter_service]);
}