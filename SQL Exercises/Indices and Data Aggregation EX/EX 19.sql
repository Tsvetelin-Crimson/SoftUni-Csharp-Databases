SELECT FirstName, LastName, DepartmentID, Salary
	FROM Employees as emp
WHERE Salary > (SELECT AVG(Salary) 
	FROM Employees as aver
	WHERE aver.DepartmentID = emp.DepartmentID
)
ORDER BY DepartmentID

SELECT DepartmentID, AVG(Salary) 
	FROM Employees 
	GROUP BY DepartmentID 