-- 1. Displaying a default status for unpublished articles
SELECT 
    article_id, 
    IFNULL(current_published_version_id, 'Draft - Not Published') AS publish_status 
FROM ARTICLE;