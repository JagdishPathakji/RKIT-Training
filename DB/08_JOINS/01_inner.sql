-- 1. Get Published Articles and their Author's Name
SELECT 
    article.article_id,
    article_version.title AS article_title,
    usr.username AS author_name,
    article.created_at AS original_creation_date
FROM ARTICLE article
INNER JOIN USER usr ON article.author_id = usr.user_id
INNER JOIN ARTICLE_VERSION article_version ON article.current_published_version_id = article_version.version_id;


-- 2. Show Tags for an Article
SELECT 
    tag.name AS tag_name
FROM ARTICLE article
INNER JOIN ARTICLE_TAG article_tag ON article.article_id = article_tag.article_id
INNER JOIN TAG tag ON article_tag.tag_id = tag.tag_id
WHERE article.article_id = UUID_TO_BIN('AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA', 1);



-- 3. Show Categories for an Article
SELECT
    category.name AS category
FROM ARTICLE article
INNER JOIN ARTICLE_CATEGORY article_category ON article.article_id = article_category.article_id
INNER JOIN CATEGORY category ON category.category_id = article_category.category_id
WHERE article.article_id = UUID_TO_BIN('AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA', 1);
