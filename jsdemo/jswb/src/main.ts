import { RpcError, Status } from 'grpc-web';
import {
    SayJsDemoRequest,
    SayJsDemoReply,
} from './jsdemo_pb';
import { JsDemoClient } from './JsdemoServiceClientPb';

export class Jswb {
    private client: JsDemoClient;

    constructor(
        url: string = 'http://localhost:50051',
        credentials?: null | { [index: string]: string; },
        options?: null | { [index: string]: any; }
    ) {
        this.client = new JsDemoClient(url, credentials, options);
    }

    async request(content: string) : Promise<any> {
        return new Promise<any>((resolve, reject) => {
            const req = new SayJsDemoRequest();
            req.setContent(content);
            const call = this.client.say(req, {
                'my-http-header-1': '12312',
            }, (err: RpcError, response: SayJsDemoReply) => {
                if (err) {
                    reject(err);
                } else {
                    resolve(response);
                }
                console.log('response', err, response);
            });
            call.on('status', (status: Status) => {
                console.log('status', status);
            });
        });
    }
}

declare global {
    interface Window {
        Jswb: any,
    }    
}

window.Jswb = Jswb;
