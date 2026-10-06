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
-- Table structure for table `content_block`
--

DROP TABLE IF EXISTS `content_block`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `content_block` (
  `block_id` binary(16) NOT NULL DEFAULT (uuid_to_bin(uuid(),1)),
  `version_id` binary(16) NOT NULL,
  `block_type_id` int NOT NULL,
  `content_data` text,
  `sequence_order` int NOT NULL,
  PRIMARY KEY (`block_id`),
  KEY `version_id` (`version_id`),
  KEY `block_type_id` (`block_type_id`),
  CONSTRAINT `content_block_ibfk_1` FOREIGN KEY (`version_id`) REFERENCES `article_version` (`version_id`) ON DELETE CASCADE,
  CONSTRAINT `content_block_ibfk_2` FOREIGN KEY (`block_type_id`) REFERENCES `block_type` (`block_type_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `content_block`
--

LOCK TABLES `content_block` WRITE;
/*!40000 ALTER TABLE `content_block` DISABLE KEYS */;
INSERT INTO `content_block` VALUES (_binary 'Òêe\Àzë†z\0]©Qˆ',_binary 'ªªªªªªªªªªªªªªªª',1,'SQL stands for Structured Query Language.',1),(_binary 'ÒêeÀÄ\‚†z\0]©Qˆ',_binary 'ªªªªªªªªªªªªªªªª',3,'SELECT * FROM users;',2),(_binary 'Òêe\ŸS≠†z\0]©Qˆ',_binary '\›\›\›\›\›\›\›\›\›\›\›\›\›\›\›\›',1,'Indexes improve database query performance by reducing the amount of data scanned.',1),(_binary 'Òêe\ŸZ˛†z\0]©Qˆ',_binary '\›\›\›\›\›\›\›\›\›\›\›\›\›\›\›\›',1,'However, indexes also increase storage usage and slightly slow INSERT, UPDATE, and DELETE operations.',2),(_binary 'Òêe\Ÿ\\d†z\0]©Qˆ',_binary '\›\›\›\›\›\›\›\›\›\›\›\›\›\›\›\›',3,'CREATE INDEX idx_username ON users(username);',3),(_binary 'Òêe\Â’†z\0]©Qˆ',_binary 'ˇˇˇˇˇˇˇˇˇˇˇˇˇˇˇˇ',1,'JOIN combines rows from two or more tables based on a related column.',1),(_binary 'Òêe\Â!w†z\0]©Qˆ',_binary 'ˇˇˇˇˇˇˇˇˇˇˇˇˇˇˇˇ',1,'Common JOIN types include INNER JOIN, LEFT JOIN, RIGHT JOIN, and FULL OUTER JOIN.',2),(_binary 'Òêe\Â\"\·†z\0]©Qˆ',_binary 'ˇˇˇˇˇˇˇˇˇˇˇˇˇˇˇˇ',3,'SELECT u.username, r.role_name FROM user u INNER JOIN user_role ur ON u.user_id = ur.user_id INNER JOIN role r ON ur.role_id = r.role_id;',3),(_binary 'Ò†G})~§§|£*\≈_',_binary 'Ò†G{±K§|£*\≈_',1,'A transaction in SQL ensures the All-or-Nothing rule applies to database operations.',1),(_binary 'Ò†G~\€\“§|£*\≈_',_binary 'Ò†G{±K§|£*\≈_',2,'START TRANSACTION;\nUPDATE ARTICLE_VERSION SET status_id = 5;\nCOMMIT;',2);
/*!40000 ALTER TABLE `content_block` ENABLE KEYS */;
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
