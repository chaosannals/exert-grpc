use tonic::{transport::Server, Request, Response, Status};

pub mod echopb {
    tonic::include_proto!("/grpc.examples.echo");
}

#[derive(Default)]
pub struct MyEcho {}

#[tonic::async_trait]
impl echopb::echo_server::Echo for MyEcho {
    async fn unary_echo(
        &self,
        request: Request<echopb::EchoRequest>,
    ) -> Result<Response<echopb::EchoResponse>, Status> {
        println!("got a request from {:?}", request.remote_addr());

        let response = echopb::EchoResponse {
            message: format!("the message: {}", request.into_inner().message),
        };
        Ok(Response::new(response))
    }
}


#[tokio::main]
async fn main() -> Result<(), Box<dyn std::error::Error>> {
    let addr = "[::1]:40041".parse()?;
    let echo = MyEcho::default();
    
    println!("start echo server on {}", addr);

    Server::builder()
        .add_service(echopb::echo_server::EchoServer::new(echo))
        .serve(addr)
        .await?;

    Ok(())
}
