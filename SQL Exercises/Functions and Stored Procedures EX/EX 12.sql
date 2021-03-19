CREATE PROC usp_CalculateFutureValueForAccount (@AccountId INT, @YearlyRate FLOAT)
AS
	SELECT a.Id, 
		ah.FirstName, 
		ah.LastName, 
		a.Balance AS [Current Balance], 
		dbo.ufn_CalculateFutureValue(a.Balance, @YearlyRate, 5) AS [Balance in 5 years]
		FROM Accounts a
		JOIN AccountHolders ah ON ah.Id = a.AccountHolderId
	WHERE a.Id = @AccountId
	