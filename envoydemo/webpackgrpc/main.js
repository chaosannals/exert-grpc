const {
  EchoRequest,
} = require("./grpc/echo_pb.js");
const { EchoServiceClient } = require("./grpc/echo_grpc_web_pb.js");

const grpc = {};
grpc.web = require("grpc-web");

var echoService = new EchoServiceClient(
  "http://" + window.location.hostname + ":8080",
  null,
  null
);

function echo(msg) {
  var unaryRequest = new EchoRequest();
  unaryRequest.setMessage(msg);
  var call = echoService.echo(
    unaryRequest,
    { "custom-header-1": "value1" },
    function (err, response) {
      console.log("echo", err, response);
      if (err) {
        console.log(
          "Error code: " + err.code + ' "' + err.message + '"'
        );
      } else {
        setTimeout(function () {
          console.log(response.getMessage());
        }, 1000);
      }
    }
  );

  call.on('status', function(status) {
    if (status.metadata) {
      console.log("Received metadata");
      console.log(status.metadata);
    }
  });
}

echo("aaaaa");