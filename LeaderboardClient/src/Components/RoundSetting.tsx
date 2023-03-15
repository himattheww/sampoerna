import React, { Fragment, useEffect, useState } from "react";
import type { DatePickerProps, TimePickerProps, RadioChangeEvent } from "antd";
import { DatePicker, Select, Space, TimePicker, Radio } from "antd";
import { Link, RouteComponentProps } from "react-router-dom";
import axios from "axios";
import { useForm } from "react-hook-form";
import HeaderAdmin from "./HeaderAdmin";
import "./button.css";
import PopupRound from "./PopUpRound";
import Popup from "./Popup";
import PopupSalesman from "./PopupSalesman";
import Header from "./Header";
import "./header.css"
import "./leaderboard.css"

import paths from "../ApiServices.json";


const { RangePicker } = DatePicker;

type SomeComponentProps = RouteComponentProps;
const RoundSetting: React.FC<SomeComponentProps> = (props) => {

  const USER_TOKEN = localStorage.getItem("auth");
  const AuthStr = "Bearer " + USER_TOKEN;

  const [modalShow, setModalShow] = useState(false);
  const roundUpdate = process.env.REACT_APP_BASE_URL + paths.admin.roundUpdate;

  let data: any = props.location.state;
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm();

  const [startDate, setStartDate] = useState("");
  const [endDate, setEndDate] = useState("");

  const startDateChangeHandler = (event: any) => {
    setStartDate(event.target.value);
  };

  const endDateChangeHandler = (event: any) => {
    setEndDate(event.target.value);
  };

 

  function callBoth(){
    // handleSubmit(DateRound)
    DateRound()
    setModalShow(!modalShow)
  }

  const DateRound = () => {
    let data: any = props.location.state;
    const stringdata : string = data.toString();

    const roundDate = {
      roundId: data.roundId,
      roundName: data.roundName,
      startDate: startDate,
      endDate: endDate,
    };
    axios
      .put(roundUpdate, roundDate,{headers: {Authorization: AuthStr,"Access-Control-Allow-Origin":"*"}})
      .then((res) => {
      });

      
  };

  return (
    <Fragment>
      <Header/>
      {/* tombol back */}
      <div
        className="col ms-1"
        style={{
          paddingLeft: "0px",
          paddingBottom: "4px",
          fontSize: "larger",
        }}
      >
        <Link to="/round">
          <i className="fa fa-arrow-left m-3" />
        </Link>
        <label className="p-3" style={{ marginLeft: "-20px" }}>
          <b>Round Setting</b>
        </label>
      </div>
      {/* tombol back */}
      <h6 className="ronde-sekarang">Round {data.roundId}</h6>

      <div className="start-labelinput">
      
      <label className="roundsetting-label">Mulai Round </label>
      <div>
        <input className="start-round-input"
          type="date"
          value={startDate}
          onChange={startDateChangeHandler}
          />
      </div>
          </div>
      
      <div className="end-labelinput">

      <label className="roundsetting-label">Round Berakhir</label>
      <div>
        <input className="end-round-input" type="date" value={endDate} onChange={endDateChangeHandler} />
      </div>
      </div>



      
      {/* button cancel */}
      <Link to="/round">
        <button
          className="btn cancelbutton btn-light btn-outline-danger  text-center shadow-none mb-3"
          type="button"
        >
          Cancel
        </button>
      </Link>

      {/* button save */}
        <button
          className="btn btn-danger savebutton text-center shadow-none mb-3 mr-3"
          type="button"
          onClick={callBoth}
        >
          Save
        </button>
        {modalShow? <PopupRound show={modalShow} onHide={() => setModalShow(false)} /> : null}


           {/* popup round start */}
      {/* <button onClick={()=>setModalShow(!modalShow)} className="btn btn-warning float-end detail-information">ZZZZ</button>
      {modalShow? <PopupRound show={modalShow} onHide={() => setModalShow(false)} /> : null} */}
      {/* popup round end */}



    </Fragment>
  );
};

export default RoundSetting;
