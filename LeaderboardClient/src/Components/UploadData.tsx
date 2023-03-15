import { FC, useState, useEffect, Fragment, ChangeEvent } from "react";
import { RouteComponentProps } from "react-router";
import ProgressBar from "react-bootstrap/ProgressBar";
import axios from "axios";
import { Link } from "react-router-dom";
import HeaderAdmin from "./HeaderAdmin";

import paths from "../ApiServices.json";

import Header from "./Header";
import "./leaderboard.css";
import { Flip, toast, ToastContainer } from "react-toastify";

import { Audio, ColorRing } from "react-loader-spinner";
import ClipLoader from "react-spinners/ClipLoader";
type SomeComponentProps = RouteComponentProps;

const UploadData: FC<SomeComponentProps> = ({ history }) => {
  const uploadFile = process.env.REACT_APP_BASE_URL + paths.admin.uploadFile;
  const downloadFile = process.env.REACT_APP_BASE_URL + paths.admin.downloadFile;
  const downloadAllEmployee = process.env.REACT_APP_BASE_URL + paths.admin.downloadAllEmployee;
  const downloadAllDataLeaderboard = process.env.REACT_APP_BASE_URL + paths.admin.downloadAllDataLeaderboard;

  // submit start here
  const USER_TOKEN = localStorage.getItem("auth");
  const AuthStr = "Bearer " + USER_TOKEN;
  const [filebase64, setFileBase64] = useState<string>("");
  const [file, setFile] = useState<any>();
  const [fileinfo, setFileInfo] = useState<any[]>([]);
  const [percentage, setPercentage] = useState(0);
  const [loading, setLoading] = useState(false);

  function formSubmit(e: any) {
    e.preventDefault();
    // Submit your form with the filebase64 as
    // one of your fields

    var data = new FormData();

    // bodyFormData.append('fileName', fileName);
    data.append("file", file);

    var config = {
      onUploadProgress: function (progressEvent: any) {
        const { loaded, total } = progressEvent;
        let percentCompleted = Math.round(
          (progressEvent.loaded * 100) / progressEvent.total
        );
        setPercentage(percentCompleted);
      },
      headers: { Authorization: AuthStr, "Access-Control-Allow-Origin": "*" },
    };
    axios
      .post(uploadFile, data, config)
      // receive two parameter endpoint url ,form data
      .then((response) => {
        toast.success(response.data.message, {
          position: "top-right",
          autoClose: 3000,
          hideProgressBar: true,
          closeOnClick: true,
          pauseOnHover: true,
          draggable: false,
          progress: 0,
          toastId: "my_toast",
        });
        //handle success
      })
      .catch((error) => {
        // percobaan
        //handle error
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

    getDataUpload();
  }

  // download
  function downloadFileExcel(base64: any, fileName: any) {
    const byteString = window.atob(base64);
    const arrayBuffer = new ArrayBuffer(byteString.length);
    const int8Array = new Uint8Array(arrayBuffer);
    for (let i = 0; i < byteString.length; i++) {
      int8Array[i] = byteString.charCodeAt(i);
    }

    // const blob = new Blob([int8Array], { type: "application/vnd.ms-excel" });
    const blob = new Blob([int8Array], { type: "text/csv" });
    // const blob = new Blob([int8Array], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
    const url = URL.createObjectURL(blob);
    var a = document.createElement("a");
    a.href = url;
    a.download = fileName;
    a.click();
    window.URL.revokeObjectURL(url);
    // window.open(url, "_blank");
  }

  function downloadAllEmployees(fileList: any) {
    axios
      .get(downloadAllEmployee, {
        headers: { Authorization: AuthStr, "Access-Control-Allow-Origin": "*" },
      })
      .then((response) => {
        if (response.data.statusCode === 200) {
          var base64 = response.data.data.data;
          var fileName = response.data.data.namaFile;
          const byteString = window.atob(base64);
          const arrayBuffer = new ArrayBuffer(byteString.length);
          const int8Array = new Uint8Array(arrayBuffer);
          for (let i = 0; i < byteString.length; i++) {
            int8Array[i] = byteString.charCodeAt(i);
          }

          const blob = new Blob([int8Array], { type: "text/csv" });

          // const blob = new Blob([int8Array], {
          //   type: "application/vnd.ms-excel",
          // });
          const url = URL.createObjectURL(blob);
          var a = document.createElement("a");
          a.href = url;
          a.download = fileName;
          a.click();
          window.URL.revokeObjectURL(url);
          // window.open(url, "_blank");
          toast.success(response.data.message, {
            position: "top-right",
            autoClose: 3000,
            hideProgressBar: true,
            closeOnClick: true,
            pauseOnHover: true,
            draggable: false,
            progress: 0,
            toastId: "my_toast",
          });
          setTimeout(() => {}, 3000);
        } else {
          toast.error(response.data.message, {
            position: "top-right",
            autoClose: 3000,
            hideProgressBar: true,
            closeOnClick: true,
            pauseOnHover: true,
            draggable: false,
            progress: 0,
            toastId: "my_toast",
          });
          setTimeout(() => {}, 3000);
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
  }

  function downloadAllLeaderboards(fileList: any) {
    axios
      .get(downloadAllDataLeaderboard, {
        headers: { Authorization: AuthStr, "Access-Control-Allow-Origin": "*" },
      })
      .then((response) => {
        if (response.data.statusCode === 200) {
          var base64 = response.data.data.data;
          var fileName = response.data.data.namaFile;
          const byteString = window.atob(base64);
          const arrayBuffer = new ArrayBuffer(byteString.length);
          const int8Array = new Uint8Array(arrayBuffer);
          for (let i = 0; i < byteString.length; i++) {
            int8Array[i] = byteString.charCodeAt(i);
          }

          const blob = new Blob([int8Array], { type: "text/csv" });

          // const blob = new Blob([int8Array], {
          //   type: "application/vnd.ms-excel",
          // });
          // const blob = new Blob([int8Array], { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" });
          const url = URL.createObjectURL(blob);
          var a = document.createElement("a");
          a.href = url;
          a.download = fileName;
          a.click();
          window.URL.revokeObjectURL(url);
          // window.open(url, "_blank");
          toast.success(response.data.message, {
            position: "top-right",
            autoClose: 3000,
            hideProgressBar: true,
            closeOnClick: true,
            pauseOnHover: true,
            draggable: false,
            progress: 0,
            toastId: "my_toast",
          });
          setTimeout(() => {}, 3000);
        } else {
          toast.error(response.data.message, {
            position: "top-right",
            autoClose: 3000,
            hideProgressBar: true,
            closeOnClick: true,
            pauseOnHover: true,
            draggable: false,
            progress: 0,
            toastId: "my_toast",
          });
          setTimeout(() => {}, 3000);
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
  }

  const getDataUpload = () => {
    setLoading(true);
    axios
      .get(downloadFile, {
        headers: { Authorization: AuthStr, "Access-Control-Allow-Origin": "*" },
      })
      .then(function (response) {
        //handle success
        setFileInfo(response.data.data.result);
        setLoading(false);
      })
      .catch(function (response) {
        //handle error
      });
  };
  useEffect(() => {
    getDataUpload();
    // didalam kurung fileinfo biar dia ga click refresh"
  }, []);

  // function handleFile(e: any) {
  // }

  function convertFile(files: FileList | null) {
    setPercentage(0);
    if (files) {
      const fileRef = files[0] || "";
      const fileType: string = fileRef.type || "";
      const fileName: string = fileRef.name || "";
      setFile(fileRef);
      const reader = new FileReader();
      reader.readAsBinaryString(fileRef);
      reader.onload = (ev: any) => {
        // convert it to base64
        setFileBase64(`data:${fileType};base64,${btoa(ev.target.result)}`);
      };
    }
  }
  // submit end here

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
    
    <Fragment>
      <Header />
      {loading && 
      
      <ClipLoader 
      size={30}
      color={"#123abc"}
      loading={loading}
      />}
      
      <div className="main-form ">
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
                  <b>Unggah Data</b>
                </label>

                {/* submit start here */}
                <form onSubmit={formSubmit}>
                  <input
                    type="file"
                    onChange={(e) => convertFile(e.target.files)}
                  />
                  <hr />
                  {filebase64 && (
                    <>
                      <p>File telah siap untuk di submit</p>
                      {/* progress bar */}
                      <div
                        className="progress-bar progress-bar-striped progress-bar-animated"
                        style={{ height: 30 }}
                      >
                        {percentage > 0 && (
                          <ProgressBar
                            now={percentage}
                            label={`${percentage}%`}
                          />
                        )}
                      </div>
                      {/* progress bar */}

                      {/* if it's an image */}

                      {filebase64.indexOf("image/") > -1 && (
                        <img src={filebase64} width={300} />
                      )}
                      {/* if it's an image */}

                      {/* if it's a video */}
                      {filebase64.indexOf("video/") > -1 && (
                        <video controls>
                          <source src={filebase64} />
                        </video>
                      )}
                      {/* if it's a video */}

                      {/* if it's a audio (music, sound) */}

                      {filebase64.indexOf("audio/") > -1 && (
                        <audio controls>
                          <source src={filebase64} />
                        </audio>
                      )}
                      {/* if it's a audio (music, sound) */}

                      {/* if it's a PDF */}
                      {filebase64.indexOf("application/pdf") > -1 && (
                        <embed src={filebase64} width="800px" height="2100px" />
                      )}
                      {/* if it's a PDF */}

                      <hr />
                      <button> Submit</button>
                    </>
                  )}
                </form>
                {/* submit end here */}
              </div>
            </div>
          </div>
        </div>

        {/* content tabel mulai dari sini */}
        <label className="click_File">click nama file untuk mendownload</label>
        <br />
        <br />

        <div className="button_Download">
          <button
            className="all-employee"
            // style={{ cursor: "pointer" }}
            onClick={() => downloadAllEmployees(fileinfo)}
          >
            Download All Employee
          </button>
          <button
            className="dataLeaderboardss"
            onClick={() => downloadAllLeaderboards(fileinfo)}
          >
            Download Leaderboards Data
          </button>
        </div>

        {/* <div className="table-wrapper-scroll-y my-custom-scrollbar"> */}
        {/* <div style={{display:'flex',justifyContent:'center'}}> */}

        <div id="divOne">
          <table
            id="tblOne"
            className="table table-bordered table-striped mb-0"
          >
            <thead className="head-upload">
              <tr style={{ backgroundColor: "#ecbc44" }}>
                <th scope="col" id="UD-No">
                  No
                </th>
                <th>Name File</th>
                <th scope="col" id="UD-UTime">
                  Upload Time
                </th>
                <th scope="col" id="UD-UFinisih">
                  Finish Upload
                </th>
                <th scope="col" id="UD-UploadBy">
                  Upload By
                </th>
                <th scope="col" id="UD-SProcess">
                  Status Process
                </th>
              </tr>
            </thead>

            <tbody className="table-leaderboard">
              {!loading &&
                fileinfo &&
                fileinfo.length > 0 &&
                fileinfo.map((group, index) => (
                  <tr key={index}>
                    <td>{index + 1}</td>
                    <td
                      style={{ cursor: "pointer" }}
                      onClick={() =>
                        downloadFileExcel(group.data, group.nameFile)
                      }
                    >
                      {group.nameFile}
                    </td>
                    <td>{group.uploadDateTime.replace("T", " ")}</td>
                    <td>{group.finishUpload.replace("T", " ")}</td>
                    <td>{group.userUpload}</td>
                    <td>{group.status}</td>
                  </tr>
                ))}
            </tbody>
          </table>
        </div>
      </div>

      {/* </div> */}
      {/* content tabel berakhir disini */}
      {/* </div> */}
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
    </Fragment>
  );
};

export default UploadData;
