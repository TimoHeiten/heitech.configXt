import * as inq from 'inquirer';
import { ContextModel,} from "./contextModel";
import { AuthModel } from './authModel';
import { Commands } from './commands';
import { updateEntry, promptGetEntry, promptGetAll, upload } from './entityCrud';
import { readFileSync } from 'fs';

const credentials : any = JSON.parse(readFileSync("./src/config.json", 'utf8'));
// todo: config file for admin, user and app name
// main to run the loop
async function main() : Promise<[string, AuthModel]>{
     console.clear();
     let user : AuthModel;
     let key : string;
     (await inq.prompt({
        type: "input",
        name : "add",
        message : "input User: [appName] [userName] [password]"
    }).then(answer => {
        let cmd : string = answer['add'];
        if (cmd === '')
        {
            key = credentials.appName;
            user = new AuthModel(<string> credentials.name, <string> credentials.password);
        }
        else{
            let _input = cmd.split(' ');
            user = new AuthModel(_input[1], _input[2]);
            key = _input[0];
        }
    }));

    return [key, user];
}

export function selectUseCase(appName: string, user: AuthModel) : void{
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
                        promptGetEntry(appName, user);
                        break;
                    case Commands.GetAllEntries:
                        promptGetAll(appName, user);
                        break;
                    case Commands.UpdateEntry:
                        updateEntry(appName, user);
                        break;
                    case Commands.Upload:
                        upload(appName, user);
                        break;
                    case Commands.Login:
                        main();
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

main().then(logInResult => {
    let appName = logInResult[0];
    let user = logInResult[1];
    selectUseCase(appName, user);
});
