import React, { FC, useEffect, Fragment, useState, useMemo } from "react";
import { Link, NavLink, RouteComponentProps } from "react-router-dom";
import HeaderAdmin from "./HeaderAdmin";
import axios from "axios";
import HeaderSalesman from "./HeaderSalesman";
import "./leaderboard.css"

import paths from "../ApiServices.json";
import { formatPoint } from "./Popup";



type SomeComponentProps = RouteComponentProps;
const SalesmanLeaderboard: FC<SomeComponentProps> = ({ history })   => {
  const stat : any = history.location.state;
  const wholesellernama : any = stat.wholesellerName;

  const detailRound = process.env.REACT_APP_BASE_URL + paths.employee.getRoundDetails;
  const getMain= process.env.REACT_APP_BASE_URL + paths.employee.getMain; 
  const getLeaderboard =  process.env.REACT_APP_BASE_URL + paths.employee.getLeaderboard;

  // window.onbeforeunload = () => {
  //   localStorage.removeItem("auth");
  // }



  const [groupName, setGroupName] = useState<String>();
  const [roundName, setRoundName] = useState(0);
  const [roundDetail, setRoundDetail] = useState({
    currentRoundId: 0,
    currentRoundName: "",
  });
  
  const [wholesellerDetail, setWholesellerDetail] = useState<any[]>([]);

  const [leaderboards, setLeaderboards] = useState<any[]>([]);

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




  const GetGroup =  () => {
     axios
      .get(
        getMain + wholeseller.wholesellerCode,
        { headers: { Authorization: AuthStr,"Access-Control-Allow-Origin":"*"  } }
      )
      .then((res) => {
        localStorage.setItem(
          "groupName",
          res.data.data.wholesellerDetail.groupName
        );
        
        setWholesellerDetail(res.data.data.wholesellerDetail);
        localStorage.setItem("wholesellerDetailSalesman", JSON.stringify(res.data.data.wholesellerDetail))
        localStorage.setItem("nameSalesman", res.data.data.employeeName)
        setRoundDetail({
          currentRoundId: roundName,
          currentRoundName: localStorage.getItem("roundName")?.toString() || "",
        });
        setGroupName(res.data.data.wholesellerDetail.groupName);
        getLeaderboardStats();
      });
  };

 

  const getLeaderboardStats =  () => {
    const dataLeaderboard = {
      groupName: stat.groupName,
      roundId: localStorage.getItem("roundId"),
    };
     axios
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
    GetGroup();
  }, [roundName])

  function highlightColor(name : any) {
    if (name==localStorage.getItem('wholesellerCode')) {
      return "#FFE5B4";
    }
  }




 

  // useEffect( () => {
    
  // }, []);

  
  

  return (
    <Fragment>
      <HeaderSalesman groupName={stat.groupName} wholesellerName={stat.wholesellerName}/>
      
       {/* tombol back */}
       <div 
                className="col ms-1"
                style={{
                  paddingLeft: "0px",
                  paddingBottom : "4px",
                  fontSize: "larger",
                }}
              >
                
                <Link to="/salesman">
                  <i className="fa fa-arrow-left m-3" />
                </Link>
                <label className="p-3" style={{ marginLeft: "-20px" }}>
                  <b>Salesman</b>
                </label>
                
              </div>
          {/* tombol back */}
          
        <div id="divOne">
        <table id="tblOne" className="table table-bordered table-striped mb-0" style={{overflowY:"scroll"}}>
      {/* <div className="ex2"> */}
      {/* <div> */}
        {/* <table> */}
        
          <thead>
            <tr style={{backgroundColor : "#ecbc44"}}>
              <th scope="col" id="SM-Rank">Rank</th>
              <th scope="col" id="SM-Rank">Nama Panel</th>
              <th scope="col" id="SM-Rank">Area</th>
              <th scope="col" id="SM-Rank">Point</th>
            </tr>
          </thead>
         
          <tbody className="table-leaderboardSM">
            {leaderboards &&
              leaderboards.length > 0 &&
              leaderboards.map((group, index) => (
                <tr key={index} style={{backgroundColor: highlightColor(group.wholesellerCode)}}>
                  {/* group.nama di api */}
                  <td>{group.rank}</td>
                  <td>{group.wholesellerName}</td>
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

export default SalesmanLeaderboard;
