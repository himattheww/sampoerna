import React from "react";
import { Redirect, Route } from "react-router-dom";

const PrivateRoute = (props: any) => {
  // const isAuth  = false

  const token = localStorage.getItem("auth");
  const role = localStorage.getItem("employeeRole");
  const pageAdmin = [
    "/admin",
    "/round",
    "/maintainprofile",
    "/uploaddata",
    "/roundsetting",
  ];
  const pageWholesaler = ["/wholeseller"];
  const pageSalesman = ["/leaderboards", "/salesman"];



  if (role === "A" && token && pageAdmin.includes(props.path)) {
    return <>{<Route {...props} />}</>;
  } else if (role === "W" && token && pageWholesaler.includes(props.path)) {
    return <>{<Route {...props} />}</>;
  } else if (role === "S" && token && pageSalesman.includes(props.path)) {
    return <>{<Route {...props} />}</>;
  } else {
    localStorage.clear()
    return <>{<Redirect to="/" />}</>;
  }
};

export default PrivateRoute;
