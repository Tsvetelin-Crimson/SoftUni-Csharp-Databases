CREATE OR ALTER PROC usp_GetEmployeesFromTown (@Town VARCHAR(50))
AS
SELECT e.FirstName, e.LastName
	FROM Employees e
	LEFT JOIN Addresses a ON a.AddressID = e.AddressID
	LEFT JOIN Towns t ON a.TownID = t.TownID
	WHERE t.Name = @Town
	-- No need for a left join
EXEC usp_GetEmployeesFromTown sofia