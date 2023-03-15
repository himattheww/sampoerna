import { FC, useState, useEffect } from "react";
import { RouteComponentProps } from "react-router";
import axios from "axios";
import { Link } from "react-router-dom";
import HeaderAdmin from "./HeaderAdmin";
import { DatePicker, DatePickerProps, RadioChangeEvent } from "antd";
import "./leaderboard.css";
import Header from "./Header";
import paths from "../ApiServices.json";



type SomeComponentProps = RouteComponentProps;
const Round: FC<SomeComponentProps> = ({ history }) => {

  // window.onbeforeunload = () => {
  //   localStorage.removeItem("auth");
  // }

  const getRoundd = process.env.REACT_APP_BASE_URL + paths.admin.getRound;


  const [roundList, setRoundList] = useState<any[]>([]);
  // date start here
  const { RangePicker } = DatePicker;
  const calendarz = () => {
    return (
      <>
        <td>ini tombol tanggal</td>
        <RangePicker style={{ marginLeft: "17cm" }} />
      </>
    );
  };

  // date end here
  const getRound = () => {
    const USER_TOKEN = localStorage.getItem("auth");
    const AuthStr = "Bearer " + USER_TOKEN;
    axios
      .get(getRoundd, {
        headers: { Authorization: AuthStr, "Access-Control-Allow-Origin":"*"  },
      })
      .then((res) => {
        setRoundList(res.data.data);
      });
  };

  const detail = (round: any) => {
    // localStorage.setItem("roundId", selectedWs);
    const data = {
     roundId : round.roundId,
     roundName : round.roundName,
    }
    // hover ke push, liat apa aja yang bisa dimasukin
    history.push("/roundsetting",data)
  };

  useEffect(() => {
    getRound();
  }, []);

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
                  <b>Atur Ulang Round</b>
                </label>
              </div>
            </div>

            <div className="row">
              <div className="list">
                {roundList &&
                  roundList.length > 0 &&
                  roundList.map((round) => (
                    <div key={round.roundId}>
                      <div className="list-item-round">
                        <div className="col-2">
                          <p
                            style={{
                              marginTop: "16px",
                            }}
                          >
                            <label>{round.roundName}</label>
                          </p>
                        </div>
                        <div className="col-4 text-center">
                          <label>
                            {round.startDate.replace("T00:00:00", "")}
                          </label>
                          <span> until </span>
                          <label>
                            {round.endDate.replace("T00:00:00", "")}
                          </label>
                        </div>
                            

                            {/* yang diganti */}
                        <div className="col d-block text-end">
                          <a
                            className="btn btn-oval"
                            onClick={(e) => detail(round)}
                          >
                            Edit
                            {/* <i className="btn btn-oval">Edit</i> */}
                          </a>
                        </div>
                        {/* yang diganti */}

                        {/* <div className="col d-block text-end">
                          <a
                            className="btn btn-oval"
                            onChange={() => calendarz}
                          >
                            Edit
                          </a>
                        </div> */}
                      </div>
                    </div>
                  ))}
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default Round;
