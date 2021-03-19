CREATE TABLE Employees
(
	Id INT PRIMARY KEY IDENTITY,
	FirstName VARCHAR(200) NOT NULL,
	LastName VARCHAR(200) NOT NULL,
	Title VARCHAR(100) NOT NULL,
	Notes VARCHAR(MAX)
)

INSERT INTO Employees VALUES 
('A', 'B', 'Janitor', NULL),
('C', 'D', 'Service', 'Some notes'),
('E', 'F', 'Maid', NULL)


CREATE TABLE Customers
(
	AccountNumber INT PRIMARY KEY,
	FirstName VARCHAR(200) NOT NULL,
	LastName VARCHAR(200) NOT NULL,
	PhoneNumber VARCHAR(10) NOT NULL,
	EmergencyName VARCHAR(200) NOT NULL,
	EmergencyNumber VARCHAR(10) NOT NULL,
	Notes VARCHAR(MAX)
)


INSERT INTO Customers VALUES 
(123213 ,'A', 'B', '0881233451', 'G','0881233451', NULL),
(54352 ,'C', 'D', '0882236451', 'H','0882236451','Some notes'),
(1234132 ,'E', 'F', '0889223461','I','0889223461', NULL)


CREATE TABLE RoomStatus
(
	RoomStatus VARCHAR(100) PRIMARY KEY,
	Notes VARCHAR(MAX)
)

INSERT INTO RoomStatus VALUES 
('Free', NULL),
('Taken', 'Some notes'),
('Cleaning', NULL)


CREATE TABLE RoomTypes
(
	RoomType VARCHAR(100) PRIMARY KEY,
	Notes VARCHAR(MAX)
)

INSERT INTO RoomTypes VALUES 
('Two Bed', NULL),
('AFWA', 'Some notes'),
('AWFWA', NULL)


CREATE TABLE BedTypes
(
	BedType VARCHAR(100) PRIMARY KEY,
	Notes VARCHAR(MAX)
)

INSERT INTO BedTypes VALUES 
('Two Bed', NULL),
('One Bed', 'Some notes'),
('Child Bed', NULL)

CREATE TABLE Rooms 
(
	RoomNumber INT PRIMARY KEY,
	RoomType VARCHAR(100) NOT NULL,
	BedType VARCHAR(100) NOT NULL,
	RoomStatus VARCHAR(100) NOT NULL,
	Notes VARCHAR(MAX)
)

INSERT INTO Rooms VALUES 
(120, 'Two Bed','Two Bed', 'Free', NULL),
(121, 'AFWA','One Bed', 'Taken', 'Some notes'),
(122, 'AWFWA','Child Bed', 'Cleaning', NULL)

CREATE TABLE Payments 
(
	Id INT PRIMARY KEY IDENTITY,
	EmployeeId INT NOT NULL,
	PaymentDate DATETIME NOT NULL,
	AccountNumber INT NOT NULL,
	FirstDateOccupied DATETIME,
	LastDateOccupied DATETIME,
	TotalDays INT NOT NULL,
	AmountCharged DECIMAL(15,2) NOT NULL,
	TaxRate DECIMAL(15,2) NOT NULL,
	TaxAmount DECIMAL(15,2),
	PaymentTotal DECIMAL(15,2) NOT NULL,
	Notes VARCHAR(MAX)
)

INSERT INTO Payments VALUES 
(1, 2020-10-11, 123213, NULL, NULL, 60, 400.23, 100.00, NULL, 400.23, NULL),
(2, 2020-10-15, 54352, 1/1/2021, GETDATE(), 30, 200.23, 300.50, 60.3, 260.23, 'Some notes'), -- test date
(3, 2020-10-27, 123213, NULL, NULL, 60, 400.23, 200.00, NULL, 400.23, NULL)


CREATE TABLE Occupancies 
(
	Id INT PRIMARY KEY IDENTITY,
	EmployeeId INT NOT NULL,
	DateOccupied DATETIME NOT NULL,
	AccountNumber INT NOT NULL,
	RoomNumber INT NOT NULL,
	RateApplied DECIMAL(15, 2) NOT NULL,
	PhoneCharge DECIMAL(15, 2),
	Notes VARCHAR(MAX)
)

INSERT INTO Occupancies VALUES 
(1, 2021-1-1, 123213, 120, 20.0, NULL, NULL),
(1, 2021-1-10, 54352, 121, 30.0, 10.0, 'Some Notes'),
(1, 2021-1-13, 123213, 122, 40.0, NULL, NULL)


-- EX 23
--UPDATE Payments
--SET TaxRate = TaxRate * 0.97

--SELECT TaxRate FROM Payments