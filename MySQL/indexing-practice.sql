-- Creating, Viewing, Using and Dropping Indexes

-- 1. Create table
USE db;
DROP TABLE Customers;
CREATE TABLE Customers (
    CustomerID INT PRIMARY KEY, -- indexing automatically
    CustomerName VARCHAR(100) NOT NULL,
    Email VARCHAR(150),
    City VARCHAR(50),
    Phone VARCHAR(15)
);

INSERT INTO Customers
(CustomerID, CustomerName, Email, City, Phone)
VALUES
(1, 'Amit Patel',  'amit@gmail.com',  'Ahmedabad', '9876500001'),
(2, 'Rahul Shah',  'rahul@gmail.com', 'Surat',     '9876500002'),
(3, 'Priya Mehta', 'priya@gmail.com', 'Vadodara',  '9876500003'),
(4, 'Neha Patel',  'neha@gmail.com',  'Surat',     '9876500004'),
(5, 'Karan Joshi', 'karan@gmail.com', 'Rajkot',    '9876500005'),
(6, 'Riya Shah',   'riya@gmail.com',  'Surat',     '9876500006');

-- 2. View existing indexes
SHOW INDEX FROM Customers;
-- or
SHOW INDEXES FROM Customers;

-- 3. Create a single column secondary index (Allows duplicate city)
CREATE INDEX idx_customer_city ON Customers(City);

-- 4. Create another secondaru index (Allows duplicate phone)
CREATE INDEX idx_customer_phone ON Customers(Phone);

-- 5. Create index using `ALTER TABLE`
ALTER TABLE Customers ADD INDEX idx_customer_name(CustomerName);

-- 6. Create a unique index (does not allow duplicate Email)
CREATE UNIQUE INDEX uq_customer_email ON Customers(Email);

-- 7. Drop a secondary index
DROP INDEX idx_customer_city ON Customers;
-- or
ALTER TABLE Customers DROP INDEX idx_customer_city;

-- 8. Removing primary-key index
ALTER TABLE Customers DROP PRIMARY KEY;