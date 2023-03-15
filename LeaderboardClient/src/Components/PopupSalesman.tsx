import React from 'react';
import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
import { formatPoint } from './Popup';

const PopupSalesman= (props : any) => {
    const data = {
        employeeName: localStorage.getItem("nameSalesman"),
        wholesellerDetail: JSON.parse(localStorage.getItem("wholesellerDetailSalesman") || '{"pointA":"senpai","pointB":"senpai","pointC":"senpai"}'),
    };


    if(data.wholesellerDetail.kumulatif == null){
      return (
        <Modal
          {...props}
          size="lg"
          aria-labelledby="contained-modal-title-vcenter"
          centered
        >
          <Modal.Header closeButton>
            <Modal.Title id="contained-modal-title-vcenter">
                Informasi Detail
            </Modal.Title>
          </Modal.Header>
          <Modal.Body>
    <div>
    
    
    <tr>
      <td>Wholesaler Name : {data.employeeName}</td>
    </tr>
    
    
    
    
    <tr>
      <td>Point Tetap : {(formatPoint(data.wholesellerDetail.pointA))}</td>
    </tr>
    <tr>
      <td>Potensi Point : {(formatPoint(data.wholesellerDetail.pointB))}</td>
    </tr>
    
    <tr>
      <td>Total Point : {(formatPoint(data.wholesellerDetail.salePoint))}</td>
    </tr>
    <tr>
      <td>Target Stock Mingguan : {(formatPoint(data.wholesellerDetail.baselineStock))}</td>
    </tr>

    <tr>
      <td>Last Update : {data.wholesellerDetail.latestUpdate.replace("T00:00:00", "")}</td>
    </tr>
    </div>
          </Modal.Body>
          <Modal.Footer>
            <Button onClick={props.onHide}>Close</Button>
          </Modal.Footer>
        </Modal>
      );
    }
  return (
    <Modal
      {...props}
      size="lg"
      aria-labelledby="contained-modal-title-vcenter"
      centered
    >
      <Modal.Header closeButton>
        <Modal.Title id="contained-modal-title-vcenter">
            Informasi Detail
        </Modal.Title>
      </Modal.Header>
      <Modal.Body>
<div>


<tr>
  <td>Wholesaler Name : {data.employeeName}</td>
</tr>




<tr>
  <td>Point Tetap : {(formatPoint(data.wholesellerDetail.pointA))}</td>
</tr>
<tr>
  <td>Potensi Point : {(formatPoint(data.wholesellerDetail.pointB))}</td>
</tr>

<tr>
  <td>Total Point : {(formatPoint(data.wholesellerDetail.salePoint))}</td>
</tr>
<tr>
  <td>Target Stock Mingguan : {(formatPoint(data.wholesellerDetail.baselineStock))}</td>
</tr>

<tr>
      <td>Kumulatif : {(formatPoint(data.wholesellerDetail.kumulatif))}</td>
</tr>

<tr>
  <td>Last Update : {data.wholesellerDetail.latestUpdate.replace("T00:00:00", "")}</td>
</tr>
</div>
      </Modal.Body>
      <Modal.Footer>
        <Button onClick={props.onHide}>Close</Button>
      </Modal.Footer>
    </Modal>
  );
}




export default PopupSalesman;


