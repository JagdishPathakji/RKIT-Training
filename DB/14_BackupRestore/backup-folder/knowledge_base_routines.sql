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
/*!50001 VIEW `vw_publishedarticles` AS select `a`.`article_id` AS `article_id`,`av`.`version_id` AS `version_id`,`av`.`title` AS `title`,`u`.`username` AS `author_name`,`av`.`created_at` AS `published_date` from (((`article` `a` join `article_version` `av` on((`a`.`current_published_version_id` = `av`.`version_id`))) join `user` `u` on((`a`.`author_id` = `u`.`user_id`))) join `status` `s` on((`av`.`status_id` = `s`.`status_id`))) where (`s`.`status_name` = 'Published') */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

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
/*!50003 DROP PROCEDURE IF EXISTS `sp_CountArticlesByTag_UsingCursor` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `sp_CountArticlesByTag_UsingCursor`(IN p_tag_name VARCHAR(50))
BEGIN

    DECLARE v_total_count INT DEFAULT 0;
    DECLARE v_current_article_id BINARY(16);
    
    -- We need a flag to know when the loop hits the end of the list
    DECLARE done INT DEFAULT FALSE;
    
    -- 2. DECLARE THE CURSOR: We define the "list" we want to loop through
    DECLARE article_cursor CURSOR FOR 
        SELECT at.article_id 
        FROM ARTICLE_TAG at
        JOIN TAG t ON at.tag_id = t.tag_id
        WHERE t.name = p_tag_name;
        
    -- 3. Declare an error handler: When the cursor runs out of rows, set 'done' to TRUE
    DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = TRUE;

    -- 4. OPEN the Cursor (loads the list into memory)
    OPEN article_cursor;

    -- 5. Start the foreach loop
    count_loop: LOOP
        
        -- Grab the next row's article_id and put it in our variable
        FETCH article_cursor INTO v_current_article_id;
        
        -- If the 'NOT FOUND' handler triggered, break out of the loop!
        IF done THEN 
            LEAVE count_loop; 
        END IF;

        -- We successfully fetched a row! Add 1 to our manual counter.
        SET v_total_count = v_total_count + 1;

    END LOOP;

    -- 6. CLOSE the Cursor to free up database RAM
    CLOSE article_cursor;

    -- Output the final result to the screen
    SELECT p_tag_name AS TagName, v_total_count AS TotalArticlesFound;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-09-02 19:33:48
