USE SoftUni
-- Fof Judge remove "GO" at the end of every creation
-- EX 1
CREATE PROCEDURE usp_GetEmployeesSalaryAbove35000 
AS
	SELECT FirstName, LastName 
		FROM Employees
		WHERE Salary > 35000 
GO
-- EX 2
CREATE PROCEDURE usp_GetEmployeesSalaryAboveNumber @MinSalary DECIMAL(18,4)
AS
	SELECT FirstName, LastName 
		FROM Employees
		WHERE Salary >= @MinSalary
GO

-- EX 3
CREATE PROCEDURE usp_GetTownsStartingWith @LeftChars NVARCHAR(100)
AS
	SELECT Name
		FROM Towns
		WHERE LEFT(Name, LEN(@LeftChars)) = @LeftChars
GO
