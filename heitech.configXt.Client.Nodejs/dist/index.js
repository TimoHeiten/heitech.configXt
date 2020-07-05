"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.selectUseCase = void 0;
const inq = require("inquirer");
const authModel_1 = require("./authModel");
const commands_1 = require("./commands");
const entityCrud_1 = require("./entityCrud");
const fs_1 = require("fs");
const credentials = JSON.parse(fs_1.readFileSync("./src/config.json", 'utf8'));
// todo: config file for admin, user and app name
// main to run the loop
async function main() {
    console.clear();
    let user;
    let key;
    (await inq.prompt({
        type: "input",
        name: "add",
        message: "input User: [appName] [userName] [password]"
    }).then(answer => {
        let cmd = answer['add'];
        if (cmd === '') {
            key = credentials.appName;
            user = new authModel_1.AuthModel(credentials.name, credentials.password);
        }
        else {
            let _input = cmd.split(' ');
            user = new authModel_1.AuthModel(_input[1], _input[2]);
            key = _input[0];
        }
    }));
    return [key, user];
}
function selectUseCase(appName, user) {
    inq.prompt({
        type: "list",
        name: "command",
        message: "choose option",
        choices: Object.values(commands_1.Commands)
    }).then(async (answer) => {
        let context;
        let cmd = answer["command"];
        if (cmd !== commands_1.Commands.Quit) {
            switch (cmd) {
                case commands_1.Commands.GetEntry:
                    entityCrud_1.promptGetEntry(appName, user);
                    break;
                case commands_1.Commands.GetAllEntries:
                    entityCrud_1.promptGetAll(appName, user);
                    break;
                case commands_1.Commands.UpdateEntry:
                    entityCrud_1.updateEntry(appName, user);
                    break;
                case commands_1.Commands.Upload:
                    entityCrud_1.upload(appName, user);
                    break;
                case commands_1.Commands.Login:
                    main();
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
exports.selectUseCase = selectUseCase;
main().then(logInResult => {
    let appName = logInResult[0];
    let user = logInResult[1];
    selectUseCase(appName, user);
});
