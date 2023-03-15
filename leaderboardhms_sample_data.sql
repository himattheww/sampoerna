-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Jan 02, 2023 at 07:24 PM
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
-- Dumping data for table `leaderboards`
--

INSERT INTO `leaderboards` (`wholeseller_code`, `round_id`, `group_name`, `baseline_stock`, `sale_date`, `sale_point`, `rank`) VALUES
('WS0001', 1, 'A123', 200000, '2023-01-03', 500, 3),
('WS0002', 1, 'B345', 120000, '2023-01-03', 600, 2),
('WS0003', 1, 'A123', 150000, '2023-01-03', 800, 2),
('WS0007', 1, 'B345', 300000, '2023-01-03', 200, 3),
('WS0008', 1, 'B345', 210000, '2023-01-03', 700, 1),
('WS0009', 1, 'A123', 180000, '2023-01-03', 810, 1);

-- --------------------------------------------------------

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