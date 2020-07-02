"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.ContextType = void 0;
var ContextType;
(function (ContextType) {
    ContextType[ContextType["CreateEntry"] = 0] = "CreateEntry";
    ContextType[ContextType["ReadEntry"] = 1] = "ReadEntry";
    ContextType[ContextType["UpdateEntry"] = 2] = "UpdateEntry";
    ContextType[ContextType["DeleteEntry"] = 3] = "DeleteEntry";
    ContextType[ContextType["ReadAllEntries"] = 4] = "ReadAllEntries";
    ContextType[ContextType["UploadAFile"] = 5] = "UploadAFile";
    ContextType[ContextType["DownloadAsFile"] = 6] = "DownloadAsFile";
    ContextType[ContextType["AddUser"] = 7] = "AddUser";
    ContextType[ContextType["GetUser"] = 8] = "GetUser";
    ContextType[ContextType["DeleteUser"] = 9] = "DeleteUser";
    ContextType[ContextType["UpdateUser"] = 10] = "UpdateUser";
})(ContextType = exports.ContextType || (exports.ContextType = {}));
