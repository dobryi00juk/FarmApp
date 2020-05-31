import { ILoginParams } from "../../api/dto/Auth";
import { callApi } from "../apiActionsAsync";
import { authActions } from "./authActions";
import { BASE_URL } from "../../core/constants";
import { Dispatch } from "react";

export const callApiLogin = (params: ILoginParams, goToFrom: (result?:object|null) => void) =>
  callApi(
    params,
    { url:`api/Users/authenticate` , method: "POST", usePublicToken: true,headers:{
      "content-type":"application/json",
        'Access-Control-Allow-Origin': '*',
    } },
    authActions.login,
    (result?:object|null)=>goToFrom(result)
  );

