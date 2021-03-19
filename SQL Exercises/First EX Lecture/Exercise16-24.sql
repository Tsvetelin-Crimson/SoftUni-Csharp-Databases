CREATE DATABASE SoftUni
USE SoftUni
CREATE TABLE Towns
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(200) NOT NULL
)

INSERT INTO Towns VALUES 
('Sofia'),
('Plovdiv'),
('Varna'),
('Burgas')

CREATE TABLE Addresses
(
	Id INT PRIMARY KEY IDENTITY,
	AddressText VARCHAR(200) NOT NULL,
	TownId INT NOT NULL,
	FOREIGN KEY (TownId) REFERENCES  Towns(Id)
)

INSERT INTO Addresses VALUES 
('ADAWFGEAG', 1),
('KUILRGS', 2),
('XJFCYJCG', 4)

CREATE TABLE Departments
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(200) NOT NULL
)
INSERT INTO Departments VALUES 
('Engineering, Sales'),
('Marketing'),
('Software Development'),
('Quality Assurance')


CREATE TABLE Employees
(
	Id INT PRIMARY KEY IDENTITY,
	FirstName VARCHAR(200) NOT NULL,
	MiddleName VARCHAR(200) NOT NULL,
	LastName VARCHAR(200) NOT NULL,
	JobTitle VARCHAR(100) NOT NULL,
	DepartmentId INT NOT NULL,
	HireDate DATETIME NOT NULL,
	Salary DECIMAL(15,2) NOT NULL,
	AddressId INT,
	FOREIGN KEY (DepartmentId) REFERENCES  Departments(Id),
	FOREIGN KEY (AddressId) REFERENCES  Addresses(Id)
)

INSERT INTO Employees VALUES 
('Ivan', 'Ivanov', 'Ivanov', '.NET Developer', 3, 01/02/2013, 3500.00, 1)

-- EX 20
--SELECT * FROM Towns
--ORDER BY [Name] ASC
--SELECT * FROM Departments
--ORDER BY [Name] ASC
--SELECT * FROM Employees
--ORDER BY Salary DESC

-- EX 21
--SELECT [Name] FROM Towns
--ORDER BY [Name] ASC
--SELECT [Name] FROM Departments
--ORDER BY [Name] ASC
--SELECT FirstName, LastName, JobTitle, Salary FROM Employees
--ORDER BY Salary DESC

-- EX 22
--UPDATE Employees
--SET Salary = Salary * 1.10

--SELECT Salary FROM Employees

-- EX 23
--UPDATE Payments
--SET TaxRate = TaxRate * 0.97

--SELECT TaxRate FROM Payments


-- EX 24
--DELETE FROM Occupancies