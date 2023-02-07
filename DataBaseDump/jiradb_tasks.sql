-- MySQL dump 10.13  Distrib 8.0.18, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: jiradb
-- ------------------------------------------------------
-- Server version	5.5.5-10.4.13-MariaDB

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `tasks`
--

DROP TABLE IF EXISTS `tasks`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tasks` (
  `Id` varchar(255) NOT NULL,
  `Name` longtext DEFAULT NULL,
  `Description` longtext DEFAULT NULL,
  `Status` longtext DEFAULT NULL,
  `User_Id` varchar(255) DEFAULT NULL,
  `Sprint_Id` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Tasks_User_Id` (`User_Id`),
  KEY `IX_Tasks_Sprint_Id` (`Sprint_Id`),
  CONSTRAINT `FK_Tasks_AspNetUsers_User_Id` FOREIGN KEY (`User_Id`) REFERENCES `aspnetusers` (`Id`),
  CONSTRAINT `FK_Tasks_Sprints_Sprint_Id` FOREIGN KEY (`Sprint_Id`) REFERENCES `sprints` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tasks`
--

LOCK TABLES `tasks` WRITE;
/*!40000 ALTER TABLE `tasks` DISABLE KEYS */;
INSERT INTO `tasks` VALUES ('2688d9ec-a17c-473d-bc43-1873492d383e','Litiera pisici','Curatarea si schimbarea nisipului din litiera','ToDo','9beabf24-632c-4087-a988-ba89e7afb510','f0e13a96-530c-483e-912a-3a42c66e71f5'),('2a689aa2-50c9-4641-be8c-22d3d44289d3','Masina de tuns iarba','De reparat masina de tuns iarba','ToDo','4721d173-b61f-4d11-aed1-a533dea92258','9c8358b3-4e96-4e20-8bee-cc259adf6a05'),('42ed3148-0d2d-4e1d-9402-84dd006012e8','Cumparat apa','De cumparat apa din supermarket','Done','4721d173-b61f-4d11-aed1-a533dea92258','7bbddc91-c734-4811-a9c9-63cefc601730'),('764a3dfb-6be1-4ba7-bfdd-8c4a524af8ee','Veterinar','Mers la veterinar cu catelul','InProgress','ab5ca209-9232-41db-bfe7-2c4e3360ccb4','f0e13a96-530c-483e-912a-3a42c66e71f5'),('81cc0759-7aaa-4072-a454-95b5e78d79ab','Cumparaturi din piata','De cumparat legume si fructe','ToDo','ab5ca209-9232-41db-bfe7-2c4e3360ccb4','7bbddc91-c734-4811-a9c9-63cefc601730'),('9263b736-d42a-4d45-8a0d-b91520f591fc','De aspirat','Aspirat in sufragerie si bucatarie','InProgress','9beabf24-632c-4087-a988-ba89e7afb510','9c8358b3-4e96-4e20-8bee-cc259adf6a05'),('9e829a6b-1398-46a1-a2ee-ad909ba7d9a6','Tuns iarba','Tunsa iarba in curtea din spate','ToDo','4721d173-b61f-4d11-aed1-a533dea92258','9c8358b3-4e96-4e20-8bee-cc259adf6a05'),('bb76d652-b781-4ebc-ab23-eb320f599cc3','Hrana pisici','De cumparat hrana de la Pet Shop','Done','207b8cad-5789-40d1-99ac-2b866acd6dba','f0e13a96-530c-483e-912a-3a42c66e71f5'),('db663ef2-6b62-46c7-813b-e17308b40143','Tort','De cumparat ingrediente pentru tortul de ciocolata','InProgress','207b8cad-5789-40d1-99ac-2b866acd6dba','7bbddc91-c734-4811-a9c9-63cefc601730'),('f96888fa-1ac2-420e-a4ee-dbec019cdebe','Sters praful','De sters praful pe mobila din dormitor','Done','ab5ca209-9232-41db-bfe7-2c4e3360ccb4','9c8358b3-4e96-4e20-8bee-cc259adf6a05');
/*!40000 ALTER TABLE `tasks` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2022-02-04 17:38:30
