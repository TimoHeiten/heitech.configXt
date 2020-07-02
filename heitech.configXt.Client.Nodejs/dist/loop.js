"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const mq = require("zeromq");
const contextModel_1 = require("./contextModel");
// 1 user prompt
// 2 inquire User: Get, Add, Delete, update
// 3 inquire ConfigEntity: Upload, Get, getall, Add, update, remove
// trace
// connect to zeroMQ with request socket
const tcpConnect = "tcp://localhost:5557";
let socket = mq.socket('req');
socket.connect(tcpConnect);
var replyNbr = 0;
socket.on('message', function (msg) {
    console.log('got: ' + replyNbr, msg.toString());
    replyNbr += 1;
});
var addUserContext = contextModel_1.createUserContext("typescript-user", "strong-password", "RabbitMQ", "MVC-1");
socket.send(JSON.stringify(addUserContext));
// for blocking the ui with key press
var readline = require('readline');
var rl = readline.createInterface(process.stdin, process.stdout);
rl.setPrompt('press any key to exit');
rl.prompt();
rl.on('line', (line) => rl.close())
    .on('close', () => process.exit(0));
