"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const inq = require("inquirer");
const mq = require("zeromq");
const contextModel_1 = require("./contextModel");
const authModel_1 = require("./authModel");
const tcpConnect = "tcp://localhost:5557";
function busSend(context) {
    let socket = mq.socket('req');
    socket.connect(tcpConnect);
    socket.on('message', function (msg) {
        console.log('got' + msg.toString());
        selectUseCase(context.user);
    });
    socket.send(JSON.stringify(context));
}
var Commands;
(function (Commands) {
    Commands["Login"] = "Login-As";
    Commands["Quit"] = "Quit";
    Commands["GetEntry"] = "GetEntry";
    Commands["UpdateEntry"] = "UpdateEntry";
    Commands["CreateEntry"] = "CreateEntry";
    Commands["GetAllEntries"] = "GetAllEntries";
    Commands["AddUser"] = "AddUser";
    Commands["GetUser"] = "GetUser";
    Commands["UpdateUser"] = "UpdateUser";
    Commands["DeleteUser"] = "DeleteUser";
    Commands["Upload"] = "upload-data";
    Commands["Download"] = "download-data";
})(Commands || (Commands = {}));
async function run() {
    console.clear();
    let user;
    (await inq.prompt({
        type: "input",
        name: "add",
        message: "input User: [userName] [password]"
    }).then(answer => {
        let cmd = answer['add'];
        let _input = cmd.split(' ');
        user = new authModel_1.AuthModel(_input[0], _input[1]);
        return user;
    }));
    return user;
}
// todo hard coded const appName (extend authmodel for appname)
const appName = "test-app-1";
// todo create a class for each use case and then do input for each required values
function selectUseCase(user) {
    inq.prompt({
        type: "list",
        name: "command",
        message: "choose option",
        choices: Object.values(Commands)
    }).then(async (answer) => {
        let context;
        let cmd = answer["command"];
        if (cmd !== Commands.Quit) {
            switch (cmd) {
                case Commands.GetEntry:
                    inq.prompt({
                        type: "input",
                        name: "configkey",
                        message: "enter [configkey] to read"
                    }).then(answer => {
                        let key = answer["configkey"];
                        context = contextModel_1.getEntityContext(key, user, appName);
                        busSend(context);
                    });
                    break;
                case Commands.GetAllEntries:
                    context = contextModel_1.getAllEntitiesContext(user, appName);
                    busSend(context);
                    break;
                case Commands.Upload:
                    context = contextModel_1.uploadFile(user, appName);
                    busSend(context);
                    break;
                case Commands.Login:
                    run();
                    break;
                default:
                    break;
            }
        }
        else {
            console.log("done! - bye bye");
            process.exit(0);
        }
    });
}
run().then(authModel => selectUseCase(authModel));
