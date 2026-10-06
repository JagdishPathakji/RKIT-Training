-- 1. find which user is allocated which role, and find roles which have no user
SELECT 
    user_role.user_id, 
    role.role_name AS role_name
FROM USER_ROLE user_role
RIGHT JOIN ROLE role ON user_role.role_id = role.role_id;

-- 2. Find Unused Categories
SELECT
    category.name AS unused_categories
FROM ARTICLE_CATEGORY article_category 
RIGHT JOIN
CATEGORY category ON category.category_id = article_category.category_id
WHERE article_category.article_id IS NULL;
