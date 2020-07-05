import { ConfigurationModel } from "./configModel";
export declare class UiOperationResult {
    IsSuccess: boolean;
    ResultName: string;
    ErrorMessage: string;
    ConfigurationModels: ConfigurationModel[];
    constructor(IsSuccess: boolean, ResultName: string, ErrorMessage: string, ConfigurationModels: ConfigurationModel[]);
    format(): string;
    static FromMsg(result: any): UiOperationResult;
}
