-- refering to this database for next set of sql statements
USE knowledge_base;

-- store general user information 
CREATE TABLE USER (
    user_id BINARY(16) DEFAULT (UUID_TO_BIN(UUID(), 1)) PRIMARY KEY,
    username VARCHAR(100) UNIQUE NOT NULL,
    email VARCHAR(255) UNIQUE NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

USE knowledge_base;
DESCRIBE USER_ROLE;
-- store role of the each user
CREATE TABLE USER_ROLE (
    user_id BINARY(16),
    role_id INT,
    PRIMARY KEY (user_id, role_id),
    FOREIGN KEY (user_id) REFERENCES USER(user_id) ON DELETE CASCADE, -- delete this entry if user is deleted
    FOREIGN KEY (role_id) REFERENCES ROLE(role_id) ON DELETE CASCADE -- delete this entry if role is deleted
);