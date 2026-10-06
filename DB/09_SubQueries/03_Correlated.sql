-- Query:- Find the latest version of EACH article.
SELECT 
    article_id, version_id, title 
FROM ARTICLE_VERSION AS article_version
WHERE version_number = (
    SELECT MAX(version_number) 
    FROM ARTICLE_VERSION article_version
    WHERE article_version.article_id = article_version.article_id
);


-- Query:- Find users who have commented on their OWN articles.
SELECT 
    user_id,article_id
FROM USER_COMMENT AS user_comment
WHERE user_id = (
    SELECT author_id 
    FROM ARTICLE AS article
    WHERE article.article_id = user_comment.article_id
);


-- Query:- Get article list of authors along with their total published articles count.
SELECT 
    usr.username,
    (
        SELECT COUNT(*) 
        FROM ARTICLE AS article 
        WHERE article.author_id = usr.user_id 
          AND article.current_published_version_id IS NOT NULL
    ) AS total_published_articles
FROM USER AS user;