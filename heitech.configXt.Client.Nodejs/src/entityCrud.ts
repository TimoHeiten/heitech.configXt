import * as inq from 'inquirer';
import { 
    ContextModel,
    createUserContext, 
    getEntityContext,
    getAllEntitiesContext, 
    uploadFile, 
    updateEntityContext
} from "./contextModel";
import { busSend } from "./utils";
import { selectUseCase } from "./index";
import { AuthModel } from './authModel';

export function promptGetEntry(appName : string, user : AuthModel) : void {
    inq.prompt({
        type: "input",
        name: "configkey",
        message : "enter [configkey] to read"
    }).then(answer => {
        let key : string = answer["configkey"];
        let context = getEntityContext(key, user, appName);
        busSend(context, selectUseCase);
    });
}

export function promptGetAll(appName : string, user : AuthModel) : void{
    let context = getAllEntitiesContext(user, appName);
    busSend(context, selectUseCase);
}

export function upload(appName : string, user : AuthModel) : void {
    let context = uploadFile(user, appName);
    busSend(context, selectUseCase);
}

export function updateEntry(appName : string, user : AuthModel) : void {
    let key : string = "";
    let val : string = "";
    // inquire the key for the entity to be updated
    inq.prompt({
        type: "input",
        name: "configkey",
        message : "enter [configkey] to update"
    }).then(answer => {
            key = answer["configkey"];
            // inquire value to update to
            inq.prompt({
                type: "input",
                name: "configvalue",
                message : "enter [config-value] to update"
                }
            ).then(answer => {
                val = answer["configvalue"];
                let context = updateEntityContext(key, val, user, appName);
                busSend(context, selectUseCase);
            })
        }
    );
}