
export class AuthModel
{
    constructor(public name : string, public password : string){

    }
}

export class AppClaim
{
    constructor(public AppName: string, public entityKey: string, public canRead:boolean, public canWrite:boolean)
    {

    }
}