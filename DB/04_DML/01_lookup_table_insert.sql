-- Insert Default values in all lookup tables
INSERT INTO ROLE (role_id, role_name) VALUES 
(1, 'Reviewer'), (2, 'Author'), (3, 'Editor');

INSERT INTO STATUS (status_id, status_name) VALUES 
(1, 'Draft'), (2, 'Pending Editor Review'), (3, 'Needs Improvement'), (4, 'Rejected'), (5, 'Published');

INSERT INTO BLOCK_TYPE (block_type_id, type_name) VALUES 
(1, 'Text'), (2, 'Code'), (3, 'Image');

INSERT INTO CATEGORY (name) VALUES 
('Engineering & Technology'),
('Product & Design'),
('Human Resources'),
('Sales & Marketing'),
('Customer Support'),
('Company Operations');