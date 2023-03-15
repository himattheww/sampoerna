import { FC, useState, useEffect } from "react";
import { RouteComponentProps, useLocation } from "react-router";
import axios from "axios";
import { Link } from "react-router-dom";
import HeaderAdmin from "./HeaderAdmin";
import paths from "../ApiServices.json";

import "./leaderboard.css";
import Header from "./Header";
import PopupReset from "./PopupReset";
import { TextField } from "@mui/material";
import { Console } from "console";
import { Flip, toast, ToastContainer } from "react-toastify";
import { useForm } from "react-hook-form";
import ClipLoader from "react-spinners/ClipLoader";

type SomeComponentProps = RouteComponentProps;
const MaintainProfile: FC<SomeComponentProps> = ({ history }) => {
  const [loading, setLoading] = useState(false);

  const [urlResetAdmin, setUrlResetAdmin] = useState("");
  
  // search feature
  let [employeList, setEmployeeList] = useState<any[]>([]);
  let [filterEmployee, setfilterEmployee] = useState<any[]>([]);
  var urlData = "";
  let inputHandler = (e: any) => {
    var lowerCase = e.target.value.toLowerCase();
    if (lowerCase == "") {
      setfilterEmployee(employeList);
    } else {
      let filterData = employeList.filter((data) => {
        return (
          data.employeeCode.toLowerCase().includes(lowerCase) ||
          data.employeeName.toLowerCase().includes(lowerCase)
        );
      });
      setfilterEmployee(filterData);
    }
  };

  const [modalShow, setModalShow] = useState(false);
  const getUserList = process.env.REACT_APP_BASE_URL + paths.admin.getUserList;
  const getResetAdmin = process.env.REACT_APP_BASE_URL + paths.admin.resetAdmin;

  const getProfile = () => {
    const USER_TOKEN = localStorage.getItem("auth");
    const AuthStr = "Bearer " + USER_TOKEN;
    axios
      .get(getUserList, {
        headers: { Authorization: AuthStr, "Access-Control-Allow-Origin": "*" },
      })
      .then((res) => {
        setEmployeeList(res.data.data);
        setfilterEmployee(res.data.data);
      });
  };

  const getUrl = (customerCode: any) => {
    const USER_TOKEN = localStorage.getItem("auth");
    const AuthStr = "Bearer " + USER_TOKEN;
    setLoading(true);
    axios
      .put(
        getResetAdmin + customerCode,
        {},
        {
          headers: {
            Authorization: AuthStr,
            "Access-Control-Allow-Origin": "*",
          },
        }
      )
      .then((res) => {
        toast.success(res.data.message, {
          position: "top-right",
          autoClose: 3000,
          hideProgressBar: true,
          closeOnClick: true,
          pauseOnHover: true,
          draggable: false,
          progress: 0,
          toastId: "my_toast",
        });
        setLoading(false);
        setUrlResetAdmin(res.data.data.url);
        urlData = res.data.data.url;
        setModalShow(!modalShow);
      }).catch((error)=> {
        //handle error
        toast.error(error.response.data.message,{
                position: "top-right",
                autoClose: 3000,
                hideProgressBar: true,
                closeOnClick: true,
                pauseOnHover: true,
                draggable: false,
                progress: 0,
                toastId: "my_toast",
        })});
  };

  useEffect(() => {
    getProfile();
  }, []);

  if(loading == true ){
    return(
      <div className="Loadinggg">
      <ClipLoader
      size={30}
      color={"#123abc"}
      />
      Now Loading...
      </div>
    )
  }   

  return (
    <>
      <Header />
      <div className="main-form">
        <div className="container">
          <div className="reward">
            <div className="row">
              <div
                className="col ms-1"
                style={{
                  paddingLeft: "0px",
                  fontSize: "larger",
                }}
              >
                <Link to="/admin">
                  <i className="fa fa-arrow-left m-3" />
                </Link>
                <label className="p-3" style={{ marginLeft: "-20px" }}>
                  <b>Atur Ulang Sandi</b>
                </label>
              </div>
            </div>

            <div className="search pt-1">
              <div className="input-group">
                {/* <span className="input-group-text" id="basic-addon1"><i className="fa fa-search"></i></span> */}
                <TextField
                  id="outlined-basic"
                  onChange={inputHandler}
                  variant="outlined"
                  fullWidth
                  label="Search"
                />
                {/* <input type="text" onChange={inputHandler} className="form-control" placeholder="Find name or area" aria-label="Username" aria-describedby="basic-addon1" /> */}
              </div>
            </div>

            <div className="row">
              <div className="list">
                {filterEmployee &&
                  filterEmployee.length > 0 &&
                  filterEmployee.map((employee, currentPosts) => (
                    <div key={employee.employeeCode}>
                      <div className="list-item-maintain">
                        <div className="col-2">
                          <p
                            style={{
                              marginTop: "0px",
                            }}
                          >
                            <label>{employee.employeeCode}</label>
                          </p>
                        </div>
                        <div className="col-4 text-center">
                          <label>{employee.employeeName}</label>
                        </div>
                        <div className="col d-block text-end">
                          <a
                            onClick={() => {
                              getUrl(employee.employeeCode);
                              setModalShow(!modalShow);
                            }}
                            className="btn btn-oval"
                          >
                            Reset
                          </a>
                          {/* {modalShow? <PopupReset url= {urlResetAdmin} show={modalShow} onHide={() => setModalShow(false)} /> : null} */}
                        </div>
                      </div>
                    </div>
                  ))}
              </div>
            </div>
          </div>
        </div>
      </div>
      <ToastContainer
        position="top-right"
        autoClose={5000}
        hideProgressBar
        closeOnClick
        rtl={false}
        pauseOnFocusLoss={false}
        draggable={false}
        pauseOnHover
        limit={1}
        transition={Flip}
      />
    </>
  );
};
export default MaintainProfile;
