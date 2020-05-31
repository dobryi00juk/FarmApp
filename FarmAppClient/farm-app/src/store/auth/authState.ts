import { reducerWithInitialState } from "typescript-fsa-reducers";
import { login, logout, RESTORE_AUTH } from "./authActions";
import { IAccount } from "../../api/dto/Auth";
import {Action} from 'redux';
import { deflate } from "zlib";

export interface ActionWithPayload<T> extends Action {
    payload: T;
}

export interface User{
    id:number
    firstName:string
    lastName:string
    userName:string
    token:string
    role:{
      id:number
      roleName:string
    }
  
}
export interface Auth {
  loadState: boolean;
  error: Error | null;
  account: IAccount | null;
   user: User|null
}

const initial: Auth = {
  account: null,
  error: null,
  loadState: false,
  user:null
};
export default  (state:Auth = initial, action:ActionWithPayload<any>) => {
	switch (action.type) {
    case "auth/LOGIN_STARTED":
      return {
        ...state,
        loadState: true,
        error: null,
      }
    case "auth/LOGIN_DONE":
      return {
      ...state,
      user: action.payload.result,
      loadState: false,
      error: null,
    }

    case "auth/LOGIN_FAILED":
       return {
      ...state,
      loadState: false,
      error:action.payload.error,
    }

    case RESTORE_AUTH:
       return {
        ...state,
        user: action.payload,
        loadState: false,
        error: null,
      }

     case "auth/LOGOUT":
       return initial
     default:
            return state

  }
}

