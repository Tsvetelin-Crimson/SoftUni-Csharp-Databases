
	-- "My" way
SELECT SUM(Sums.Difference) AS SumDifference
	FROM (SELECT  LEAD(DepositAmount, 1) OVER (ORDER BY Id DESC) - DepositAmount AS [Difference]
	FROM WizzardDeposits) AS Sums


	-- Teachers way
SELECT SUM(w2.DepositAmount - w1.DepositAmount)
	FROM WizzardDeposits w1
	JOIN WizzardDeposits w2 ON  w2.id + 1 = w1.Id

	SELECT * FROM WizzardDeposits