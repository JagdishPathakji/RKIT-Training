-- refering to this database for next set of sql statements
USE knowledge_base;


-- storing track of status of each article when reviewed by an editor
CREATE TABLE EDITORIAL_REVIEW (
    review_id BINARY(16) DEFAULT (UUID_TO_BIN(UUID(), 1)) PRIMARY KEY,
    version_id BINARY(16) NOT NULL, -- which version is being reviewed by editor ?
    editor_id BINARY(16) NOT NULL, -- who is the editor reviewing this article ?
    decision ENUM('Approve', 'Reject', 'Suggest Improvements') NOT NULL, -- possible decisions
    feedback TEXT NOT NULL, -- feedback by editor to author
    reviewed_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP, -- when it was reviewed for publishment
    FOREIGN KEY (version_id) REFERENCES ARTICLE_VERSION(version_id) ON DELETE CASCADE, -- delete this entry when that article version is deleted
    FOREIGN KEY (editor_id) REFERENCES USER(user_id) -- editor must be a user with role as editor
);