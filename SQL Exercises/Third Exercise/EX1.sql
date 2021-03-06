CREATE TABLE Passports 
(
	PassportID INT PRIMARY KEY IDENTITY(101, 1),
	PassportNumber CHAR(8),
)

INSERT INTO Passports VALUES 
('N34FG21B'),
('K65LO4R7'),
('ZE657QP2')


CREATE TABLE Persons 
(
	PersonID INT PRIMARY KEY IDENTITY,
	FirstName NVARCHAR(50),
	Salary DECIMAL(15, 2),
	PassportID INT REFERENCES Passports(PassportID) UNIQUE
)

INSERT INTO Persons VALUES 
('Roberto', 43300.00, 102),
('Tom', 56100.00, 103),
('Yana', 60200.00, 101)


--1, 'Roberto' 43300.00 102		101 N34FG21B
--2, 'Tom' 56100.00 103			102 K65LO4R7
--3, 'Yana' 60200.00 101		103 ZE657QP2