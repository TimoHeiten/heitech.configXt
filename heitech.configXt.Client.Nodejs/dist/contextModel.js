"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.createUserContext = exports.ContextModel = void 0;
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
function deleteUserContext() {
    let model = new ContextModel();
    return model;
}
function updateUserContext() {
    let model = new ContextModel();
    return model;
}
// --------------------- context for config entities------------------------------------------------------
function createEntityContext() {
    let model = new ContextModel();
    return model;
}
function getAllEntitiesContext() {
    let model = new ContextModel();
    return model;
}
function getEntityContext() {
    let model = new ContextModel();
    return model;
}
function updateEntityContext() {
    let model = new ContextModel();
    return model;
}
