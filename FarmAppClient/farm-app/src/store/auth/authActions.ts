import {actionCreatorFactory} from "typescript-fsa";
import {ILoginParams, IAccount} from "../../api/dto/Auth";
import {authRequest} from "../../api/BaseFetch";
import {Dispatch} from "redux";
import {IAppState} from "../../core/mainReducer";
import {User} from "./authState"
import {GET_REGION_ERROR, GET_REGION_REQUEST, GET_REGION_RESPONCE} from "../region/regionActions";
import {regions} from "../../core/api";
import {dispatch} from "../../index";


const authActionCreator = actionCreatorFactory("auth");

export const login = authActionCreator.async<ILoginParams,
    User | null,
    Error>("LOGIN");


export const logout = authActionCreator("LOGOUT");

export const authActions = {
    login,
    logout,
};

// export const LOGIN_STARTED = 'auth/LOGIN_STARTED'
// export const LOGIN_DONE = 'auth/LOGIN_DONE'
// export const LOGIN_FAILED = 'auth/LOGIN_FAILED'
//
// export const auth = (login: string,password: string) => async (dispatch: Dispatch<any>, getState: () => IAppState): Promise<void> => {
//     dispatch({type: LOGIN_STARTED})
//     try {
//         const token = getState().auth?.user?.token
//         if (token) {
//             const response = await authRequest(login,password)
//             const responseJson = await response.json();
//             if (response.ok) {
//                 dispatch({
//                     type: LOGIN_DONE,
//                     payload: responseJson
//                 })
//             } else {
//                 dispatch({
//                     type: LOGIN_FAILED,
//                     payload: responseJson
//                 })
//             }
//         }
//     } catch (error) {
//         dispatch({
//             type: LOGIN_FAILED,
//             payload: error
//         })
//     }
// }


export const RESTORE_AUTH = 'RESTORE_AUTH'
export const restoreAuth = (result: object | null) => async (dispatch: Dispatch<any>) => {
    dispatch({
        type: RESTORE_AUTH,
        payload: result
    })
}


