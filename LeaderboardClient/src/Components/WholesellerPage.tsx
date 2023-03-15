import React, { FC, useEffect, Fragment, useState, useMemo } from "react";
import { Link, NavLink, RouteComponentProps } from "react-router-dom";

import axios from "axios";
import DetailBox from "./DetailBox";
import Table from 'react-bootstrap/Table';
import { MDBTable} from 'mdbreact';
import WSHeader from "./WSHeader";
import "./leaderboard.css"

import paths from "../ApiServices.json";
import path from "path";
import { formatPoint } from "./Popup";

type SomeComponentProps = RouteComponentProps;
const WholesellerPage: FC<SomeComponentProps> = ({}) => {
  const [groupName, setGroupName] = useState();
  const [roundName, setRoundName] = useState(0);
  const [roundDetail, setRoundDetail] = useState({
    currentRoundId: 0,
    currentRoundName: "",
  });

  const detailRound = process.env.REACT_APP_BASE_URL + paths.employee.getRoundDetails;
  const getMain = process.env.REACT_APP_BASE_URL + paths.employee.getMain;
  const getLeaderboard = process.env.REACT_APP_BASE_URL + paths.employee.getLeaderboard;

  const [leaderboards, setLeaderboards] = useState<any[]>([]);
  const kodeemployee = localStorage.getItem("employeeCode");
  const wholeseller = {
    wholesellerCode: localStorage.getItem("wholesellerCode")
  };

  const USER_TOKEN = localStorage.getItem("auth");
  const AuthStr = "Bearer " + USER_TOKEN;

  const round = async () => {
    await axios
    .get(detailRound, {headers: {Authorization: AuthStr,"Access-Control-Allow-Origin":"*"}})
    .then((res) => {
        setRoundDetail(res.data.data)
        localStorage.setItem("roundId", res.data.data.currentRoundId)
        localStorage.setItem("roundName", res.data.data.currentRoundName)
        setRoundDetail({
            currentRoundId: res.data.data.currentRoundId,
            currentRoundName: res.data.data.currentRoundName
        })
    })
}



  const GetGroup = () => {

    axios
      .get(
        getMain + wholeseller.wholesellerCode,
        { headers: { Authorization: AuthStr , "Access-Control-Allow-Origin":"*"} }
      )
      .then((res) => {
        localStorage.setItem(
          "groupName",
          res.data.data.wholesellerDetail.groupName
        );
        setGroupName(res.data.data.wholesellerDetail.groupName);
        if(res.data.data.wholesellerDetail){
          localStorage.setItem("wholesellerDetail", JSON.stringify(res.data.data.wholesellerDetail))
        }
        setRoundName(Number(localStorage.getItem("roundId")));
        setRoundDetail({
          currentRoundId: roundName,
          currentRoundName: localStorage.getItem("roundName")?.toString() || "",
        });
        getLeaderboardStats();
      });
  };

  

  const getLeaderboardStats = async () => {
    const dataLeaderboard = {
      groupName: groupName,
      roundId: roundDetail.currentRoundId,
    };
    await axios
      .post(
        getLeaderboard,
        dataLeaderboard,
        { headers: { Authorization: AuthStr, "Access-Control-Allow-Origin":"*" } }
      
      )
      .then((res) => {
        setLeaderboards(res.data.data);
      })
      .catch((e) => {
      });
  };

  

  useEffect(() => {
    if(localStorage.getItem("roundId") == null){
      round();
    }
  })


  useEffect( () => {
    GetGroup();
  }, [roundDetail, groupName]);
  

  function highlightColor(name : any) {
    if (name==localStorage.getItem('wholesellerCode')) {
      return "#FFE5B4";
    }
  }

  return (
    <Fragment>
      <WSHeader groupName = {localStorage.getItem("groupName")}/>
        <div id="divOne">

      
        <table id="tblOne" className="table table-bordered table-striped mb-0"style={{overflowY:"scroll"}}>
      {/* <div> */}
        {/* <table> */}
      {/* <div className="ex1"> */}

          <thead>
            <tr style={{backgroundColor : "#ecbc44"}}>
              <th scope="col" id="WS-Rank">Rank</th>
              <th scope="col" id="WS-Panel">Nama Panel</th>
              <th scope="col" id="WS-Area">Area</th>
              <th scope="col" id="WS-Point">Point</th>
            </tr>
          </thead>
          
          <tbody className="table-leaderboard"  >
            {leaderboards &&
              leaderboards.length > 0 &&
              leaderboards.map((group, index  ) => (
                <tr key={index} style={{backgroundColor: highlightColor(group.wholesellerCode)}}>
                  {/* group.nama di api */}
                  <td>{group.rank}</td>
                  <td>{(group.wholesellerName)}</td>
                  <td>{group.wholesellerArea}</td>
                  <td>{(formatPoint(group.salePoint))}</td>
                </tr>
              ))}
          </tbody>
        {/* </div> */}
        </table>
        </div>
    </Fragment>
  );
};

export default WholesellerPage;
