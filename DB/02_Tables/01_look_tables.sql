-- refering to this database for next set of sql statements
USE knowledge_base;

-- this table is for the different roles provided like (author, editor, reviewer / normal user)
CREATE TABLE ROLE (
    role_id INT PRIMARY KEY,
    role_name VARCHAR(50) UNIQUE NOT NULL
);

-- this table is to keep the track of status (draft -> pending for review -> needs improvement / rejected / published)
CREATE TABLE STATUS (
    status_id INT PRIMARY KEY,
    status_name VARCHAR(50) UNIQUE NOT NULL
);

-- this table is for the type of content we will allow in articles (text, code, image)
CREATE TABLE BLOCK_TYPE (
    block_type_id INT PRIMARY KEY,
    type_name VARCHAR(50) UNIQUE NOT NULL
);

-- different categories 
CREATE TABLE CATEGORY (
    category_id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100) UNIQUE NOT NULL
);