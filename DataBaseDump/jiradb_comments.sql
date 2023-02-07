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
-- Table structure for table `comments`
--

DROP TABLE IF EXISTS `comments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `comments` (
  `Id` varchar(255) NOT NULL,
  `Description` longtext DEFAULT NULL,
  `Date_Posted` datetime(6) NOT NULL,
  `User_Id` varchar(255) DEFAULT NULL,
  `Task_Id` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Comments_Task_Id` (`Task_Id`),
  KEY `IX_Comments_User_Id` (`User_Id`),
  CONSTRAINT `FK_Comments_AspNetUsers_User_Id` FOREIGN KEY (`User_Id`) REFERENCES `aspnetusers` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_Comments_Tasks_Task_Id` FOREIGN KEY (`Task_Id`) REFERENCES `tasks` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `comments`
--

LOCK TABLES `comments` WRITE;
/*!40000 ALTER TABLE `comments` DISABLE KEYS */;
INSERT INTO `comments` VALUES ('40a07990-abbc-47bb-92d3-098ddd715b74','O sa caut eu maine cand merg la piata.','2022-02-04 17:10:33.229399','ab5ca209-9232-41db-bfe7-2c4e3360ccb4','db663ef2-6b62-46c7-813b-e17308b40143'),('42f77514-420c-4360-b326-cfc4886758eb','Voi mai merge din nou la o a -2 a consultatie.','2022-02-04 17:19:34.212874','ab5ca209-9232-41db-bfe7-2c4e3360ccb4','764a3dfb-6be1-4ba7-bfdd-8c4a524af8ee'),('5f723bda-5ae2-4e77-9e77-6b14f94c9f1e','Am sters praful si in sufragerie.','2022-02-04 17:18:59.347928','ab5ca209-9232-41db-bfe7-2c4e3360ccb4','f96888fa-1ac2-420e-a4ee-dbec019cdebe'),('75b60bf6-9e61-4d4c-ac1f-5fdca986bf26','Sa cumperi si portocale, te rog.','2022-02-04 17:08:29.824186','207b8cad-5789-40d1-99ac-2b866acd6dba','81cc0759-7aaa-4072-a454-95b5e78d79ab'),('8db96f2a-0be5-4750-9f4c-5eecf909d9de','Trebuie sa repar mai intai masina de tuns iarba.','2022-02-04 17:21:42.852701','4721d173-b61f-4d11-aed1-a533dea92258','9e829a6b-1398-46a1-a2ee-ad909ba7d9a6'),('a04729e8-277e-4839-b569-a49f4b6ced43','Nu am gasit lapte.','2022-02-04 17:08:02.623023','207b8cad-5789-40d1-99ac-2b866acd6dba','db663ef2-6b62-46c7-813b-e17308b40143'),('b29f6797-b5d6-4d59-a84f-318d4f3f63e2','Am terminat de aspirat dormitorul. A mai ramas bucataria.','2022-02-04 17:07:10.308673','9beabf24-632c-4087-a988-ba89e7afb510','9263b736-d42a-4d45-8a0d-b91520f591fc'),('cebdd471-5840-4ab7-a225-fc301c2f5910','Sigur.','2022-02-04 17:10:01.348843','ab5ca209-9232-41db-bfe7-2c4e3360ccb4','81cc0759-7aaa-4072-a454-95b5e78d79ab');
/*!40000 ALTER TABLE `comments` ENABLE KEYS */;
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
