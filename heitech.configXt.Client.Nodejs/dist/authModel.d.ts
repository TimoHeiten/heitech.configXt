export declare class AuthModel {
    name: string;
    password: string;
    constructor(name: string, password: string);
    toString(): string;
}
export declare class AppClaim {
    AppName: string;
    entityKey: string;
    canRead: boolean;
    canWrite: boolean;
    constructor(AppName: string, entityKey: string, canRead: boolean, canWrite: boolean);
}
