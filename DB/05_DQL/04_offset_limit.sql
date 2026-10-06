-- Get the first 10 newest users
SELECT username, created_at 
FROM USER 
ORDER BY created_at DESC 
LIMIT 10;


-- Get the NEXT 10 users (Page 2)
SELECT username, created_at 
FROM USER 
ORDER BY created_at DESC 
LIMIT 10 OFFSET 10;