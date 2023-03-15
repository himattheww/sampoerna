import { FC, useState } from "react";
import { Link } from "react-router-dom";
import { useForm } from "react-hook-form";
import axios from "axios";
import { ToastContainer, toast, Flip } from "react-toastify";
import "react-toastify/dist/ReactToastify.min.css"
import { RouteComponentProps, useHistory, useLocation } from "react-router-dom";

import paths from "../ApiServices.json";




import "./leaderboard.css"
import mainlogo2 from "./mainlogo2.png"
import PopupReset from "./PopupReset";
import ClipLoader from "react-spinners/ClipLoader";

type SomeComponentProps = RouteComponentProps;
const ForgotPassword: FC<SomeComponentProps> = ({ history }): JSX.Element => {
const [loading, setLoading] = useState(false);
const passwordForgot = process.env.REACT_APP_BASE_URL + paths.employee.forgotPassword;




    const {
        register,
        handleSubmit,
        formState: { errors },
    } = useForm();

    const [customerCode, setCustomerCode] = useState("")
    const [phoneNumber, setPhoneNumber] = useState("")
    const [modalShow, setModalShow] = useState(false); 
    const [getUrl, setgetUrl] = useState("")

    

    const Forgot = () => {
        const dataForgot = {
            customerCode: customerCode,
            phoneNumber: phoneNumber
        };
        setLoading(true);
        axios
            .put(passwordForgot, dataForgot,{headers: {"Access-Control-Allow-Origin":"*"}})
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
              setgetUrl(res.data.data.url)
              setModalShow(!modalShow)
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
        return(
            <>
            <div className="text-center bg-main">
                <img className="img-fluid" src={mainlogo2} width="150" 
                    style={{
                        marginTop: "100px", 
                        marginBottom: "125px"}} />
            </div>
            <div className="main-form">
          <div className="login p-5 container">
              <h1>Lupa Kata Sandi</h1>
                <p>
                <form autoComplete="off" onSubmit={handleSubmit(Forgot)}>
                  <div className="mb-3">
                    <label className="form-label">Kode Pelanggan</label>
                    <input
                      type="text"
                      className="form-control"
                      {...register("customercode", { required: "Kode Pelanggan harap di isi !" })}
                      onChange={(e) => setCustomerCode(e.target.value)}
                    />
                    {errors.customercode && (
                      <p className="text-danger" style={{ fontSize: 14 }}>
                        {errors.customercode.message}
                      </p>
                    )}
                  </div>
                  <div className="mb-3">
                    <label className="form-label">Nomor Handphone</label>
                    <input
                      type="number"
                      className="form-control"
                      {...register("phonenumber", { required: "Nomor Handphone harap di isi !" })}
                      onChange={(e) => setPhoneNumber(e.target.value)}
                    />
                    {errors.phonenumber && (
                      <p className="text-danger" style={{ fontSize: 14 }}>
                        {errors.phonenumber.message}
                      </p>
                    )}
                  </div>
                  <div className="d-grid gap-4 mt-5">
                    <button
                      className="btn btn-warning text-center shadow-none mb-3"
                      type="button" 
                      onClick={handleSubmit(Forgot)}
                    >
                      Kirim Link
                    </button>
                    {/* {modalShow? <PopupReset url= {getUrl} show={modalShow} onHide={() => setModalShow(false)} /> : null} */}
                    <Link to={"/"} className="text-center">        
                      <button className="btn btn-link" >
                        Batal
                      </button>
                      </Link>
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

    export default ForgotPassword
