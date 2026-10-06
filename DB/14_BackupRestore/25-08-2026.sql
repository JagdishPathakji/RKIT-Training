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
-- Table structure for table `article`
--

DROP TABLE IF EXISTS `article`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `article` (
  `article_id` binary(16) NOT NULL DEFAULT (uuid_to_bin(uuid(),1)),
  `author_id` binary(16) NOT NULL,
  `current_published_version_id` binary(16) DEFAULT NULL,
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`article_id`),
  KEY `author_id` (`author_id`),
  KEY `fk_current_published_version` (`current_published_version_id`),
  CONSTRAINT `article_ibfk_1` FOREIGN KEY (`author_id`) REFERENCES `user` (`user_id`),
  CONSTRAINT `fk_current_published_version` FOREIGN KEY (`current_published_version_id`) REFERENCES `article_version` (`version_id`) ON DELETE SET NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `article`
--

LOCK TABLES `article` WRITE;
/*!40000 ALTER TABLE `article` DISABLE KEYS */;
INSERT INTO `article` VALUES (_binary '�Gx\�D@�|�*\�_',_binary '�Gt�䁤|�*\�_',_binary '�G{�K�|�*\�_','2026-08-25 05:40:25'),(_binary '����������������',_binary '',_binary '����������������','2026-08-05 17:09:19'),(_binary '\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�',_binary '',_binary '\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�','2026-08-05 17:09:20'),(_binary '\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�',_binary '',_binary '����������������','2026-08-05 17:09:20');
/*!40000 ALTER TABLE `article` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `article_category`
--

DROP TABLE IF EXISTS `article_category`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `article_category` (
  `article_id` binary(16) NOT NULL,
  `category_id` int NOT NULL,
  PRIMARY KEY (`article_id`,`category_id`),
  KEY `category_id` (`category_id`),
  CONSTRAINT `article_category_ibfk_1` FOREIGN KEY (`article_id`) REFERENCES `article` (`article_id`) ON DELETE CASCADE,
  CONSTRAINT `article_category_ibfk_2` FOREIGN KEY (`category_id`) REFERENCES `category` (`category_id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `article_category`
--

LOCK TABLES `article_category` WRITE;
/*!40000 ALTER TABLE `article_category` DISABLE KEYS */;
INSERT INTO `article_category` VALUES (_binary '����������������',1),(_binary '\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�',1),(_binary '\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�',1);
/*!40000 ALTER TABLE `article_category` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `article_tag`
--

DROP TABLE IF EXISTS `article_tag`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `article_tag` (
  `article_id` binary(16) NOT NULL,
  `tag_id` int NOT NULL,
  PRIMARY KEY (`article_id`,`tag_id`),
  KEY `tag_id` (`tag_id`),
  CONSTRAINT `article_tag_ibfk_1` FOREIGN KEY (`article_id`) REFERENCES `article` (`article_id`) ON DELETE CASCADE,
  CONSTRAINT `article_tag_ibfk_2` FOREIGN KEY (`tag_id`) REFERENCES `tag` (`tag_id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `article_tag`
--

LOCK TABLES `article_tag` WRITE;
/*!40000 ALTER TABLE `article_tag` DISABLE KEYS */;
INSERT INTO `article_tag` VALUES (_binary '����������������',1),(_binary '\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�',2),(_binary '\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�',3);
/*!40000 ALTER TABLE `article_tag` ENABLE KEYS */;
UNLOCK TABLES;

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
  KEY `status_id` (`status_id`),
  KEY `created_by` (`created_by`),
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
INSERT INTO `article_version` VALUES (_binary '�G{�K�|�*\�_',_binary '�Gx\�D@�|�*\�_',1,'How to use Transactions in SQL',5,_binary '�Gt�䁤|�*\�_','2026-08-25 05:40:29'),(_binary '����������������',_binary '����������������',1,'My Awesome SQL Guide',5,_binary '','2026-08-05 17:09:19'),(_binary '\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�',_binary '\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�',1,'Understanding Database Indexes',5,_binary '','2026-08-05 17:09:20'),(_binary '����������������',_binary '\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�',1,'Getting Started with SQL JOINs',5,_binary '','2026-08-05 17:09:20');
/*!40000 ALTER TABLE `article_version` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `block_type`
--

DROP TABLE IF EXISTS `block_type`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `block_type` (
  `block_type_id` int NOT NULL,
  `type_name` varchar(50) NOT NULL,
  PRIMARY KEY (`block_type_id`),
  UNIQUE KEY `type_name` (`type_name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `block_type`
--

LOCK TABLES `block_type` WRITE;
/*!40000 ALTER TABLE `block_type` DISABLE KEYS */;
INSERT INTO `block_type` VALUES (2,'Code'),(3,'Image'),(1,'Text');
/*!40000 ALTER TABLE `block_type` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `category`
--

DROP TABLE IF EXISTS `category`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `category` (
  `category_id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(100) NOT NULL,
  PRIMARY KEY (`category_id`),
  UNIQUE KEY `name` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `category`
--

LOCK TABLES `category` WRITE;
/*!40000 ALTER TABLE `category` DISABLE KEYS */;
INSERT INTO `category` VALUES (6,'Company Operations'),(5,'Customer Support'),(1,'Engineering & Technology'),(3,'Human Resources'),(2,'Product & Design'),(4,'Sales & Marketing');
/*!40000 ALTER TABLE `category` ENABLE KEYS */;
UNLOCK TABLES;

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
INSERT INTO `content_block` VALUES (_binary '��e\�z��z\0]�Q�',_binary '����������������',1,'SQL stands for Structured Query Language.',1),(_binary '��eˀ\�z\0]�Q�',_binary '����������������',3,'SELECT * FROM users;',2),(_binary '��e\�S��z\0]�Q�',_binary '\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�',1,'Indexes improve database query performance by reducing the amount of data scanned.',1),(_binary '��e\�Z��z\0]�Q�',_binary '\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�',1,'However, indexes also increase storage usage and slightly slow INSERT, UPDATE, and DELETE operations.',2),(_binary '��e\�\\d�z\0]�Q�',_binary '\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�',3,'CREATE INDEX idx_username ON users(username);',3),(_binary '��e\�ՠz\0]�Q�',_binary '����������������',1,'JOIN combines rows from two or more tables based on a related column.',1),(_binary '��e\�!w�z\0]�Q�',_binary '����������������',1,'Common JOIN types include INNER JOIN, LEFT JOIN, RIGHT JOIN, and FULL OUTER JOIN.',2),(_binary '��e\�\"\�z\0]�Q�',_binary '����������������',3,'SELECT u.username, r.role_name FROM user u INNER JOIN user_role ur ON u.user_id = ur.user_id INNER JOIN role r ON ur.role_id = r.role_id;',3),(_binary '�G})~��|�*\�_',_binary '�G{�K�|�*\�_',1,'A transaction in SQL ensures the All-or-Nothing rule applies to database operations.',1),(_binary '�G~\�\��|�*\�_',_binary '�G{�K�|�*\�_',2,'START TRANSACTION;\nUPDATE ARTICLE_VERSION SET status_id = 5;\nCOMMIT;',2);
/*!40000 ALTER TABLE `content_block` ENABLE KEYS */;
UNLOCK TABLES;

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
INSERT INTO `editorial_review` VALUES (_binary '��e\�:�z\0]�Q�',_binary '����������������',_binary '\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"','Approve','Excellent tutorial. Ready to go live!','2026-08-05 17:09:19'),(_binary '��e\�\��z\0]�Q�',_binary '\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�',_binary '\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"','Approve','Well structured explanation with useful SQL example.','2026-08-05 17:09:20'),(_binary '��e\�`��z\0]�Q�',_binary '����������������',_binary '\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"','Approve','Clear introduction with practical SQL example.','2026-08-05 17:09:20'),(_binary '�G�݄\�|�*\�_',_binary '�G{�K�|�*\�_',_binary '�GTl\�I�|�*\�_','Approve','Excellent article, perfectly explained!','2026-08-25 05:40:54');
/*!40000 ALTER TABLE `editorial_review` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `role`
--

DROP TABLE IF EXISTS `role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `role` (
  `role_id` int NOT NULL,
  `role_name` varchar(50) NOT NULL,
  PRIMARY KEY (`role_id`),
  UNIQUE KEY `role_name` (`role_name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `role`
--

LOCK TABLES `role` WRITE;
/*!40000 ALTER TABLE `role` DISABLE KEYS */;
INSERT INTO `role` VALUES (2,'Author'),(3,'Editor'),(1,'Reviewer');
/*!40000 ALTER TABLE `role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `status`
--

DROP TABLE IF EXISTS `status`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `status` (
  `status_id` int NOT NULL,
  `status_name` varchar(50) NOT NULL,
  PRIMARY KEY (`status_id`),
  UNIQUE KEY `status_name` (`status_name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `status`
--

LOCK TABLES `status` WRITE;
/*!40000 ALTER TABLE `status` DISABLE KEYS */;
INSERT INTO `status` VALUES (1,'Draft'),(3,'Needs Improvement'),(2,'Pending Editor Review'),(5,'Published'),(4,'Rejected');
/*!40000 ALTER TABLE `status` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tag`
--

DROP TABLE IF EXISTS `tag`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tag` (
  `tag_id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(100) NOT NULL,
  PRIMARY KEY (`tag_id`),
  UNIQUE KEY `name` (`name`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tag`
--

LOCK TABLES `tag` WRITE;
/*!40000 ALTER TABLE `tag` DISABLE KEYS */;
INSERT INTO `tag` VALUES (2,'Indexing'),(3,'JOIN'),(1,'SQL');
/*!40000 ALTER TABLE `tag` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user`
--

DROP TABLE IF EXISTS `user`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user` (
  `user_id` binary(16) NOT NULL DEFAULT (uuid_to_bin(uuid(),1)),
  `username` varchar(100) NOT NULL,
  `email` varchar(255) NOT NULL,
  `password_hash` varchar(255) NOT NULL,
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `bio` text,
  PRIMARY KEY (`user_id`),
  UNIQUE KEY `username` (`username`),
  UNIQUE KEY `email` (`email`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user`
--

LOCK TABLES `user` WRITE;
/*!40000 ALTER TABLE `user` DISABLE KEYS */;
INSERT INTO `user` VALUES (_binary '','jagdish','jagdish@example.com','hash1','2026-08-05 17:09:19',NULL),(_binary '�GTl\�I�|�*\�_','editor_tirth','tirth@example.com','hashed_pw_123','2026-08-25 05:39:37',NULL),(_binary '�Gt�䁤|�*\�_','author_tirth','tirth_auth@example.com','hashed_pw_456','2026-08-25 05:40:18',NULL),(_binary '\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"','mihir','mihir@example.com','hash2','2026-08-05 17:09:19',NULL),(_binary '3333333333333333','rudra','rudra@example.com','hash3','2026-08-05 17:09:19',NULL);
/*!40000 ALTER TABLE `user` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_comment`
--

DROP TABLE IF EXISTS `user_comment`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user_comment` (
  `comment_id` binary(16) NOT NULL DEFAULT (uuid_to_bin(uuid(),1)),
  `article_id` binary(16) NOT NULL,
  `user_id` binary(16) NOT NULL,
  `comment_text` text NOT NULL,
  `created_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`comment_id`),
  KEY `article_id` (`article_id`),
  KEY `user_id` (`user_id`),
  CONSTRAINT `user_comment_ibfk_1` FOREIGN KEY (`article_id`) REFERENCES `article` (`article_id`) ON DELETE CASCADE,
  CONSTRAINT `user_comment_ibfk_2` FOREIGN KEY (`user_id`) REFERENCES `user` (`user_id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_comment`
--

LOCK TABLES `user_comment` WRITE;
/*!40000 ALTER TABLE `user_comment` DISABLE KEYS */;
INSERT INTO `user_comment` VALUES (_binary '��e\�Ų�z\0]�Q�',_binary '����������������',_binary '3333333333333333','This guide saved my life today, thank you Jagdish!','2026-08-05 17:09:20'),(_binary '��e\�\�o�z\0]�Q�',_binary '\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�',_binary '3333333333333333','Very helpful explanation of indexes and their trade-offs.','2026-08-05 17:09:20'),(_binary '��e\�?��z\0]�Q�',_binary '\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�',_binary '3333333333333333','Excellent beginner-friendly article on SQL JOINs.','2026-08-05 17:09:20');
/*!40000 ALTER TABLE `user_comment` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_rating`
--

DROP TABLE IF EXISTS `user_rating`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user_rating` (
  `article_id` binary(16) NOT NULL,
  `user_id` binary(16) NOT NULL,
  `rating_value` decimal(2,1) NOT NULL,
  `updated_at` timestamp NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`article_id`,`user_id`),
  KEY `user_id` (`user_id`),
  CONSTRAINT `user_rating_ibfk_1` FOREIGN KEY (`article_id`) REFERENCES `article` (`article_id`) ON DELETE CASCADE,
  CONSTRAINT `user_rating_ibfk_2` FOREIGN KEY (`user_id`) REFERENCES `user` (`user_id`) ON DELETE CASCADE,
  CONSTRAINT `user_rating_chk_1` CHECK (((`rating_value` >= 1) and (`rating_value` <= 5)))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_rating`
--

LOCK TABLES `user_rating` WRITE;
/*!40000 ALTER TABLE `user_rating` DISABLE KEYS */;
INSERT INTO `user_rating` VALUES (_binary '����������������',_binary '3333333333333333',5.0,'2026-08-05 17:09:20'),(_binary '\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�',_binary '3333333333333333',4.0,'2026-08-05 17:09:20'),(_binary '\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�\�',_binary '3333333333333333',5.0,'2026-08-05 17:09:20');
/*!40000 ALTER TABLE `user_rating` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_role`
--

DROP TABLE IF EXISTS `user_role`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user_role` (
  `user_id` binary(16) NOT NULL,
  `role_id` int NOT NULL,
  PRIMARY KEY (`user_id`,`role_id`),
  KEY `role_id` (`role_id`),
  CONSTRAINT `user_role_ibfk_1` FOREIGN KEY (`user_id`) REFERENCES `user` (`user_id`) ON DELETE CASCADE,
  CONSTRAINT `user_role_ibfk_2` FOREIGN KEY (`role_id`) REFERENCES `role` (`role_id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_role`
--

LOCK TABLES `user_role` WRITE;
/*!40000 ALTER TABLE `user_role` DISABLE KEYS */;
INSERT INTO `user_role` VALUES (_binary '3333333333333333',1),(_binary '',2),(_binary '�Gt�䁤|�*\�_',2),(_binary '�GTl\�I�|�*\�_',3),(_binary '\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"\"',3);
/*!40000 ALTER TABLE `user_role` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary view structure for view `vw_articlemetrics`
--

DROP TABLE IF EXISTS `vw_articlemetrics`;
/*!50001 DROP VIEW IF EXISTS `vw_articlemetrics`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `vw_articlemetrics` AS SELECT 
 1 AS `article_id`,
 1 AS `total_ratings`,
 1 AS `average_rating`,
 1 AS `total_comments`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `vw_publishedarticles`
--

DROP TABLE IF EXISTS `vw_publishedarticles`;
/*!50001 DROP VIEW IF EXISTS `vw_publishedarticles`*/;
SET @saved_cs_client     = @@character_set_client;
/*!50503 SET character_set_client = utf8mb4 */;
/*!50001 CREATE VIEW `vw_publishedarticles` AS SELECT 
 1 AS `article_id`,
 1 AS `version_id`,
 1 AS `title`,
 1 AS `author_name`,
 1 AS `published_date`*/;
