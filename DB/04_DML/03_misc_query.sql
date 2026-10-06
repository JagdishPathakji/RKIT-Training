-- upsert 
-- The database tries to INSERT a new row. If that row already exists (based on a PRIMARY KEY or UNIQUE constraint), it doesn't throw an error; instead, it automatically switches to doing an UPDATE on the existing row.

-- sample query
-- INSERT INTO USER_RATING (article_id, user_id, rating_value) 
-- VALUES ('article-uuid', 'user-uuid', 5)
-- ON DUPLICATE KEY UPDATE 
--     rating_value = 5, 
--     updated_at = CURRENT_TIMESTAMP;



-- replace into -- check
-- REPLACE INTO looks similar to an Upsert, but it is much more aggressive under the hood. If it finds a duplicate key, it completely DELETES the old row and INSERTS a brand new one.
-- (dangerous if applied ON DELETE CASCADE)



-- decimal vs float
-- FLOAT (or DOUBLE): Stores an approximate value -- not 100% accurate
-- DECIMAL (or NUMERIC): Stores the exact value -- 100% accurate