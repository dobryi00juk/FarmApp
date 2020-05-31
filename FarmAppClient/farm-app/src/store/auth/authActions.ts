import { actionCreatorFactory } from "typescript-fsa";
import { ILoginParams, IAccount } from "../../api/dto/Auth";
import {authRequest} from "../../api/BaseFetch";
import {Dispatch} from "redux";
import {IAppState} from "../../core/mainReducer";
import {User} from "./authState"


const authActionCreator = actionCreatorFactory("auth");

export const login = authActionCreator.async<
  ILoginParams,
  User | null,
  Error
>("LOGIN");


export const logout = authActionCreator("LOGOUT");

export const authActions = {
  login,
  logout,
};


export const RESTORE_AUTH = 'RESTORE_AUTH'
export const restoreAuth=(result:object|null)=>async (dispatch:any) =>{
  dispatch({
    type:RESTORE_AUTH,
    payload:result
  })
}  