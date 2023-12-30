pub mod echopb {
    tonic::include_proto!("/grpc.examples.echo");
}

#[derive(Default)]
pub struct MyEchoClient {}

#[tokio::main]
async fn main() -> Result<(), Box<dyn std::error::Error>> {
    println!("start client");

    let mut client = echopb::echo_client::EchoClient::connect("http://[::1]:40041").await?;
    let request = tonic::Request::new(echopb::EchoRequest {
        message: "aaaa".to_string(),
    });
    let response = client.unary_echo(request).await?;

    println!("response: {:?}", response.into_inner().message);

    Ok(())
}
