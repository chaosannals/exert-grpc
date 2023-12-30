
fn main() {
    println!("start build proto.");

    tonic_build::compile_protos("../proto/echo.proto").unwrap();
}