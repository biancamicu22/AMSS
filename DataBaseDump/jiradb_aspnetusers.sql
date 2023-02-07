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
-- Table structure for table `aspnetusers`
--

DROP TABLE IF EXISTS `aspnetusers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `aspnetusers` (
  `Id` varchar(255) NOT NULL,
  `Active` tinyint(1) NOT NULL,
  `Birthdate` datetime(6) DEFAULT NULL,
  `FirstName` longtext DEFAULT NULL,
  `LastName` longtext DEFAULT NULL,
  `Roles` longtext DEFAULT NULL,
  `Description` longtext DEFAULT NULL,
  `UserName` varchar(256) DEFAULT NULL,
  `NormalizedUserName` varchar(256) DEFAULT NULL,
  `Email` varchar(256) DEFAULT NULL,
  `NormalizedEmail` varchar(256) DEFAULT NULL,
  `EmailConfirmed` tinyint(1) NOT NULL,
  `PasswordHash` longtext DEFAULT NULL,
  `SecurityStamp` longtext DEFAULT NULL,
  `ConcurrencyStamp` longtext DEFAULT NULL,
  `PhoneNumber` longtext DEFAULT NULL,
  `PhoneNumberConfirmed` tinyint(1) NOT NULL,
  `TwoFactorEnabled` tinyint(1) NOT NULL,
  `LockoutEnd` datetime(6) DEFAULT NULL,
  `LockoutEnabled` tinyint(1) NOT NULL,
  `AccessFailedCount` int(11) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `UserNameIndex` (`NormalizedUserName`),
  KEY `EmailIndex` (`NormalizedEmail`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `aspnetusers`
--

LOCK TABLES `aspnetusers` WRITE;
/*!40000 ALTER TABLE `aspnetusers` DISABLE KEYS */;
INSERT INTO `aspnetusers` VALUES ('207b8cad-5789-40d1-99ac-2b866acd6dba',0,'1997-06-27 00:00:00.000000','Clara','Vasile','USER',NULL,'Clara','CLARA','claragabrielav@gmail.com','CLARAGABRIELAV@GMAIL.COM',0,'AQAAAAEAACcQAAAAEHHdzHRwSXTP4wWFQtN+6Qnh1ozq7uTFXcK0yA89kUtirUzMKso45Fa9160mbiKrBw==','IGFL45E45XCBSE4FAB62K6KS5N2XWKEG','85f286e2-430a-4c96-8a09-39a3efa005a2',NULL,0,0,NULL,1,0),('4721d173-b61f-4d11-aed1-a533dea92258',0,'1998-01-16 00:00:00.000000','Ionut','Gheorghe','USER',NULL,'Ionut','IONUT','gheorghei65@yahoo.com','GHEORGHEI65@YAHOO.COM',0,'AQAAAAEAACcQAAAAEMg0ei84VBrUjAA+i82iQxvm5rsclP9xN9x72oo4pxGFM62C2jLQkF09IOxIMWve8Q==','5U56WBGLTT6N2J6G4FWK2HKXQNJ6L4BZ','81529f3a-71ad-4935-9ad8-ce993917a37d',NULL,0,0,NULL,1,0),('9beabf24-632c-4087-a988-ba89e7afb510',0,'1997-05-15 00:00:00.000000','Razvan','Dragusin','USER',NULL,'Razvan','RAZVAN','dragusin.razvan18@gmail.com','DRAGUSIN.RAZVAN18@GMAIL.COM',0,'AQAAAAEAACcQAAAAECwISsy68cSHXppnw7Dl84Y1rj+duQ52BojmpZa5c/Gd3SerUnELWVu33UpttezhYw==','MRZN6CABSVQIYWAMQQKUXUYIGJDDNXLX','078ebe8d-a7e7-437b-856b-b12eb158b6bc',NULL,0,0,NULL,1,0),('ab5ca209-9232-41db-bfe7-2c4e3360ccb4',0,'1992-12-19 00:00:00.000000','Stefan','Mesteacan','USER',NULL,'Stefan','STEFAN','stefanmesteacan@gmail.com','STEFANMESTEACAN@GMAIL.COM',0,'AQAAAAEAACcQAAAAEP/L/Yg+sXdUOJc6VZQSnAJhZ66EOVlNvoTYzkb5koGZxQIDdufTTAhk/v86yNE+wA==','FGG4FKHQ56YCWVZQR2B3PYYMIVBDWTNL','18d08c24-90f8-4ac1-b214-146f98812b82',NULL,0,0,NULL,1,0);
/*!40000 ALTER TABLE `aspnetusers` ENABLE KEYS */;
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
