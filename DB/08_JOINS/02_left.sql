-- 1. List All Articles and their Assigned Categories (Including uncategorized articles)
SELECT 
    article.article_id,
    IFNULL(GROUP_CONCAT(category.name SEPARATOR ', '), 'Uncategorized') AS category_names
FROM ARTICLE article
LEFT JOIN ARTICLE_CATEGORY article_category ON article.article_id = article_category.article_id
LEFT JOIN CATEGORY category ON article_category.category_id = category.category_id
GROUP BY article.article_id;


-- 2. A list of every single user in the database, and the titles of all the officially published articles they have written.
SELECT 
    usr.username, 
    IFNULL(article_version.title, 'No Published Articles Yet') AS authored_article_title
FROM USER usr 
LEFT JOIN ARTICLE article ON usr.user_id = article.author_id
LEFT JOIN ARTICLE_VERSION article_version ON article.current_published_version_id = article_version.version_id;
