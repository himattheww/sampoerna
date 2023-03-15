import React, {FC, useEffect} from "react";
import { NavLink, RouteComponentProps } from "react-router-dom";
import { useState } from "react";
import axios from "axios";

import "./leaderboard.css"
import HeaderAdmin from "./HeaderAdmin";
import Header from "./Header";

import paths from "../ApiServices.json";
import { TextField } from "@mui/material";
import HeaderSM from "./HeaderSM";


type SomeComponentProps = RouteComponentProps;
const Salesman: FC<SomeComponentProps> = ({ history }) => {

    const detailRound = process.env.REACT_APP_BASE_URL + paths.employee.getRoundDetails;
    const getMain= process.env.REACT_APP_BASE_URL +  paths.employee.getMain; 
    const getLeaderboard = paths.employee.getLeaderboard;
    
    // wholesellerList
    // wholesellerCode, wholesellerName, groupName

    // window.onbeforeunload = () => {
    //     localStorage.removeItem("auth");
    //   }
    
    const [whosellerList, setWholeSellerList] = useState<any[]>([])
    const [roundDetail, setRoundDetail] = useState({
        currentRoundId: 0,
        currentRoundName: ""
    })
    let [filterEmployee, setfilterEmployee] = useState<any[]>([])


    let inputHandler = (e : any) => {
  
        var lowerCase = e.target.value.toLowerCase()
        if(lowerCase == ''){
            setfilterEmployee(whosellerList);
        } else{
            
            let filterData = whosellerList.filter(data => {
                return data.wholesellerCode.toLowerCase().includes(lowerCase) || data.wholesellerName.toLowerCase().includes(lowerCase) || data.groupName.toLowerCase().includes(lowerCase) ;
            })
            setfilterEmployee(filterData);
        }
    }

    const user = {
        employeeCode: localStorage.getItem("employeeCode")
    }

    const round = () => {
        axios
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

    const userSales = {
        employeeCode: localStorage.getItem("employeeCode")
    }

    const USER_TOKEN = localStorage.getItem("auth")
    const AuthStr = 'Bearer ' + USER_TOKEN
    const GetWholeSellerList = () => {
        axios
        .get(getMain + user.employeeCode, {headers: {Authorization: AuthStr,"Access-Control-Allow-Origin":"*"}})
        .then((res) => {
            setWholeSellerList(res.data.data.wholesellers)
            
            // coba set item salesmanDetail
            localStorage.setItem("salesmanName", res.data.data.employeeName);
            localStorage.setItem("salesmanDetail", res.data.data.wholesellers)
            setfilterEmployee(res.data.data.wholesellers)
            
            // localStorage.setItem("groupName", res.data.data.groupName)
        })
    }
    

    const detail = (selectedWs: string,wholeseller: any) => {
        localStorage.setItem("wholesellerCode", selectedWs )
        history.push("/leaderboards",wholeseller)

    }
    
    useEffect(() => {
        round()
        GetWholeSellerList()
    }, [])

    return (
        <>
        {/* <Header /> */}
        <HeaderSM/>

            <div className="main-form">
                <div className="container">
                    <div className="search">
                        <div className="input-group mb-3 mt-0">
                        <TextField  id="outlined-basic" onChange={inputHandler} variant="outlined" fullWidth label="Search"/>
                            {/* <span className="input-group-text" id="basic-addon1"><i className="fa fa-search"></i></span>
                            <input type="text" className="form-control" placeholder="Find name or area" aria-label="Username" aria-describedby="basic-addon1" /> */}
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
                                {filterEmployee && filterEmployee.length > 0 && filterEmployee.map((Wholeseller) => (
                                    <div key={Wholeseller.wholesellerCode}>
                                        <div className="list-item">
                                            <div className="col-2">
                                                <span> Grup </span>
                                                <label>{Wholeseller.groupName}</label>
                                            </div>
                                            <div className="col-4">
                                                <span> Kode WS </span>
                                                <label>{Wholeseller.wholesellerCode}</label>
                                            </div>
                                            <div className="col-4">
                                                <span>Nama WS</span>
                                                <label>{Wholeseller.wholesellerName}</label>
                                            </div>
                                            <div className="col d-block text-end">
                                                <a className="btn btn-circle" onClick={(e) => detail(Wholeseller.wholesellerCode,Wholeseller)}><i className="fa fa-arrow-right text-light"></i></a>
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

export default Salesman