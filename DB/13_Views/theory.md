# What is View ?
A view is a `virtual table`.
> It does not store data itself, instead it stores a SELECT query.
Whenever someone accesses the view, MySQL executes that stored SELECT statement and returns the latest data.



# Syntax
CREATE VIEW ViewName AS
SELECT ...
FROM ...
WHERE ...;



# Example
### Create view
CREATE VIEW ProductDetails AS 
SELECT ProductID, ProductName, Price FROM Products;
### Use view
SELECT * FROM ProductDetails;



> We can build views on top of other views with same syntax.
> Views can contain JOIN, GROUP BY, HAVING, WHERE, DISTINCT, UNION, AGGR FUNCTION, SUBQUERIES, ETC. Almost any valid `SELECT` statement is applicanle to views.



# Usage
1. Reusable logic
2. Cleaner
3. Security (can remove password, etc from views)
4. Simpler queries when we need to view the data


# Limitations
1. Can become slower if built on very complex queries.
2. Depend on the underlying tables - dropping or changing required columns can break a view.



# Updatable and Non Updatable Views
If a view is not a real table, can i INSERT, UPDATE, DELETE using it ?
> Ans : Sometime Y, Sometime N.
It depends on how the view is created.

Generally, a view is updatable when every row in the view maps directly to exactly one row in one base table. 
Example :-
CREATE VIEW ProductView AS SELECT * FROM Products;
(this is updatable)


A view is not updatable when the SELECT query contains :-
1. Group by
2. Aggregate functions
3. DISTINCT
4. Multiple Tables (JOIN)