import { actionCreatorFactory } from "typescript-fsa";
import { ILoginParams, IAccount } from "../../api/dto/Auth";
import {dispatch} from "../../index";
import {authRequest} from "../../api/BaseFetch";
import {Dispatch} from "redux";
import {IAppState} from "../../core/mainReducer";


const authActionCreator = actionCreatorFactory("auth");

export const login = authActionCreator.async<
  ILoginParams,
  IAccount | null,
  Error
>("LOGIN");
export const logout = authActionCreator("LOGOUT");

export const authActions = {
  login,
  logout,
};

export const START_LOG_IN = 'START_LOG_IN'
export const AUTH_SUCCESS = 'AUTH_SUCCESS'
export const AUTH_ERROR = 'AUTH_ERROR'

export const authLogin = (email:string,pass:string) => async (dispatch:Dispatch, getState:IAppState) =>  {

  debugger
  try {
    const response = await authRequest(email,pass)
    const status =  response.status;
    if (status === 200) {
      dispatch({type: login.done,payload: response.data})
    }
    else {
      dispatch({type:login.failed, payload: `error Login with ${status}`})
    }
  } catch (e) {
    dispatch({type:login.failed, payload: 'error Login'})
  }
}


// const logInWithLoagInAndPassword = (login, password, organization, rememberMe) => async (dispatch, getState) => {
//   console.log('logInWithLoagInAndPassword', login, password, organization, rememberMe)
//
//   let response = await api.logInWithLoagInAndPasswordAsync(login, password, organization, rememberMe)
//   if (response.isSuccess) {
//     let payload = response.data
//
//     let custom_fields = payload && payload.user && payload.user.entity_types
//     custom_fields &&
//     Object.values(custom_fields).map(el => {
//       console.log('custom_fields ', el.feature_name, custom_fields)
//       switch (el.feature_name) {
//         case 'person': {
//           return dispatch({ type: 'SET_PERSON_CUSTOM_FIELDS', payload: el.custom_fields })
//         }
//         case 'group': {
//           return dispatch({ type: 'SET_GROUP_CUSTOM_FIELDS', payload: el.custom_fields })
//         }
//         case 'location': {
//           return dispatch({ type: 'SET_LOCATION_CUSTOM_FIELDS', payload: el.custom_fields })
//         }
//         case 'project': {
//           return dispatch({ type: 'SET_PROJECT_CUSTOM_FIELDS', payload: el.custom_fields })
//         }
//         case 'case': {
//           return dispatch({ type: 'SET_CASE_CUSTOM_FIELDS', payload: el.custom_fields })
//         }
//         case 'contact': {
//           return dispatch({ type: 'SET_CONTACT_CUSTOM_FIELDS', payload: el.custom_fields })
//         }
//         case 'organization': {
//           return dispatch({ type: 'SET_ORGANIZATION_CUSTOM_FIELDS', payload: el.custom_fields })
//         }
//         default:
//           break
//       }
//     })
//
//     let features = oc(payload).user.tenant.features(null)
//     console.log(' payloadv', payload, features)
//     if (features) {
//       await dispatch({ type: SET_FEATURES, payload: features })
//     }
//     await dispatch({ type: AUTH_SUCCESS, payload: payload })
//   } else {
//     console.log('error', response)
//     if (response.error && !response.error.response) {
//       await dispatch({ type: SET_SNACKBAR_OPEN, payload: { openSnackbar: true, massage: 'Geen netwerkverbinding' } })
//     }
//     await dispatch({ type: AUTH_ERROR, payload: { error: response.error, errorText: response.errorText } })
//   }
//   //{"username":"demotest","password":"demotest123","customer":"sportenwelzijn","remember":"yes","login":"Inloggen"}
//   //https://dev.dasystems.nl/das/login?username=demotest&password=demotest123&customer=sportenwelzijn&remember=yes&login=Inloggen
// }
