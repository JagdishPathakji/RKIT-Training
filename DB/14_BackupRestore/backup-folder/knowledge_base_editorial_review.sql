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
-- Table structure for table `editorial_review`
--

DROP TABLE IF EXISTS `editorial_review`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `editorial_review` (
  `review_id` binary(16) NOT NULL DEFAULT (uuid_to_bin(uuid(),1)),
  `version_id` binary(16) NOT NULL,
  `editor_id` binary(16) NOT NULL,
  `decision` enum('Approve','Reject','Suggest Improvements') NOT NULL,
  `feedback` text NOT NULL,
  `reviewed_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`review_id`),
  KEY `version_id` (`version_id`),
  KEY `editor_id` (`editor_id`),
  CONSTRAINT `editorial_review_ibfk_1` FOREIGN KEY (`version_id`) REFERENCES `article_version` (`version_id`) ON DELETE CASCADE,
  CONSTRAINT `editorial_review_ibfk_2` FOREIGN KEY (`editor_id`) REFERENCES `user` (`user_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `editorial_review`
--

LOCK TABLES `editorial_review` WRITE;
/*!40000 ALTER TABLE `editorial_review` DISABLE KEYS */;
INSERT INTO `editorial_review` VALUES (_binary 'Òêe\—:†z\0]©Qˆ',_binary 'ªªªªªªªªªªªªªªªª',_binary '\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"','Approve','Excellent tutorial. Ready to go live!','2026-08-05 17:09:19'),(_binary 'Òêe\›\ÿ†z\0]©Qˆ',_binary '\›\›\›\›\›\›\›\›\›\›\›\›\›\›\›\›',_binary '\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"','Approve','Well structured explanation with useful SQL example.','2026-08-05 17:09:20'),(_binary 'Òêe\Í`¡†z\0]©Qˆ',_binary 'ˇˇˇˇˇˇˇˇˇˇˇˇˇˇˇˇ',_binary '\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"','Approve','Clear introduction with practical SQL example.','2026-08-05 17:09:20'),(_binary 'Ò†Gâ›Ñ\È§|£*\≈_',_binary 'Ò†G{±K§|£*\≈_',_binary 'Ò†GTl\ﬁI§|£*\≈_','Approve','Excellent article, perfectly explained!','2026-08-25 05:40:54');
/*!40000 ALTER TABLE `editorial_review` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-09-02 19:33:47
