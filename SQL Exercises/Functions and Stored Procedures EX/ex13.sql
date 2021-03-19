CREATE FUNCTION ufn_CashInUsersGames (@GameName NVARCHAR(50))
RETURNS TABLE
AS
RETURN (SELECT SUM(T.Cash) AS SumCash
	FROM (SELECT g.Name, ug.Cash AS Cash, ROW_NUMBER() OVER(ORDER BY ug.Cash DESC) AS [Row Number]
				FROM Games g
				JOIN UsersGames ug ON g.Id = ug.GameId
				WHERE g.Name = @GameName) AS t
	WHERE t.[Row Number] % 2 = 1)

SELECT * FROM dbo.ufn_CashInUsersGames('Moscow')