import {BASE_URL} from "../core/constants";
var qs = require('qs');

const axios = require('axios');
export type IHttpMethods = "GET" | "POST" | "PUT" | "DELETE";

interface IResponse<R> {
  result: R | null;
  status: number;
  error: Error | null;
  message: string | null;
}

export const baseFetch = async <P, R>(
  url: string,
  params: P,
  method: IHttpMethods = "GET",
  token: string,
  headers: { [key: string]: string } = {}
): Promise<IResponse<R>> => {
  try {
    const bodyObj = method !== "GET" ? { body: JSON.stringify(params) } : {};
    const res = await fetch(`/api/${url}`, {
      method,
      ...bodyObj,
      headers: {
        Accept: "application/json, text/plain",
        "Content-Type": "application/json;charset=UTF-8",
        Authorization: `Bearer ${token}`,
        ...headers,
      },
    });
    if (res.status === 401) {
      return {
        error: null,
        message: res.statusText,
        result: null,
        status: 401,
      };
    }
    return {
      error: null,
      status: res.status,
      message: res.statusText,
      result: await res.json(),
    };
  } catch (error) {
    return {
      message: error.message,
      result: null,
      status: 401,
      error: error as Error,
    };
  }
};


export const authRequest = async (login:string, password:string) => {
  localStorage.setItem('login', login)
  try {
    let data = {
      username: login,
      password: password,
    }
    let response = await axios.post(`${BASE_URL}/das/login`, qs.stringify(data), {
      headers: {
        'Access-Control-Allow-Headers': 'X-Merp-Session-Id, Access-Control-Allow-Origin',
        'Access-Control-Expose-Headers': 'X-Merp-Session-Id, Access-Control-Allow-Origin',
        'Content-Type': 'application/x-www-form-urlencoded;; charset=utf-8',
        'Access-Control-Allow-Origin': '*',
      },
    })
    //console.log('api response data', response)
    return response
  } catch (error) {
    console.log('error', error)
  }
}
