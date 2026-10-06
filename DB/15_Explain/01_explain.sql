-- When you type EXPLAIN in front of article query, the database does not return the data. Instead, it returns article Query Execution Plan.

-- It is the database's way of opening its brain and telling you: "Here is the exact strategy I am going to use to find the data you asked for."

-- Developers use EXPLAIN when a query is running slowly. By reading the output, you can instantly see if your database is working efficiently, or if it is doing unnecessary heavy lifting.


-- (*) When you run it, you look for three main columns in the output:
-- 1. type: How did it search? If it says ALL, it means a "Full Table Scan" (it read every single row in the table, which is terrible for performance). If it says ref or eq_ref, it used an Index (which is incredibly fast).
-- 2. key: Which Index did it use? If it says NULL, it means the database couldn't find an index and had to search manually.
-- 3. rows: How many rows does the database estimate it has to read to find your answer? If this number is in the millions, your query needs fixing!


EXPLAIN SELECT 
    version_id, content_data 
    FROM CONTENT_BLOCK
    WHERE content_data LIKE '%transaction%';

EXPLAIN SELECT 
    article_version.title, usr.username
    FROM ARTICLE article
    JOIN ARTICLE_VERSION article_version 
    ON 
    article.current_published_version_id = article_version.version_id
    JOIN USER usr 
    ON article.author_id = usr.user_id;

EXPLAIN ANALYZE  -- provides time taken to execute the query
    SELECT article_id, AVG(rating_value) 
    FROM USER_RATING 
    GROUP BY article_id;


-- If EXPLAIN tells you article query is slow (usually by showing type: ALL and key: NULL), you have to step in and fix it.

-- 1. Create an Index
-- 2. Rewrite the Query
-- 3. Add Pagination


-- (1) id -> this is execution step number.
-- (2) select_type = SIMPLE -> it means no join, no subquery, no union, no cte, just straight forward SELECT.
-- (3) table = Customers -> Tells which table MySQL is reading.
-- (4) partitions = NULL -> Your table is not partitioned. Partitioning is an advanced feature where article table is split into multiple physical partitions.
-- (5) type = ref -> type = How is MySQL going to access this table ?. more than one rows expected (normal index = ref) , primary key = const (const = 1 row expected)
-- (6) possible_keys = primaryindex -> index that could be used.
-- (7) key = primaryindex -> After evaluating all possibilities, I chose this index.
-- (8) key_len = 4 -> This tells how many bytes of the index MySQL used. (INT so 4)
-- (9) ref = const -> This tells what MySQL is comparing the indexed column against. (101 is constant value used in query)
-- (10) rows = 1 -> It is MySQL's estimate of how many rows it expects to examine.
-- (11) filtered = 100.00 -> I expect all examined rows to satisfy the WHERE condition.
-- (12) Extra = NULL -> No extra operations required.