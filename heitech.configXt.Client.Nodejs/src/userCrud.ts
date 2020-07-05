import * as inq from 'inquirer';
import { createUserContext,  } from "./contextModel";
import { busSend } from "./utils";
import { selectUseCase } from "./index";
import { AuthModel } from './authModel';

export function addUser(appName : string, user : AuthModel) : void{
    // prompt user name and pw
    // bus send
}

export function getUser(appName : string, user : AuthModel) : void{
    // prompt user name 
}

export function updateUser(appName : string, user : AuthModel) : void{
    // prompt user name and new pw as well as appclaims
}