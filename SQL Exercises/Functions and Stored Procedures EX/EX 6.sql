CREATE OR ALTER PROC usp_EmployeesBySalaryLevel (@LevelOfSalary VARCHAR(50))
AS
SELECT FirstName, LastName
		FROM Employees
	WHERE dbo.ufn_GetSalaryLevel(Salary) = @LevelOfSalary

EXEC usp_EmployeesBySalaryLevel 'Low'

