import { ConfigurationModel } from "./configModel";

export class UiOperationResult
{
    constructor(public isSuccess:boolean,
                public resultName:string, 
                public errorMessage:string, 
                public configurationModels:ConfigurationModel[])
    { }
}