-- BEFORE INSERT (validate email to convert into lowercase)

DELIMITER //
CREATE TRIGGER trg_beforeinsert_user
BEFORE INSERT
ON USER
FOR EACH ROW
BEGIN
    -- 'NEW' holds the data the user is trying to insert
    -- We force the email to be in lowercase to avoid problems during auth
    SET NEW.email = LOWER(NEW.email)
END //
DELIMITER ;



-- AFTER INSERT (for editorial flow)

DELIMITER //
CREATE TRIGGER trg_afterinsert_editorialreview
AFTER INSERT
ON EDITORIAL_REVIEW
FOR EACH ROW
BEGIN

    IF NEW.decision = 'Approve' THEN
        UPDATE ARTICLE_VERSION
        SET status_id = (
            SELECT status_id
            FROM STATUS
            WHERE status_name = 'Published'
        )
        WHERE version_id = NEW.version_id;
    
    ELSEIF NEW.decision = 'Reject' THEN
        UPDATE ARTICLE_VERSION
        SET status_id = (
            SELECT status_id 
            FROM STATUS
            WHERE status_name = 'Rejected'
        )
        WHERE version_id = NEW.version_id;

    ELSEIF NEW.decision = 'Suggest Improvement' THEN
        UPDATE ARTICLE_VERSION
        SET status_id = (
            SELECT status_id 
            FROM STATUS
            WHERE status_name = 'Needs Improvement'
        )
        WHERE version_id = NEW.version_id;

    END IF;
END //
DELIMITER ;



-- BEFORE UPDATE (cannot downgrade a article status)

DELIMITER //
CREATE TRIGGER trg_beforeupdate_articleversion
BEFORE UPDATE 
ON ARTICLE_VERSION
FOR EACH ROW
BEGIN
    DECLARE v_published_id INT;
    DECLARE v_draft_id INT;
    
    -- Dynamically grab the IDs so we avoid hardcoding magic numbers
    SELECT status_id INTO v_published_id FROM STATUS WHERE status_name = 'Published';
    SELECT status_id INTO v_draft_id FROM STATUS WHERE status_name = 'Draft';

    -- Check if someone is trying to make an illegal state change
    IF OLD.status_id = v_published_id AND NEW.status_id = v_draft_id THEN
        -- SIGNAL SQLSTATE '45000' acts like 'throw new Exception()' in C#
        -- It immediately aborts the UPDATE query.
        SIGNAL SQLSTATE '45000' 
        SET MESSAGE_TEXT = 'Invalid Action: Cannot downgrade a Published version back to a Draft. Please create a new version instead.';
    END IF;
END //
DELIMITER ;



-- AFTER UPDATE (when article is published, update current_published_version_id to point the new version)

DELIMITER //
CREATE TRIGGER trg_afterupdate_articleversion
AFTER UPDATE
ON ARTICLE_VERSION
FOR EACH ROW
BEGIN

    DECLARE v_published_id INT;
    
    SELECT 
        status_id 
    INTO 
        v_published_id 
    FROM 
        STATUS 
    WHERE 
        status_name = 'Published';

    IF NEW.status_id = v_published_id AND OLD.status_id != v_published_id THEN
        UPDATE ARTICLE
        SET current_published_version_id = NEW.version_id
        WHERE article_id = NEW.article_id;
    END IF;

END //
DELIMITER ;



-- BEFORE DELETE (take backup in the cases where needed)

DELIMITER //
CREATE TRIGGER trg_beforedelete_article
BEFORE DELETE 
ON ARTICLE
FOR EACH ROW
BEGIN
    -- Just before the article is permanently destroyed, 
    -- we rescue the data and copy it into an Archive table!
    
    INSERT INTO ARTICLE_ARCHIVE (old_article_id, original_author_id, deleted_at)
    VALUES (OLD.article_id, OLD.author_id, CURRENT_TIMESTAMP); 
END //
DELIMITER ;



-- AFTER DELETE ()

DELIMITER //
CREATE TRIGGER trg_afterdelete_user
AFTER DELETE 
ON USER
FOR EACH ROW
BEGIN
    -- The user has been successfully deleted from the database.
    -- Now, we write a "receipt" to our Audit Log using their OLD data.
    
    INSERT INTO USER_AUDIT_LOG (deleted_user_id, deleted_username, deleted_timestamp)
    VALUES (OLD.user_id, OLD.username, CURRENT_TIMESTAMP);
    
END //
DELIMITER ;