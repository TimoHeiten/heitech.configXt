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

function getUserContext() : ContextModel{
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

function createEntityContext() : ContextModel{
    let model = new ContextModel();


    return model;
}

function getAllEntitiesContext() : ContextModel{
    let model = new ContextModel();
    

    return model;
}

function getEntityContext() : ContextModel{
    let model = new ContextModel();
    

    return model;
}

function updateEntityContext() : ContextModel{
    let model = new ContextModel();
    

    return model;
}
