import * as inq from 'inquirer';
import * as mq from 'zeromq';
import { 
    ContextModel,
    createUserContext, 
    getEntityContext,
    getAllEntitiesContext, 
    uploadFile 
} from "./contextModel";
import { UiOperationResult } from "./uiOperationResult";
import { AuthModel } from './authModel';

const tcpConnect = "tcp://localhost:5557";

function busSend(context : ContextModel){
    let socket = mq.socket('req');
    socket.connect(tcpConnect);

    socket.on('message', function(msg){
        console.log('got' + msg.toString());
        
        selectUseCase(context.user);
    });

    socket.send(JSON.stringify(context));
}

enum Commands{
    Login = 'Login-As',
    Quit = "Quit",

    GetEntry = "GetEntry",
    UpdateEntry = "UpdateEntry",
    CreateEntry = "CreateEntry",
    GetAllEntries = "GetAllEntries",

    AddUser = "AddUser",
    GetUser = "GetUser",
    UpdateUser = "UpdateUser",
    DeleteUser = "DeleteUser",

    Upload = "upload-data",
    Download = "download-data"
}

async function run() : Promise<AuthModel>{
     console.clear();
     let user : AuthModel;
     (await inq.prompt({
        type: "input",
        name : "add",
        message : "input User: [userName] [password]"
    }).then(answer => {
        let cmd : string = answer['add'];
        let _input = cmd.split(' ');
        user = new AuthModel(_input[0], _input[1]);
        return user;
    }));

    return user;
}

// todo hard coded const appName (extend authmodel for appname)
const appName : string = "test-app-1";

// todo create a class for each use case and then do input for each required values
function selectUseCase(user: AuthModel) : void{
        inq.prompt({
            type: "list",
            name: "command",
            message: "choose option",
            choices : Object.values(Commands)
        }).then(async answer => {
            let context : ContextModel;
            let cmd = answer["command"];
            if (cmd !== Commands.Quit)
            {
                switch (cmd) {
                    case Commands.GetEntry:
                        inq.prompt({
                            type: "input",
                            name: "configkey",
                            message : "enter [configkey] to read"
                        }).then(answer => {
                            let key : string = answer["configkey"];
                            context = getEntityContext(key, user, appName);
                            busSend(context);
                        });
                        break;
                    case Commands.GetAllEntries:
                        context = getAllEntitiesContext(user, appName);
                        busSend(context);
                        break;
                    case Commands.Upload:
                        context = uploadFile(user, appName);
                        busSend(context);
                        break;
                    case Commands.Login:
                        run();
                        break;
                    default:
                        break;
                }
            }
            else 
            {
                console.log("done! - bye bye");
                process.exit(0);
            }
        });
}

run().then(authModel => selectUseCase(authModel));
