-- refering to this database for next set of sql statements
USE knowledge_base;

-- store each version of each article with its status
CREATE TABLE ARTICLE_VERSION (
    version_id BINARY(16) DEFAULT (UUID_TO_BIN(UUID(), 1)) PRIMARY KEY,
    article_id BINARY(16) NOT NULL,
    version_number INT NOT NULL UNIQUE,
    title VARCHAR(255) NOT NULL,
    status_id INT NOT NULL,
    created_by BINARY(16) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP, -- yyyy-mm-dd 
    FOREIGN KEY (article_id) REFERENCES ARTICLE(article_id) ON DELETE CASCADE, -- delete this entry when article is deleted
    FOREIGN KEY (status_id) REFERENCES STATUS(status_id),
    FOREIGN KEY (created_by) REFERENCES USER(user_id)
);

-- ARTICLE table always point to latest version
ALTER TABLE ARTICLE 
ADD CONSTRAINT fk_current_published_version 
FOREIGN KEY (current_published_version_id) REFERENCES ARTICLE_VERSION(version_id) ON DELETE SET NULL;

-- content block for each article's each version (track by version_id directly)
CREATE TABLE CONTENT_BLOCK (
    block_id BINARY(16) DEFAULT (UUID_TO_BIN(UUID(), 1)) PRIMARY KEY,
    version_id BINARY(16) NOT NULL,
    block_type_id INT NOT NULL,
    content_data TEXT,
    sequence_order INT NOT NULL,
    FOREIGN KEY (version_id) REFERENCES ARTICLE_VERSION(version_id) ON DELETE CASCADE, -- delete this entry if this version of article is deleted
    FOREIGN KEY (block_type_id) REFERENCES BLOCK_TYPE(block_type_id) -- available types for article must be from BLOCK_TYPE look_table
);