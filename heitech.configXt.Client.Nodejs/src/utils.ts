import * as mq from 'zeromq';
import { ContextModel } from './contextModel';
import { UiOperationResult } from './uiOperationResult';
const tcpConnect = "tcp://localhost:5557";

// send to config service
export function busSend(context : ContextModel, selectUseCaseCallback : Function){
    let socket = mq.socket('req');
    socket.connect(tcpConnect);

    socket.on('message', function(msg){
        let result = JSON.parse(msg.toString());
        let uiResult = UiOperationResult.FromMsg(result);
        console.log(uiResult.format());
        
        selectUseCaseCallback(context.appName, context.user);
    });

    socket.send(JSON.stringify(context));
}