SET character_set_client = @saved_cs_client;

--
-- Dumping events for database 'knowledge_base'
--

--
-- Dumping routines for database 'knowledge_base'
--
/*!50003 DROP FUNCTION IF EXISTS `fn_EstimateReadingTime` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` FUNCTION `fn_EstimateReadingTime`(p_version_id BINARY(16)) RETURNS int
    DETERMINISTIC
BEGIN
    DECLARE total_chars INT;
    DECLARE minutes_to_read INT;

    -- SUM of length of all 'TEXT' blocks for this specific article version
    SELECT 
        COALESCE(SUM(LENGTH(content_data)),0) INTO total_chars
    FROM CONTENT_BLOCK
    WHERE version_id = p_version_id
        AND
    block_type_id = (
        SELECT 
            block_type_id 
        FROM BLOCK_TYPE 
        WHERE type_name = 'Text'
    );

    -- assume average reading speed is 200 chars per minute
    SET minutes_to_read = CEILING(total_chars / 200);

    -- If less than 1 min, just return 1
    IF minutes_to_read = 0 THEN 
        SET minutes_to_read = 1;
    END IF;

    RETURN minutes_to_read;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!50003 DROP FUNCTION IF EXISTS `fn_GetAverageRating` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` FUNCTION `fn_GetAverageRating`(p_article_id BINARY(16)) RETURNS decimal(3,1)
    READS SQL DATA
BEGIN
    DECLARE avg_score DECIMAL(3,1);
    
    SELECT COALESCE(AVG(rating_value), 0.0) INTO avg_score
    FROM USER_RATING
    WHERE article_id = p_article_id;
    
    RETURN avg_score;
END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;

--
-- Final view structure for view `vw_articlemetrics`
--

/*!50001 DROP VIEW IF EXISTS `vw_articlemetrics`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `vw_articlemetrics` AS select `a`.`article_id` AS `article_id`,(select count(0) from `user_rating` where (`user_rating`.`article_id` = `a`.`article_id`)) AS `total_ratings`,(select coalesce(avg(`user_rating`.`rating_value`),0) from `user_rating` where (`user_rating`.`article_id` = `a`.`article_id`)) AS `average_rating`,(select count(0) from `user_comment` where (`user_comment`.`article_id` = `a`.`article_id`)) AS `total_comments` from `article` `a` */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `vw_publishedarticles`
--

/*!50001 DROP VIEW IF EXISTS `vw_publishedarticles`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8mb4 */;
/*!50001 SET character_set_results     = utf8mb4 */;
/*!50001 SET collation_connection      = utf8mb4_0900_ai_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `vw_publishedarticles` AS select `a`.`article_id` AS `article_id`,`av`.`version_id` AS `version_id`,`av`.`title` AS `title`,`user`.`username` AS `author_name`,`av`.`created_at` AS `published_date` from (((`article` `a` join `article_version` `av` on((`a`.`current_published_version_id` = `av`.`version_id`))) join `user` `user` on((`a`.`author_id` = `user`.`user_id`))) join `status` `s` on((`av`.`status_id` = `s`.`status_id`))) where (`s`.`status_name` = 'Published') */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-08-25 12:22:33
