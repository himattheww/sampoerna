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
-- Dumping data for table `employees`
--

INSERT INTO `employees` (`employee_code`, `role_type`, `employee_name`, `employee_email`, `employee_phone`, `employee_area`, `employee_password`, `first_login`, `password_issued`, `change_token`) VALUES
('AM0006', 'A', 'Jali', 'jali@mail.com', '0852123456', NULL, '$2a$11$/9giFUg66nev6j56rIF8YObTFsiehCAxTkfaeU4yM.5amvdlDoNcu', b'1', b'0', NULL),
('SM0005', 'S', 'Sadi', 'sadi@mail.com', '0813123459', NULL, '$2a$11$5emB9p.4cx8dOXGamMz35Ohx26G4n4xGQcaengnOZZpUjY8bttIrW', b'1', b'0', NULL),
('SM0010', 'S', 'Wali', 'wati@mail.com', '0897123456', NULL, '$2a$11$PY8dsYIXBlu/6cyTbv52fuzJwt7/uJ80w5wql9E/UYCBA/fXBlQEa', b'1', b'0', NULL),
('WS0001', 'W', 'Yati', 'yati@mail.com', '0813123456', 'Malang', '$2a$11$GDiglh/eK2QsaLCsaVfsEe54mxRq7MFKCHOGKaY6UpwoVQjg83i/q', b'0', b'1', 'jLDfMioFbEWc3gKTwhkflg=='),
('WS0002', 'W', 'Wati', 'wati@mail.com', '0813123457', 'Tegal', '$2a$11$nvQjUCf.ZSsjsO8XkQENgu64QP9b0w5PtlMHluVGiK7WyhHGMVGFm', b'1', b'0', NULL),
('WS0003', 'W', 'Sari', 'sari@mail.com', '0813123458', 'Majalengka', '$2a$11$TLsZE5IPhuXPdkuH6u36YeSiJ7s3cW0l9MhLL95tWk.wwgi1JiLFy', b'1', b'0', NULL),
('WS0007', 'W', 'Jati', 'jati@mail.com', '0852123457', 'Depok', '$2a$11$nWxUdpFt2dC3.EZOsgwvb.uBhPGhXINOZCQp/tOlZgH8yYJqAGmZu', b'1', b'0', NULL),
('WS0008', 'W', 'Zaki', 'zaki@mail.com', '0852123458', 'Bekasi', '$2a$11$bFgtSshk0MSfrYYZbL5Ul.I7m4broiByDgshc.iwcubVWWKlQWh7e', b'1', b'0', NULL),
('WS0009', 'W', 'Zadi', 'zadi@mail.com', '0852123459', 'Cirebon', '$2a$11$1qN831Jpw.Z6wsheeWqj0e7zmROkugE0e4TchmLYsKnM9vJtWbYtO', b'1', b'0', NULL);

-- --------------------------------------------------------

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
-- Dumping data for table `rounds`
--

INSERT INTO `rounds` (`round_id`, `round_name`, `start_date`, `end_date`) VALUES
(1, 'Round 1', '2023-01-01', '2023-02-28'),
(2, 'Round 2', '2023-03-01', '2023-06-30'),
(3, 'Round 3', '2023-07-01', '2023-09-30'),
(4, 'Grand', '2023-10-01', '2023-12-31');

-- --------------------------------------------------------

--
-- Table structure for table `wholeseller_mapping`
--

CREATE TABLE `wholeseller_mapping` (
  `wholeseller_code` varchar(50) NOT NULL,
  `salesman_code` varchar(50) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `wholeseller_mapping`
--

INSERT INTO `wholeseller_mapping` (`wholeseller_code`, `salesman_code`) VALUES
('WS0001', 'SM0005'),
('WS0002', 'SM0005'),
('WS0003', 'SM0010'),
('WS0007', 'SM0010'),
('WS0008', 'SM0005'),
('WS0009', 'SM0010');

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
