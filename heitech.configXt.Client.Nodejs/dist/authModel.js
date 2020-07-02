"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.AppClaim = exports.AuthModel = void 0;
class AuthModel {
    constructor(name, password) {
        this.name = name;
        this.password = password;
    }
    toString() {
        return `${this.name}-${this.password}`;
    }
}
exports.AuthModel = AuthModel;
class AppClaim {
    constructor(AppName, entityKey, canRead, canWrite) {
        this.AppName = AppName;
        this.entityKey = entityKey;
        this.canRead = canRead;
        this.canWrite = canWrite;
    }
}
exports.AppClaim = AppClaim;
