import { AuthModel, AppClaim } from "./authModel";
import { ContextType } from "./contextType";
export declare class ContextModel {
    user: AuthModel;
    appClaims: AppClaim[];
    key: string;
    value: string;
    type: ContextType;
    appName: string;
    constructor();
}
export declare function createUserContext(user: string, password: string, entity: string, appName: string): ContextModel;
export declare function getAllEntitiesContext(user: AuthModel, appName: string): ContextModel;
export declare function getEntityContext(key: string, user: AuthModel, appName: string): ContextModel;
export declare function uploadFile(user: AuthModel, appName: string): ContextModel;
