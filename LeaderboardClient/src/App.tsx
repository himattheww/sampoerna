import React, { useState } from "react";
import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap/dist/js/bootstrap.bundle.min.js";
import Login from "./Components/Login";
import ForgotPassword from "./Components/ForgotPassword";
import FirstLogin from "./Components/FirstLogin";
import Salesman from "./Components/Salesman";
import ResetPassword from "./Components/ResetPassword";
import RestrictedRoute from "./Auth/RestrictedRoute";
import PrivateRoute from "./Auth/PrivateRoute";
import { BrowserRouter, Route, Switch } from "react-router-dom";
import Admin from "./Components/Admin";
import Round from "./Components/Round";
import MaintainProfile from "./Components/Maintainprofile";
import UploadData from "./Components/UploadData";
import RoundSetting from "./Components/RoundSetting";
import WholesellerPage from "./Components/WholesellerPage";
import SalesmanLeaderboard from "./Components/SalesmanLeaderboard";
import HeaderNew from "./Components/HeaderNew";
import PhoneNumber from "./Components/PhoneNumber";
import "../src/App.css";


function App() {
  return (
    <BrowserRouter>
      <Switch>
        <RestrictedRoute exact path="/" component={Login} />
        <Route exact path="/forgotpassword" component={ForgotPassword} />
        <Route exact path="/resetpassword" component={ResetPassword} />
        <Route exact path="/firstlogin" component={FirstLogin} />
        <PrivateRoute exact path="/salesman" component={Salesman} />
        <PrivateRoute exact path="/admin" component={Admin} />
        <PrivateRoute exact path="/round" component={Round} />
        <PrivateRoute exact path="/maintainprofile" component={MaintainProfile} />
        <PrivateRoute exact path="/uploaddata" component={UploadData} />
        <PrivateRoute exact path="/wholeseller" component={WholesellerPage} />
        <PrivateRoute exact path="/leaderboards" component={SalesmanLeaderboard} />
        <PrivateRoute exact path="/roundsetting" component={RoundSetting} />
        <Route exact path="/updatephonenumber" component={PhoneNumber} />
      </Switch>
    </BrowserRouter>
  );
}

export default App;
