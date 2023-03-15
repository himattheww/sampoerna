import React from "react";
import { FcOk } from "react-icons/fc";
import Button from "react-bootstrap/Button";
import Modal from "react-bootstrap/Modal";
import { Link } from "react-router-dom";
import Success from "./Success.jpg";
import "./button.css";

const PopupRound = (props: any) => {
  return (
    <Modal
      {...props}
      size="lg"
      dialogClassName="modal-size"
      aria-labelledby="contained-modal-title-vcenter"
      centered
    >
      <Modal.Header closeButton>
        {/* <Modal.Title id="contained-modal-title-vcenter">Edit Round</Modal.Title> */}
      </Modal.Header>
      <Modal.Body>
        <h2 className="editRoundTitle" >Edit Round</h2>
        <FcOk className="iconOK" />
        

        {/* <img src={Success} className="center" style={{height: "100px", width: "100px", marginTop: "-5px"}} /> */}

        <p className="editRoundparagraph">Successfully save your update</p>
      </Modal.Body>
      <Modal.Footer>
        {/* <Button onClick={props.onHide}>Close</Button> */}
        <Link to="/round">
          <Button className="button doneRound">Done</Button>
        </Link>
      </Modal.Footer>
    </Modal>
  );
};
export default PopupRound;
