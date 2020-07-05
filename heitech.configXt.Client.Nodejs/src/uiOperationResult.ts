import { ConfigurationModel } from "./configModel";

export class UiOperationResult
{
    constructor(public IsSuccess:boolean,
                public ResultName:string, 
                public ErrorMessage:string, 
                public ConfigurationModels:ConfigurationModel[])
    { }

    public format() : string {
        let models : string = this.ConfigurationModels.map(c => `${c.Name} --> ${c.Value}`).join("|")
        return `Success:[${this.IsSuccess}]\nResult:[${this.ResultName}]\nerror:[${this.ErrorMessage}]\nModels:[${models}]`;
    }

    static FromMsg(result : any) : UiOperationResult{
        return new UiOperationResult(
            result.IsSuccess as boolean,
            result.ResultName as string,
            result.ErrorMessage as string,
            result.ConfigurationModels as ConfigurationModel[],
        );
    }
}