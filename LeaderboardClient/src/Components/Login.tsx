import React, { FC, useState } from "react";
import { Link } from "react-router-dom";
import { useForm } from "react-hook-form";
import axios from "axios";
import { ToastContainer, toast, Flip } from "react-toastify";
import "react-toastify/dist/ReactToastify.min.css";
import { RouteComponentProps } from "react-router";
import paths from "../ApiServices.json";
// encrypt
import bcrypt from "bcryptjs";
// encrypt 

import "./header.css";
import "./leaderboard.css";
import mainlogo2 from "./mainlogo2.png";



type SomeComponentProps = RouteComponentProps;
const Login: FC<SomeComponentProps> = ({ history }): JSX.Element => {
  const loginEmployee = process.env.REACT_APP_BASE_URL + paths.employee.login;
console.log("Login employee", loginEmployee)
  // coba env
  const link = process.env.REACT_APP_BASE_URL;
  const linkgabungan = link+loginEmployee;
  // success env

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm();

  const [customerCode, setCustomerCode] = useState("");
  const [password, setPassword] = useState("");
  // hash pass
  const hashedPassword = bcrypt.hashSync(password, 10);

  const testing = "xedasdsmyname"
  const headerx = btoa(customerCode+":"+password);

  const md5 = require('md5')
  

  const login = () => {
    const dataLogin = {
      customerCode: customerCode,
      password: password,
    };
  
   

    axios
      .post(loginEmployee, null, {
        headers: { "x-token": headerx ,"Access-Control-Allow-Origin": "*" },
      })
      .then((res) => {
        if (
          res.data.data.firstLogin === false &&
          res.data.data.employeePhone == false
        ) {
          localStorage.setItem(
            "cumulativePoint",
            res.data.data.mainData.kumulatif
          );
          localStorage.setItem(
            "employeeCode",
            res.data.data.mainData.employeeCode
          );
          localStorage.setItem(
            "employeeName",
            res.data.data.mainData.employeeName
          );
          localStorage.setItem(
            "employeeRole",
            res.data.data.mainData.employeeRole
          );
          localStorage.setItem("roleName", res.data.data.mainData.roleName);
          localStorage.setItem(
            "wholesellerDetail",
            res.data.data.mainData.wholesellerDetail
          );
          localStorage.setItem("groupName", res.data.data.mainData.groupName);

          var roleType = res.data.data.mainData.employeeRole;
          switch (roleType) {
            case "A":
              localStorage.setItem("auth", res.data.data.token);

              setTimeout(() => {
                history.push("/admin");
              }, 3000);
              break;
            case "S":
              localStorage.setItem("auth", res.data.data.token);
              setTimeout(() => {
                history.push("/salesman");
              }, 3000);
              break;
            case "W":
              localStorage.setItem("auth", res.data.data.token);
              localStorage.setItem(
                "wholesellerCode",
                res.data.data.mainData.employeeCode
              );
              setTimeout(() => {
                history.push("/wholeseller");
              }, 3000);
              break;
          }
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

          return;
        } else if (
          res.data.data.firstLogin === false &&
          res.data.data.employeePhone == true
        ) {
          // berarti ngambil dari sini?
          localStorage.setItem("customerCode", customerCode);
          // localStorage.setItem("auth", res.data.data.token);
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
          setTimeout(() => {
            history.push("/updatephonenumber");
          }, 3000);
        } else {
          localStorage.setItem("customerCode", customerCode);
          // localStorage.setItem("auth", res.data.data.token);
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
          setTimeout(() => {
            history.push("/firstlogin");
          }, 3000);
        }
      })
      .catch((error) => {
        toast.error(error.response.data.message, {
          position: "top-right",
          autoClose: 3000,
          hideProgressBar: true,
          closeOnClick: true,
          pauseOnHover: true,
          draggable: false,
          progress: 0,
          toastId: "my_toast",
        });
      });
  };

  return (
    <>
      <div className="text-center bg-main">
        <img
          className="img-fluid"
          src={mainlogo2}
          width="150"
          style={{
            marginTop: "100px",
            marginBottom: "125px",
          }}
        />
      </div>
      <div className="main-form">
        <div className="login p-5 container">
          <h3>Selamat datang di</h3>
          <h1>Liga Mahakarya</h1>
          <p>
            <form autoComplete="off" onSubmit={handleSubmit(login)}>
              <div className="mb-3">
                <label className="form-label">Kode Pelanggan</label>
                <input
                  type="text"
                  className="form-control"
                  {...register("customercode", {
                    required: "Customer code harap di isi !",
                  })}
                  onChange={(e) => setCustomerCode(e.target.value)}
                />
                {errors.customercode && (
                  <p className="text-danger" style={{ fontSize: 14 }}>
                    {errors.customercode.message}
                  </p>
                )}
              </div>
              <div className="mb-3">
                <label className="form-label">
                  Kata Sandi *password case sensitive
                </label>
                <input
                  type="password"
                  className="form-control"
                  {...register("password", {
                    required: "Password harap di isi !",
                  })}
                  onChange={(e) => setPassword(e.target.value)}
                />
                {errors.password && (
                  <p className="text-danger" style={{ fontSize: 14 }}>
                    {errors.password.message}
                  </p>
                )}
              </div>
              <div className="d-grid gap-4 mt-5">
                <button
                  className="btn btn-warning text-center shadow-none mb-3"
                  type="button"
                  onClick={handleSubmit(login)}
                >
                  Masuk
                </button>
              </div>
            </form>
            <div className="d-grid gap-4 mt-5">
              <Link to={"/forgotpassword"} className="text-center">
                <button className="btn btn-link">Lupa Kata Sandi ?</button>
              </Link>
            </div>
          </p>
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
export default Login;
