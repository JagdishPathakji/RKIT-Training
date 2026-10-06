-- select users created after '01-01-2026'
SELECT username, email FROM USER WHERE created_at > '2026-01-01';

-- select all articles created after '01-01-2026'
SELECT article_id FROM ARTICLE WHERE created_at > '2026-01-01';

-- select all article versions reviewed after '01-01-2026'
SELECT review_id, version_id, editor_id FROM EDITORIAL_REVIEW WHERE reviewed_at > '2026-01-01';

-- select all ratings above 4 and created after '01-01-2026'
SELECT rating_value FROM USER_RATING WHERE rating_value > 4.0 AND updated_at > '2026-01-01';

-- select all comments after '01-01-2026'
SELECT comment_text FROM USER_COMMENT WHERE created_at > '2026-01-01';