SELECT DISTINCT k.DepartmentID, k.Salary
	FROM (SELECT DepartmentID, Salary, DENSE_RANK() OVER (PARTITION BY DepartmentID ORDER BY Salary DESC) AS [Ranked]
	FROM Employees) as k
	WHERE K.Ranked = 3
