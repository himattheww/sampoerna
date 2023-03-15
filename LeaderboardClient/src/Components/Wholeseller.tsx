import React, { FC } from "react";
import { RouteComponentProps } from "react-router-dom";
import axios from "axios";
import { useState } from "react";
import { useEffect } from "react";

import "./home.css";
import exit from "./exit.svg";
import mainlogo from "./mainlogo.png";
import person from "./person.svg";
import Header from "./Header";

type SomeComponentProps = RouteComponentProps;
const Wholeseller: FC<SomeComponentProps> = ({ history }) => {
  const logout = () => {
    localStorage.clear();
    history.push("/");
  };

 


  const [employeeName, setEmployeeName] = useState("");
  const [employeeRole, setEmployeeRole] = useState("");
  const [roleName, setRoleName] = useState("");
  const [token, setToken] = useState("");

  const getUser = () => {
    const user = {
     employeeCode: localStorage.getItem("employeeCode"),
     employeeName: localStorage.getItem("employeeName"),
     employeeRole: localStorage.getItem("employeeRole"),
     roleName: localStorage.getItem("roleName"), 
    };

    const USER_TOKEN = localStorage.getItem("auth")
    const AuthStr = 'Bearer ' + USER_TOKEN;
    axios.get('https://localhost:7235/api/Employees/main?customerCode=' + user.employeeCode, {headers: {Authorization: AuthStr,"Access-Control-Allow-Origin":"*"}})
    .then((res) => {
      setEmployeeName(res.data.data.employeeName);
      setEmployeeRole(res.data.data.employeeRole);
      setRoleName(res.data.data.roleName);
      // const currentSession = localStorage.getItem("auth");
      // if(currentSession != null){
      //   setToken(currentSession.toString());
      // }
    })
  }
  useEffect(() => {
    getUser(); 
  }, []);  

  return (
    <>
    <Header />
        
        <div className="row d-flex justify-content-center align-items-center header-scale">
          <div className="align-self-end card form-radius main-user">
            <div className="col-md-12">
              <div className="card-body">
                <div className="card-title text-left page-role"> 
                Wholeseller Page </div>
              </div>
            </div>
          </div>
        </div>
    </>     
  );
};

export default Wholeseller;