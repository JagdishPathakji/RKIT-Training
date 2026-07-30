-- Product shipment example for transaction understanding

USE db;

CREATE TABLE Customers(
    CustomerID INT PRIMARY KEY,
    Name VARCHAR(50),
    Balance DECIMAL(10,2)
);
INSERT INTO Customers VALUES (1,'Jagdish',60000);

CREATE TABLE Products(
    ProductID INT PRIMARY KEY,
    ProductName VARCHAR(50),
    Price DECIMAL(10,2),
    Stock INT
);
INSERT INTO Products VALUES (101,'Laptop',50000,5);

CREATE TABLE Orders(
    OrderID INT PRIMARY KEY,
    CustomerID INT,
    ProductID INT,
    Amount DECIMAL(10,2),
    Status VARCHAR(20)
);

CREATE TABLE Payments(
    PaymentID INT PRIMARY KEY,
    OrderID INT,
    Amount DECIMAL(10,2),
    Status VARCHAR(20)
);

CREATE TABLE Shipment(
    ShipmentID INT PRIMARY KEY,
    OrderID INT,
    Status VARCHAR(20)
);


-- PSEUDO CODE UNDERSTANDING
START TRANSACTION;

-- Step 1: Create Order
INSERT INTO Orders VALUES (1001,1,101,50000,'CREATED');

IF SUCCESS THEN
    SAVEPOINT order_created;
ELSE
    ROLLBACK;
    STOP;
END IF;


-- Step 2: Reserve Stock
UPDATE Products SET Stock = Stock - 1 WHERE ProductID = 101 AND Stock > 0;

IF SUCCESS THEN
    SAVEPOINT stock_reserved;
ELSE
    ROLLBACK TO order_created;
    -- cancel order
    UPDATE Orders SET Status='CANCELLED' WHERE OrderID=1001;
    COMMIT;
END IF;


-- Step 3: Payment
UPDATE Customers SET Balance = Balance - 50000 WHERE CustomerID=1 AND Balance >= 50000;

IF SUCCESS THEN
    INSERT INTO Payments VALUES (501,1001,50000,'SUCCESS');
    SAVEPOINT payment_done;
ELSE
    ROLLBACK TO stock_reserved;
    -- retry payment
END IF;


-- Step 4: Shipment
INSERT INTO Shipment VALUES (701,1001,'CREATED');

IF SUCCESS THEN
    COMMIT;
ELSE
    ROLLBACK TO payment_done;
    -- retry shipment
END IF;