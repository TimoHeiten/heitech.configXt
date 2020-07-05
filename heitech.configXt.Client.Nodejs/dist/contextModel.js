"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.uploadFile = exports.updateEntityContext = exports.getEntityContext = exports.getAllEntitiesContext = exports.createEntityContext = exports.getUserContext = exports.createUserContext = exports.ContextModel = void 0;
const authModel_1 = require("./authModel");
const contextType_1 = require("./contextType");
class ContextModel {
    constructor() { }
}
exports.ContextModel = ContextModel;
function createUserContext(user, password, entity, appName) {
    let model = new ContextModel();
    model.appName = appName;
    model.type = contextType_1.ContextType.AddUser;
    model.user = new authModel_1.AuthModel(user, password);
    model.appClaims = [new authModel_1.AppClaim(appName, entity, true, true)];
    return model;
}
exports.createUserContext = createUserContext;
function getUserContext() {
    let model = new ContextModel();
    return model;
}
exports.getUserContext = getUserContext;
function deleteUserContext() {
    let model = new ContextModel();
    return model;
}
function updateUserContext() {
    let model = new ContextModel();
    return model;
}
// --------------------- context for config entities------------------------------------------------------
function createEntityContext(key, value, user, appName) {
    let model = new ContextModel();
    model.type = contextType_1.ContextType.CreateEntry;
    model.key = key;
    model.value = value;
    model.user = user;
    model.appName = appName;
    return model;
}
exports.createEntityContext = createEntityContext;
function getAllEntitiesContext(user, appName) {
    let model = new ContextModel();
    model.type = contextType_1.ContextType.ReadAllEntries;
    model.user = user;
    model.appName = appName;
    model.key = "must-not-be-null-but-also-not-important";
    return model;
}
exports.getAllEntitiesContext = getAllEntitiesContext;
function getEntityContext(key, user, appName) {
    let model = new ContextModel();
    model.type = contextType_1.ContextType.ReadEntry;
    model.key = key;
    model.user = user;
    model.appName = appName;
    return model;
}
exports.getEntityContext = getEntityContext;
function updateEntityContext(key, value, user, appName) {
    let model = new ContextModel();
    model.type = contextType_1.ContextType.UpdateEntry;
    model.key = key;
    model.value = value;
    model.user = user;
    model.appName = appName;
    return model;
}
exports.updateEntityContext = updateEntityContext;
function uploadFile(user, appName) {
    let model = new ContextModel();
    // todo input for this instead of hardcoded 
    model.value = JSON.stringify({
        "Connections": {
            "MySql": "server=localhost;user=root;pwd=root;port=3307",
            "Sqlite": "./my-db.db"
        },
        "RabbitMQ": {
            "Host": "localhost",
            "Port": "5672",
            "User": "guest",
            "Password": "guest"
        }
    });
    model.key = "json";
    model.user = user;
    model.type = contextType_1.ContextType.UploadAFile;
    model.appName = appName;
    return model;
}
exports.uploadFile = uploadFile;
