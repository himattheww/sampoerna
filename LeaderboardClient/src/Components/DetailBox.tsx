import React from "react";
import { Box } from "@mui/material";

const DetailBox = () => {
  const data = {
    employeeName: localStorage.getItem("employeeName"),
    wholesellerDetail: JSON.parse(localStorage.getItem("wholesellerDetail") || '{"pointA":"senpai","pointB":"senpai","pointC":"senpai"}'),
  };

  return (
    <>
      <Box
        sx={{
          backgroundColor: "warning.main",
          color: "black",
          height: "280px",
          width: "300px",
          padding: "22px",
          "&:hover": {
            backgroundColor: "warning.light",
          },
        }}
      >
       

        <tr>
          <td>Wholesaler Name : {data.employeeName}</td>
        </tr>

        <tr>
          <td>Point A : {data.wholesellerDetail.pointA}  </td>
        </tr>
        <tr>
          <td>Point B : {data.wholesellerDetail.pointB}</td>
        </tr>
        <tr>
          <td>Point C : {data.wholesellerDetail.pointC}</td>
        </tr>

        <tr>
          <td>Total Point : {data.wholesellerDetail.salePoint}</td>
        </tr>

        <tr>
          <td>Last Update : {data.wholesellerDetail.latestUpdate.replace("T00:00:00", "")}</td>
        </tr>
      </Box>
    </>
  );
};

export default DetailBox;
