import axios from 'axios';
import React from 'react';
import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
import { Link } from 'react-router-dom';

const PopupReset= (props : any) => {

  return (
    <Modal
      {...props}
      size="xl"
      aria-labelledby="contained-modal-title-vcenter"
      centered
      
    >
      <Modal.Header closeButton>
        <Modal.Title id="contained-modal-title-vcenter">
            Notifikasi!
        </Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <p>
         Reset Password berhasil, silahkan masukkan link dibawah ini untuk mengganti password??
        </p>

  <div>
<tr>
    <br />
    {/*Jika tidak open new tab  */}
    <td><a href={props.url}>{props.url}</a></td>
    {/* Jika open new tab */}
    {/* <a href={props.url} target="_blank"><td>{props.url}</td></a> */}
</tr>
</div>
      </Modal.Body>
      <Modal.Footer>
        <Button onClick={props.onHide}>Close</Button>
      </Modal.Footer>
    </Modal>
  );
}

export default PopupReset;


