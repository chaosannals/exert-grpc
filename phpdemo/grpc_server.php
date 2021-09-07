<?php

use Grpc\ServerContext;
use Grpc\RpcServer;
use Grpcdemo\DemoBookStub;
use Grpcdemo\DemoBookRequest;
use Grpcdemo\DemoBookReply;
use Grpcdemo\DemoTesterStub;

require dirname(__FILE__) . '/vendor/autoload.php';

class DemoBookImpl extends DemoBookStub
{
    public function GetName(DemoBookRequest $request, ServerContext $serverContext): ?DemoBookReply {
        $id = $request->getId();
        $response = new DemoBookReply();
        $response->setName("Hello " . $id);
        return $response;
    }
}

$server = new RpcServer();
$server->addHttp2Port('0.0.0.0:50051');
$server->handle(new DemoBookImpl());
$server->run();