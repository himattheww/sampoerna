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
import { Box } from "@mui/material";
import DetailBox from "./DetailBox";
import Popup from "./Popup";
import PopupSalesman from "./PopupSalesman";

type SomeComponentProps = RouteComponentProps;



const HeaderSalesman = (props :any) => {
  const [user, setUser] = useState({
    employeeCode: "",
    employeeName: "",
    roleName: "",
    groupName: ""
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
      employeeCode: "",
      // employeeName: localStorage.getItem("nameSalesman") || "",
      employeeName : localStorage.getItem("employeeName") || "",
      roleName: "",
      groupName: localStorage.getItem("groupName") || ""
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
                <label className="outLabelSML" style={{fontSize:"10px"}}>Keluar Aplikasi</label>
            </button>
          </div>

          <div className="nav col-6 col-md-auto mt-2 justify-content-center mb-md-0">
            <img src={mainlogo2} className="m-2" 
            style={{
                height: "60px",
                zIndex: "1",
              }}
                 />
            {/* <h1 style={{paddingRight: "20px"}}>{user.roleName}</h1> */}
          </div>
          {/* ? */}
          <div className="nav col-6 col-md-auto mt-2 justify-content-center mb-md-0" >
          <h1 className="user-roleNameSM" style={{marginTop:"10px", marginLeft:"-440px", paddingTop: "60px", paddingLeft:"20px"}}>{user.roleName} Group ({props.groupName}) </h1>
          </div>
          {/* ? */}
          <div >
            
          </div>

          <div className="col-3 text-end">
          </div>
    </header>
    <div className="bg-main" style={{marginTop: "105px"}}>
        <div className="container">
            <div className="row">
                <div className="col-12 mb-2" style={{whiteSpace: "nowrap", overflowX: "hidden"}}>
                <label className="text-light ms-2 username-SM">Halo, <b>{user.employeeName} </b></label>
                {/* andakan yang diminta by wholeseller */}
                {/* <label className="text-light ms-2 username">Halo, <b>{props.wholesellerName} </b></label> */}

                <button onClick={()=>setModalShow(!modalShow)} className="btn btn-warning float-end detail-information">Informasi Detail</button>
                {/* detail box */}
                {/* <button onClick={()=>setShow(!show)} className="btn btn-warning float-end detail-information">Informasi Detail</button> */}
                
               {modalShow? <PopupSalesman show={modalShow} onHide={() => setModalShow(false)} /> : null}


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

export default HeaderSalesman