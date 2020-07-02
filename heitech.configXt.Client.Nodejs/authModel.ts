
export class AuthModel
{
    constructor(public name : string, public password : string){

    }

    toString() : string{
        return `${this.name}-${this.password}`;
    }
}

export class AppClaim
{
    constructor(public AppName: string, public entityKey: string, public canRead:boolean, public canWrite:boolean)
    {

    }
}