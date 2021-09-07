<?php

use Grpc\ServerContext;
use Grpc\ChannelCredentials;
use Grpcdemo\DemoBookClient;
use Grpcdemo\DemoBookRequest;
use Grpcdemo\DemoBookReply;
use Grpcdemo\DemoTesterClient;

require dirname(__FILE__).'/vendor/autoload.php';

function greet($hostname, $id)
{
    $client = new DemoBookClient($hostname, [
        'credentials' => ChannelCredentials::createInsecure(),
    ]);
    $request = new DemoBookRequest();
    $request->setId($id);
    list($response, $status) = $client->GetName($request)->wait();
    if ($status->code !== Grpc\STATUS_OK) {
        echo "ERROR: " . $status->code . ", " . $status->details . PHP_EOL;
        exit(1);
    }
    echo $response->getName() . PHP_EOL;
}

$name = !empty($argv[1]) ? $argv[1] : '1';
$hostname = !empty($argv[2]) ? $argv[2] : 'localhost:50051';
greet($hostname, $name);