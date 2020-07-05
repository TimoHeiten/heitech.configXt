import { AuthModel, AppClaim } from "./authModel";
import { ContextType } from "./contextType";

export class ContextModel
{
    public user: AuthModel;
    public appClaims : AppClaim[];
    
    public key : string;
    public value : string;

    public type : ContextType;

    public appName : string;
    
    constructor() { }
}

export function createUserContext(user:string, password:string, entity:string, appName:string) : ContextModel{
    let model = new ContextModel();

    model.appName = appName;
    model.type = ContextType.AddUser;
    model.user = new AuthModel(user, password);
    model.appClaims = [ new AppClaim(appName, entity, true, true) ];

    return model;
}

export function getUserContext() : ContextModel{
    let model = new ContextModel();
    

    return model;
}

function deleteUserContext() : ContextModel{
    let model = new ContextModel();
    

    return model;
}

function updateUserContext() : ContextModel{
    let model = new ContextModel();
    

    return model;
}

// --------------------- context for config entities------------------------------------------------------

export function createEntityContext(key : string, value: string, user: AuthModel, appName: string) : ContextModel{
    let model = new ContextModel();
    model.type = ContextType.CreateEntry;
    model.key = key;
    model.value = value;
    model.user = user;
    model.appName = appName;

    return model;
}

export function getAllEntitiesContext(user: AuthModel, appName: string) : ContextModel{
    let model = new ContextModel();
    model.type = ContextType.ReadAllEntries;
    model.user = user;
    model.appName = appName;
    model.key = "must-not-be-null-but-also-not-important";

    return model;
}

export function getEntityContext(key : string, user: AuthModel, appName: string) : ContextModel{
    let model = new ContextModel();
    model.type = ContextType.ReadEntry;
    model.key = key;
    model.user = user;
    model.appName = appName;

    return model;
}

export function updateEntityContext(key : string, value: string, user: AuthModel, appName: string) : ContextModel{
    let model = new ContextModel();
    model.type = ContextType.UpdateEntry;
    model.key = key;
    model.value = value;
    model.user = user;
    model.appName = appName;

    return model;
}

export function uploadFile(user : AuthModel, appName: string) : ContextModel{
    let model = new ContextModel();
    // todo input for this instead of hardcoded 
    model.value = JSON.stringify({
            "Connections" : {
                "MySql" : "server=localhost;user=root;pwd=root;port=3307",
                "Sqlite" : "./my-db.db"
            },
            "RabbitMQ" : {
                "Host" : "localhost",
                "Port" : "5672",
                "User" : "guest",
                "Password" : "guest"
        }
    });
    model.key = "json";
    model.user = user;
    model.type = ContextType.UploadAFile;
    model.appName = appName;

    return model;
}
