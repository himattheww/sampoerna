import React, { FC } from "react";
import { RouteComponentProps, Link } from "react-router-dom";
import HeaderAdmin from "./HeaderAdmin";

import "./home.css";
import user from "./user.png";
import uploaddata from "./uploaddata.png";
import round from "./round.png";
import Header from "./Header";

type SomeComponentProps = RouteComponentProps;
const Admin: FC<SomeComponentProps> = ({ history }) => {
  // untuk logout
  const logout = () => {
    localStorage.clear();
    history.push("/");
  };
  if(localStorage.getItem('auth')==null){
    logout()
  }

// coba session 
// window.onbeforeunload = () => {
//   localStorage.removeItem("auth");
// }
// coba session

// window.onbeforeunload = function (e) {
//   window.onunload = function () {
//           window.localStorage.isMySessionActive = "false";
//   }
//   return undefined;
// };

// window.onload = function () {
//           window.localStorage.isMySessionActive = "true";
// };


  return (
    <>
      <Header />
      <div className="main-form">
        <div className="login p-5 container menu">
          <Link to="/maintainprofile">
            <div className="card shadow menu-item">
              <div className="card-body">
                <div className="menu-icon menu-icon-red">
                  <img src={user} className="img-fluid-sandi" />
                </div>
                <div className="menu-label">
                  <label>Atur Ulang Sandi</label>
                </div>
              </div>
            </div>
          </Link>

          <Link to="/uploaddata">
            <div className="card shadow menu-item">
              <div className="card-body">
                <div className="menu-icon menu-icon-grey">
                  <img src={uploaddata} className="img-fluid-upload" />
                </div>
                <div className="menu-label">
                  <label>Unggah Data</label>
                </div>
              </div>
            </div>
          </Link>
          <Link to="/round">
            <div className="card shadow menu-item">
              <div className="card-body">
                <div className="menu-icon  menu-icon-blue">
                  <img src={round} className="img-fluid-ronde" />
                </div>
                <div className="menu-label">
                  <label>Atur Ulang Round</label>
                </div>
              </div>
            </div>
          </Link>
        </div>
      </div>
    </>
  );
};

export default Admin;
