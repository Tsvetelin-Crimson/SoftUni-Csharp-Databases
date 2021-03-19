SELECT * INTO TempTable
	FROM Employees
WHERE Salary > 30000

DELETE FROM TempTable
	WHERE ManagerID = 42

UPDATE TempTable
	SET Salary = Salary + 5000
WHERE DepartmentID = 1

SELECT DepartmentID ,AVG(Salary)
	FROM TempTable
GROUP BY DepartmentID