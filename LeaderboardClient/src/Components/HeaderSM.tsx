import React, { FC } from "react";
import { RouteComponentProps, useHistory } from "react-router-dom";
import { useState } from "react";
import { Link } from "react-router-dom";
import { useEffect } from "react";

import "./header.css"
import "./leaderboard.css"
import mainlogo2 from "./mainlogo2.png"
import exit from "./exit.png"
import profile from "./profile.png"

type SomeComponentProps = RouteComponentProps;
const HeaderSM = () => {
    let hist = useHistory();
      const logout = () => {
        localStorage.clear();
        hist.push("/");
  };
  const [user, setUser] = useState({
    employeeCode: "",
    employeeName: "",
    roleName: ""
  });

  useEffect(() => {
    setUser({
      employeeCode: localStorage.getItem("employeeCode") || "",
      employeeName: localStorage.getItem("employeeName") || "",
      roleName: localStorage.getItem("roleName") || ""
    });
  }, []);

  return (
    <>
    <header className="bg-main d-flex flex-wrap align-items-center justify-content-center justify-content-md-between py-3 p-2 text-light fixed-top">
          <div className="col-3 text-start">
            <button type="button" className="btn btn-link logout-button" onClick={logout}>
                <img src={exit}
                    style={{
                    height: "20px"}} />
                <br />
                <label className="outLabelSM" style={{fontSize:"10px"}}>Keluar Aplikasi</label>
            </button>
          </div>

          <div className="user-roleName nav col-6 col-md-auto mt-2 justify-content-center mb-md-0">
            <img src={mainlogo2} className="m-2" 
            style={{
                height: "60px",
                zIndex: "1"}}
                 />
            <h3 className="role-admSM">{user.roleName}</h3>
          </div>

          <div className="col-3 text-end">
          </div>
    </header>
    <div className="bg-main pb-4 pt-2 distance-SM " style={{marginTop: "130px"}}>
        <div className="container">
            <div className="row">
                <div className="col-12" style={{whiteSpace: "nowrap", overflowX: "hidden"}}>
                <label className="text-light ms-2 username">Halo, <b>{user.employeeName}</b></label>
                {/* <button className="btn btn-warning float-end detail-information">Informasi Detail</button> */}
                </div>
        <div className="col-12 text-end">
        </div>
            </div>
        </div>
    </div>
    </>
  )
}

export default HeaderSM