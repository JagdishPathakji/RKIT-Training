USE db;
-- EXPLAIN tells you how MySQL plans to execute a query.
-- It does not execute the query, instead it shows the execution plan chosen by the optimizer.

-- Instead of immediately executing:
SELECT * FROM Customers WHERE City = 'surat';
-- We do
EXPLAIN SELECT * FROM Customers WHERE CustomerID = 101;
EXPLAIN SELECT * FROM Customers WHERE City = 'surat';

ALTER TABLE Customers ADD INDEX primaryindex(CustomerID);



-- QUERY :- EXPLAIN SELECT * FROM Customers WHERE CustomerID = 101;
-- OUTPUT :-
-- id  select_type  table     partitions  type  possible_keys  key  		key_len  ref  rows  filtered  extra
-- 1	SIMPLE		Customer  NULL		  ref	primaryindex  primaryindex    4		const	1	100			NULL

-- (1) id -> this is execution step number. (there is only one step in this query)
-- (2) select_type = SIMPLE -> it means no join, no subquery, no union, no cte, just straight forward SELECT.
-- (3) table = Customers -> Tells which table MySQL is reading.
-- (4) partitions = NULL -> Your table is not partitioned. Partitioning is an advanced feature where a table is split into multiple physical partitions.
-- (5) type = ref -> type = How is MySQL going to access this table ?. more than one rows expected (normal index = ref) , primary key = const (const = 1 row expected)
-- (6) possible_keys = primaryindex -> index that could be used.
-- (7) key = primaryindex -> After evaluating all possibilities, I chose this index.
-- (8) key_len = 4 -> This tells how many bytes of the index MySQL used. (INT so 4)
-- (9) ref = const -> This tells what MySQL is comparing the indexed column against. (101 is constant value used in query)
-- (10) rows = 1 -> It is MySQL's estimate of how many rows it expects to examine.
-- (11) filtered = 100.00 -> I expect all examined rows to satisfy the WHERE condition.
-- (12) Extra = NULL -> No extra operations required.