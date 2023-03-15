import React, {FC, useEffect} from "react";
import { NavLink, RouteComponentProps } from "react-router-dom";
import { useState } from "react";
import axios from "axios";
import paths from "../ApiServices.json";


import "./leaderboard.css"
import HeaderAdmin from "./HeaderAdmin";

type SomeComponentProps = RouteComponentProps;
const Leaderboard: FC<SomeComponentProps> = ({ }) => {

   
    
    const getMain = process.env.REACT_APP_BASE_URL + paths.employee.getMain;
    const getLeaderboard = process.env.REACT_APP_BASE_URL + paths.employee.getLeaderboard;

    const [groupName, setGroupName] = useState("");
    const [roundName, setRoundName] = useState(0);
    const [roundDetail, setRoundDetail] = useState({
        currentRoundId: 0,
        currentRoundName: ""
    });
    const [leaderboards, setLeaderboards] = useState<any[]>([])

    const wholeseller = {
        wholesellerCode: localStorage.getItem("wholesellerCode")
    }

    const USER_TOKEN = localStorage.getItem("auth")
    const AuthStr = 'Bearer ' + USER_TOKEN
    const GetGroup = () => {
        axios
        .get(getMain + wholeseller.wholesellerCode, {headers: {Authorization: AuthStr,"Access-Control-Allow-Origin":"*"}})
        
        

        .then((res) => {
            localStorage.setItem("groupName", res.data.data.wholesellerDetail.groupName);
            setGroupName(res.data.data.wholesellerDetail.groupName);
            setRoundName(Number(localStorage.getItem("roundId")));
            setRoundDetail({
                currentRoundId: roundName,
                currentRoundName: localStorage.getItem("roundName")?.toString() || ""
            });
            getLeaderboardStats();
        })
    }

    const getLeaderboardStats = () => {
        const dataLeaderboard = {
            groupName: groupName,
            roundId: roundName
        }
        axios
            .post(getLeaderboard, dataLeaderboard , {headers: {Authorization: AuthStr,"Access-Control-Allow-Origin":"*"}})
            .then((res) => {
                setLeaderboards(res.data.data)
            })
            .catch((e) =>{
            })
    }

    useEffect(() => {
        GetGroup();
    }, [groupName, roundName])

return (
    <>
    <HeaderAdmin />
        <div className="main-form">
            <div className="container">
                <div className="search">
                    <div className="input-group mb-3 mt-0">
                        <span className="input-group-text" id="basic-addon1"><i className="fa fa-search"></i></span>
                        <input type="text" className="form-control" placeholder="Find name or area" aria-label="Username" aria-describedby="basic-addon1" />
                    </div>
                </div>
                <div className="reward">
                    <div className="row">
                        {roundDetail && roundDetail.currentRoundId != 0 && (
                            <div key={roundDetail.currentRoundId}>
                                <div className="col ms-1">
                                    <label>Anda sedang dalam <b>{roundDetail.currentRoundName}</b></label>
                                </div>
                            </div>
                        )}
                    <div className="row">
                        <div className="list mt-2">
                            {leaderboards && leaderboards.length > 0 && leaderboards.map((group) => (
                                <div key={group.wholesellerCode}>
                                    <div className="list-item">
                                        <div className="col-2">
                                            <span> Name </span>
                                            <label>{group.wholesellerName}</label>
                                        </div>
                                        <div className="col-4">
                                            <span> Area </span>
                                            <label>{group.wholesellerArea}</label>
                                        </div>
                                        <div className="col-4">
                                            <span>Point</span>
                                            <label>{group.salePoint}</label>
                                        </div>
                                        </div>
                                        </div>
                                    ))}
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
    </>
    )
}

export default Leaderboard