import React, {useEffect} from "react";
import "./App.css";
import SignIn from "./modules/auth/SignIn";
import SignUp from "./modules/auth/SignUp";
import  ResponsiveDrawer  from "./modules/menu/ResponsiveDrawer";
import { BrowserRouter, Switch, Route, Redirect } from "react-router-dom";
import {restoreAuth} from "./store/auth/authActions";
import {useDispatch} from "react-redux";

const NotFoundRedirect = () => <Redirect to='/farm-app/auth/' />

const App = () => {
  const dispatch = useDispatch();
  useEffect(() => {
    const rememberMe = localStorage.getItem('auth')
    if (rememberMe !== null){
      const response = JSON.parse(rememberMe)
      dispatch(restoreAuth(response))
    }
  },[])

  return (
    <BrowserRouter>
      <Switch>
        <Route exact path="/farm-app/auth/">
          <SignIn />
        </Route>
        <Route exact path="/farm-app/reg/">
          <SignUp />
        </Route>
        <Route path="/farm-app/">
          <ResponsiveDrawer />
      </Route>
         <Route component={NotFoundRedirect} />
      </Switch>
    </BrowserRouter>
  );
}

export default App;
