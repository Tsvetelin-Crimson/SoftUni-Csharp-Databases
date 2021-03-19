SELECT Username, IpAddress
	FROM Users
WHERE IpAddress LIKE '___.1%.%.___'
ORDER BY Username
--***.1^.^.***
--Legend: * - one symbol, ^ - one or more symbols