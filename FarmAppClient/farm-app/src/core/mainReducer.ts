import { Action, combineReducers } from "redux";
import { ThunkAction, ThunkDispatch } from "redux-thunk";
import authReducer,{ Auth } from "../store/auth/authState";


export const createMainReducer = () =>
  combineReducers({
    auth: authReducer,
  });

export interface IAppState{
  auth:Auth
}
  //extends ReturnType<ReturnType<typeof createMainReducer>> {}

export interface IAppDispatch extends ThunkDispatch<IAppState, Error, Action> {}

export interface IThunkAction
  extends ThunkAction<Promise<void>, IAppState, {}, Action> {}
