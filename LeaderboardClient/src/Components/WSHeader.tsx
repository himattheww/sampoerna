import React, { FC } from "react";
import { RouteComponentProps, useHistory } from "react-router-dom";
import { useState } from "react";
import { Link } from "react-router-dom";
import { useEffect } from "react";

import "./header.css"
import mainlogo2 from "./mainlogo2.png"
import exit from "./exit.png"
import profile from "./profile.png"
import { Box } from "@mui/material";
import DetailBox from "./DetailBox";
import Popup from "./Popup";

type SomeComponentProps = RouteComponentProps;



const WSHeader = (props :any) => {

  const [user, setUser] = useState({
    employeeCode: "",
    employeeName: "",
    roleName: "",
    groupName: "zzzzz",
  });

  

    let hist = useHistory();
      const logout = () => {
        localStorage.clear();
        hist.push("/");
  };
 


  const detailInformation = ( currentWs : string, currentDetails : string ) => {
    localStorage.setItem("wholesellerCode", currentWs)
    localStorage.setItem("detail" , currentDetails)
    hist.push("/information")
  }


  // toggle data starts here//
const [show, setShow]=useState(false)
const [modalShow, setModalShow] = useState(false);
// toggle data ends here

  useEffect(() => {
    setUser({
      employeeCode: localStorage.getItem("employeeCode") || "",
      employeeName: localStorage.getItem("employeeName") || "",
      roleName: localStorage.getItem("roleName") || "",
      groupName : localStorage.getItem("groupName") || "ssss",
    });
  }, []);
  //  test push
  return (
    <>
    <header className="bg-main d-flex flex-wrap align-items-center justify-content-center justify-content-md-between py-3 p-2 text-light fixed-top">
          <div className="col-3 text-start">
            <button type="button" className="btn btn-link logout-button" onClick={logout}>
                <img src={exit}
                    style={{
                    height: "20px"}} />
                    <br />
                <label className="outLabelWS" style={{fontSize:"10px"}}>Keluar Aplikasi</label>
            </button>
          </div>

          <div className="nav col-6 col-md-auto mt-2 justify-content-center mb-md-0">
            <img src={mainlogo2} className="m-2 WsLogo" 
            style={{
                height: "60px",
                zIndex: "1",
              }}
                 />
            {/* <h1 style={{paddingRight: "20px"}}>{user.roleName}</h1> */}
          </div>
          {/* ? */}
          <div className="nav col-6 col-md-auto mt-2 justify-content-center mb-md-0" >
          <h1 className="user-roleNameWS" style={{marginTop:"10px", marginLeft:"-400px", paddingTop: "60px", paddingLeft:"20px"}}>{user.roleName} Group ({user.groupName})</h1>
          </div>
    </header>
    <div className="bg-main distance-WS" style={{marginTop: "105px"}}>
        <div className="container">
            <div className="row">
                <div className="col-12 mb-2" style={{whiteSpace: "nowrap", overflowX: "hidden"}}>
                {/* <img className="prf" src={profile} style={{height: "34px", width: "34px", marginTop: "-5px"}} /> */}
                <label className="text-light ms-2 username-WS">Halo, <b>{user.employeeName}</b></label>
                <button onClick={()=>setModalShow(!modalShow)} className="btn btn-warning float-end detail-information">Informasi Detail</button>
                {/* detail box */}
                {/* <button onClick={()=>setShow(!show)} className="btn btn-warning float-end detail-information">Informasi Detail</button> */}
                
               {modalShow? <Popup show={modalShow} onHide={() => setModalShow(false)} /> : null}


                {/* click information detail start here old ones*/}
                {/* {show? <DetailBox/> : null} */}
                {/* click information detail end here old ones */}
                </div>
        <div className="col-12 text-end">
        </div>
            </div>
        </div>
    </div>
    </>
  )
}

export default WSHeader