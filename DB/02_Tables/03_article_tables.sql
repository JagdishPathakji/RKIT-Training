-- refering to this database for next set of sql statements
USE knowledge_base;

-- different tags
CREATE TABLE TAG (
    tag_id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100) UNIQUE NOT NULL
);

-- store author, latest version accepted by editor 
CREATE TABLE ARTICLE (
    article_id BINARY(16) DEFAULT (UUID_TO_BIN(UUID(), 1)) PRIMARY KEY,
    author_id BINARY(16) NOT NULL,
    current_published_version_id BINARY(16) NULL, -- Will be set via foreign key after ARTICLE_VERSION is created
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (author_id) REFERENCES USER(user_id)
);

-- store categories of an article (one article -> many categories)
CREATE TABLE ARTICLE_CATEGORY (
    article_id BINARY(16),
    category_id INT,
    PRIMARY KEY (article_id, category_id),
    FOREIGN KEY (article_id) REFERENCES ARTICLE(article_id) ON DELETE CASCADE, -- delete this entry when article is deleted
    FOREIGN KEY (category_id) REFERENCES CATEGORY(category_id) ON DELETE CASCADE -- delete this entry when category is deleted 
);

-- store tags of an article (one article -> many tags)
CREATE TABLE ARTICLE_TAG (
    article_id BINARY(16),
    tag_id INT,
    PRIMARY KEY (article_id, tag_id),
    FOREIGN KEY (article_id) REFERENCES ARTICLE(article_id) ON DELETE CASCADE, -- delete this entry when article is deleted
    FOREIGN KEY (tag_id) REFERENCES TAG(tag_id) ON DELETE CASCADE -- delete entry entry when tag is deleted 
);