DELIMITER //

CREATE PROCEDURE sp_CountArticlesByTag_UsingCursor(IN p_tag_name VARCHAR(50))
BEGIN
	-- parameters
    
    DECLARE v_total_count INT DEFAULT 0;
    DECLARE v_current_article_id BINARY(16);
    
    -- We need a flag to know when the loop hits the end of the list
    DECLARE done INT DEFAULT FALSE;
    
    -- 2. DECLARE THE CURSOR: We define the "list" we want to loop through
    DECLARE article_cursor CURSOR FOR 
        SELECT article_tag.article_id 
        FROM ARTICLE_TAG article_tag
        JOIN TAG tag ON article_tag.tag_id = tag.tag_id
        WHERE tag.name = p_tag_name;
        
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

END //
DELIMITER ;


CALL sp_CountArticlesByTag_UsingCursor('SQL');