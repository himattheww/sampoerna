import React from "react";
import { Redirect, Route } from "react-router-dom";

const RestrictedRoute = (props: any) => {
  // const isAuth  = false

  const token = localStorage.getItem("auth");
  if (!token) {
    return <>{<Route {...props} />}</>;

  } else {
    localStorage.clear();
    return <>{<Redirect to="/" />}</>;
  }


};

export default RestrictedRoute;
