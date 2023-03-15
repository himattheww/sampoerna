-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jan 02, 2023 at 11:10 AM
-- Server version: 10.4.27-MariaDB
-- PHP Version: 8.1.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `leaderboardhms`
--

CREATE DATABASE `leaderboardhms`;

-- --------------------------------------------------------

--
-- Table structure for table `employees`
--

CREATE TABLE `employees` (
  `employee_code` varchar(50) NOT NULL,
  `role_type` varchar(1) NOT NULL,
  `employee_name` varchar(256) NOT NULL,
  `employee_email` varchar(256) NOT NULL,
  `employee_phone` varchar(256) NOT NULL,
  `employee_area` varchar(256) DEFAULT NULL,
  `employee_password` varchar(256) NOT NULL,
  `first_login` bit(1) NOT NULL DEFAULT b'1',
  `password_issued` bit(1) NOT NULL DEFAULT b'0',
  `change_token` varchar(256) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Table structure for table `leaderboards`
--

CREATE TABLE `leaderboards` (
  `wholeseller_code` varchar(50) NOT NULL,
  `round_id` int(11) NOT NULL,
  `group_name` varchar(50) NOT NULL,
  `baseline_stock` int(11) NOT NULL,
  `sale_date` date NOT NULL DEFAULT current_timestamp(),
  `sale_point` int(11) NOT NULL,
  `rank` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `rounds`
--

CREATE TABLE `rounds` (
  `round_id` int(11) NOT NULL,
  `round_name` varchar(50) NOT NULL,
  `start_date` date NOT NULL,
  `end_date` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;


--
-- Table structure for table `wholeseller_mapping`
--

CREATE TABLE `wholeseller_mapping` (
  `wholeseller_code` varchar(50) NOT NULL,
  `salesman_code` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `employees`
--
ALTER TABLE `employees`
  ADD PRIMARY KEY (`employee_code`);

--
-- Indexes for table `leaderboards`
--
ALTER TABLE `leaderboards`
  ADD PRIMARY KEY (`wholeseller_code`,`round_id`);

--
-- Indexes for table `rounds`
--
ALTER TABLE `rounds`
  ADD PRIMARY KEY (`round_id`);

--
-- Indexes for table `wholeseller_mapping`
--
ALTER TABLE `wholeseller_mapping`
  ADD PRIMARY KEY (`wholeseller_code`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;