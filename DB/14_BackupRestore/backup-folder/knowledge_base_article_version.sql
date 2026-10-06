-- MySQL dump 10.13  Distrib 8.0.46, for Win64 (x86_64)
--
-- Host: localhost    Database: knowledge_base
-- ------------------------------------------------------
-- Server version	8.0.46

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
-- Table structure for table `article_version`
--

DROP TABLE IF EXISTS `article_version`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `article_version` (
  `version_id` binary(16) NOT NULL DEFAULT (uuid_to_bin(uuid(),1)),
  `article_id` binary(16) NOT NULL,
  `version_number` int NOT NULL,
  `title` varchar(255) NOT NULL,
  `status_id` int NOT NULL,
  `created_by` binary(16) NOT NULL,
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`version_id`),
  KEY `article_id` (`article_id`),
  KEY `created_by` (`created_by`),
  KEY `idx_status_date` (`status_id`,`created_at`),
  CONSTRAINT `article_version_ibfk_1` FOREIGN KEY (`article_id`) REFERENCES `article` (`article_id`) ON DELETE CASCADE,
  CONSTRAINT `article_version_ibfk_2` FOREIGN KEY (`status_id`) REFERENCES `status` (`status_id`),
  CONSTRAINT `article_version_ibfk_3` FOREIGN KEY (`created_by`) REFERENCES `user` (`user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `article_version`
--

LOCK TABLES `article_version` WRITE;
/*!40000 ALTER TABLE `article_version` DISABLE KEYS */;
INSERT INTO `article_version` VALUES (_binary 'Ò†G{±K§|£*\≈_',_binary 'Ò†Gx\ÌD@§|£*\≈_',1,'How to use Transactions in SQL',5,_binary 'Ò†GtÉ‰Å§|£*\≈_','2026-08-25 05:40:29'),(_binary 'ªªªªªªªªªªªªªªªª',_binary '™™™™™™™™™™™™™™™™',1,'My Awesome SQL Guide',5,_binary '','2026-08-05 17:09:19'),(_binary '\›\›\›\›\›\›\›\›\›\›\›\›\›\›\›\›',_binary '\Ã\Ã\Ã\Ã\Ã\Ã\Ã\Ã\Ã\Ã\Ã\Ã\Ã\Ã\Ã\Ã',1,'Understanding Database Indexes',5,_binary '','2026-08-05 17:09:20'),(_binary 'ˇˇˇˇˇˇˇˇˇˇˇˇˇˇˇˇ',_binary '\Ó\Ó\Ó\Ó\Ó\Ó\Ó\Ó\Ó\Ó\Ó\Ó\Ó\Ó\Ó\Ó',1,'Getting Started with SQL JOINs',5,_binary '','2026-08-05 17:09:20');
/*!40000 ALTER TABLE `article_version` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-09-02 19:33:45
