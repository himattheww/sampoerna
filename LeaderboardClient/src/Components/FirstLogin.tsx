import { FC, useState } from "react";
import { Link } from "react-router-dom";
import { useForm } from "react-hook-form";
import axios from "axios";
import { ToastContainer, toast, Flip } from "react-toastify";
import "react-toastify/dist/ReactToastify.min.css"
import { RouteComponentProps, useHistory } from "react-router-dom";

import paths from "../ApiServices.json";
import "./leaderboard.css"
import mainlogo2 from "./mainlogo2.png"

type SomeComponentProps = RouteComponentProps;
const FirstLogin: FC<SomeComponentProps> = ({ history }) =>{

  const FlResetPass = process.env.REACT_APP_BASE_URL + paths.employee.firstLoginResetPassword;



  let hist = useHistory();
  const logout = () => {
      localStorage.clear();
      hist.goBack();
  };
    const {
        register,
        handleSubmit,
        formState: { errors },
    } = useForm();

    const [newPassword, setNewPassword] = useState("")
    const [confirmPassword, setConfirmPassword] = useState("")
    
    const ResetFirstLogin = () => {
        const resetBody = {
            customerCode: localStorage.getItem("customerCode"),
            newPassword: newPassword,
            confirmPassword: confirmPassword
        };
        axios
            .put(FlResetPass, resetBody, {headers: {"Access-Control-Allow-Origin":"*"}})
            .then((res) => {
            if (res.data.success === false) {
              toast.error(res.data.error, {
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
            hist.push("/");
          }, 3000);
            } else {
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
            hist.push("/");
          }, 3000);
        }
      })
      .catch((error) => {
        toast.error(error.response.data.message,{
                position: "top-right",
                autoClose: 3000,
                hideProgressBar: true,
                closeOnClick: true,
                pauseOnHover: true,
                draggable: false,
                progress: 0,
                toastId: "my_toast",
        })})
    };
    return (
        <>
        <div className="text-center bg-main">
                <img className="img-fluid" src={mainlogo2} width="150" 
                    style={{
                        marginTop: "100px", 
                        marginBottom: "125px"}} />
        </div>
        <div className="main-form">
            <div className="login p-5 container">
              <h4>Ini pertama kali anda login,</h4>
                <h4>Silahkan reset password</h4>
                <p>
                <form autoComplete="off" onSubmit={handleSubmit(ResetFirstLogin)}>
                  <div className="mb-3 mt-5">
                    <label className="form-label">Kata Sandi</label>
                    <input
                      type="password"
                      className="form-control"
                      {...register("password", { required: "Password baru harap di isi !" })}
                      onChange={(e) => setNewPassword(e.target.value)}
                    />
                    {errors.password && (
                      <p className="text-danger" style={{ fontSize: 14 }}>
                        {errors.password.message}
                      </p>
                    )}
                  </div>
                  <div className="mb-3">
                    <label className="form-label">Konfirmasi Kata Sandi</label>
                    <input
                      type="password"
                      className="form-control"
                      {...register("confirmpassword", { required: "Confirm password harap sesuai !" })}
                      onChange={(e) => setConfirmPassword(e.target.value)}
                    />
                    {errors.confirmpassword && (
                      <p className="text-danger" style={{ fontSize: 14 }}>
                        {errors.confirmpassword.message}
                      </p>
                    )}
                  </div>
                  <div className="d-grid gap-4 mt-5">
                    <button
                      className="btn btn-warning text-center shadow-none mb-3"
                      type="button" 
                      // onClick={()=>{handleSubmit(ResetFirstLogin);logout()}}
                      onClick={handleSubmit(ResetFirstLogin)}

                      // onClick={()=>{getUrl(employee.employeeCode);setModalShow(!modalShow);}}
                    >
                      Kirim
                    </button>
                    {/* <Link to="/login"> */}
                      {/* <button className="btn btn-link"> */}
                      <button className="btn btn-link" onClick={logout} >

                        Batal
                      </button>
                    {/* </Link> */}
                  </div>
                </form>
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
    )
  }


export default FirstLogin