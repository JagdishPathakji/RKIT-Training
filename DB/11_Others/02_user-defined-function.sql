-- Usage:- Function 1: Estimating "Reading Time" (fn_EstimateReadingTime)
USE knowledge_base;

DROP FUNCTION IF EXISTS fn_EstimateReadingTime;

DELIMITER //
CREATE FUNCTION fn_EstimateReadingTime(p_version_id BINARY(16))
RETURNS INT
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

END //
DELIMITER ;

SHOW FUNCTION STATUS
WHERE Db = 'knowledge_base'
  AND Name = 'fn_EstimateReadingTime';
  
SELECT DATABASE();

-- Using it:-
SELECT title, fn_EstimateReadingTime(version_id) AS read_time_mins 
FROM ARTICLE_VERSION;





-- Usage:- Fetching Average Rating (fn_GetAverageRating)
DELIMITER //
CREATE FUNCTION fn_GetAverageRating(p_article_id BINARY(16)) 
RETURNS DECIMAL(3,1)
READS SQL DATA
BEGIN
    DECLARE avg_score DECIMAL(3,1);
    
    SELECT COALESCE(AVG(rating_value), 0.0) INTO avg_score
    FROM USER_RATING
    WHERE article_id = p_article_id;
    
    RETURN avg_score;
END //
DELIMITER ;


SELECT article_id 
FROM ARTICLE 
WHERE fn_GetAverageRating(article_id) > 4.5;