SELECT d.DepositGroup 
FROM (SELECT TOP(2) DepositGroup, AVG(MagicWandSize) AS LongestMagicWand
	FROM WizzardDeposits
GROUP BY DepositGroup
ORDER BY LongestMagicWand ) AS D


-- Better solution
SELECT TOP(2) DepositGroup
	FROM WizzardDeposits
GROUP BY DepositGroup
ORDER BY AVG(MagicWandSize)