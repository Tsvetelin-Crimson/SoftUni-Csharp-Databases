CREATE PROC usp_GetHoldersWithBalanceHigherThan (@MinBalance DECIMAL(15,2))
AS
SELECT FirstName, LastName
	FROM Accounts a
	JOIN AccountHolders ah ON ah.Id = a.AccountHolderId
	GROUP BY ah.FirstName, ah.LastName
HAVING SUM(a.Balance) > @MinBalance
ORDER BY FirstName, LastName