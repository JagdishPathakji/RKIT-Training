-- Query:- get articles sorted based on a column and direction

SET @sort_column = 'created_at';
SET @sort_direction = 'DESC';

SET @query = CONCAT(
    'SELECT
        title, created_at
    FROM 
        ARTICLE_VERSION
    WHERE 
        status_id = ?
    ORDER BY
    ',
    @sort_column,
    ' ',
    @sort_direction
);

PREPARE stmt_DynamicSearch FROM @query;
SET @status_id = (SELECT status_id FROM STATUS WHERE status_name = 'Published');
EXECUTE stmt_DynamicSearch USING @status_id;
DEALLOCATE PREPARE stmt_DynamicSearch;




-- Query:- Dynamic Profile Updater
-- 1. The User selects "MONTH" from the frontend dropdown
SET @time_period = 'MONTH'; 

-- 2. Build the query string dynamically to inject the correct time function
SET @dynamic_sql = CONCAT(
    'SELECT ', 
    @time_period, '(created_at) AS time_group, ', -- Becomes: MONTH(created_at)
    'COUNT(*) AS total_articles_published ',
    'FROM ARTICLE_VERSION ',
    'WHERE status_id = ? ',
    'GROUP BY ', @time_period, '(created_at)'    -- Becomes: GROUP BY MONTH(created_at)
);

-- 3. PREPARE the chart query
PREPARE stmt_AnalyticsChart FROM @dynamic_sql;

-- 4. EXECUTE, securely passing the ID for 'Published'
SET @published_status = (SELECT status_id FROM STATUS WHERE status_name = 'Published');
EXECUTE stmt_AnalyticsChart USING @published_status;

-- 5. DEALLOCATE
DEALLOCATE PREPARE stmt_AnalyticsChart;