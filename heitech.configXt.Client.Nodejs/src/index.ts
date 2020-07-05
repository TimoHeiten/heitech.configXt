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
import { ConfigurationModel } from './configModel';

const tcpConnect = "tcp://localhost:5557";

// todo: config file for admin, user and app name


function busSend(context : ContextModel){
    let socket = mq.socket('req');
    socket.connect(tcpConnect);

    socket.on('message', function(msg){
        let result = JSON.parse(msg.toString());
        let uiResult = UiOperationResult.FromMsg(result);
        console.log(uiResult.format());
        
        selectUseCase(context.appName, context.user);
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

async function run() : Promise<[string, AuthModel]>{
     console.clear();
     let user : AuthModel;
     let key : string;
     (await inq.prompt({
        type: "input",
        name : "add",
        message : "input User: [appName] [userName] [password]"
    }).then(answer => {
        let cmd : string = answer['add'];
        let _input = cmd.split(' ');
        user = new AuthModel(_input[1], _input[2]);
        key = _input[0];
        return user;
    }));

    return [key, user];
}

// todo create a class for each use case and then do input for each required values
function selectUseCase(appName: string, user: AuthModel) : void{
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

run().then(loggedInUser => selectUseCase(loggedInUser[0], loggedInUser[1]));
