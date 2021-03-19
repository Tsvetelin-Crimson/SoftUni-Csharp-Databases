CREATE TABLE Categories
(
	Id INT PRIMARY KEY IDENTITY,
	CategoryName VARCHAR(100) NOT NULL,
	DailyRate INT NOT NULL,
	WeeklyRate INT NOT NULL,
	MonthlyRate INT NOT NULL,
	WeekendRate INT NOT NULL,
)

INSERT INTO Categories VALUES 
('Motorcycle', 10, 70, 210, 20),
('Automobile', 20, 140, 420, 40),
('Bus', 15, 115, 315, 40)


CREATE TABLE Cars
(
	Id INT PRIMARY KEY IDENTITY,
	PlateNumber VARCHAR(8) NOT NULL,
	Manufacturer VARCHAR(200) NOT NULL,
	Model VARCHAR(200) NOT NULL,
	CarYear INT NOT NULL,
	CategoryId INT NOT NULL,
	Doors VARCHAR(200),
	Picture VARCHAR(MAX) NOT NULL,
	Condition VARCHAR(100) NOT NULL,
	Available BIT NOT NULL,
)

INSERT INTO Cars VALUES 
('DD1234FE', 'BMW','Toyota', 1974, 1, NULL, 'http/...', 'Well Kept', 1),
('FF4321DG', 'AUDI','AUDI', 1984, 2, NULL, 'http/...', 'Well Kept', 1),
('GD4213FJ', 'BMW','Toyota', 1989, 3, NULL, 'http/...', 'Well Kept', 0)


CREATE TABLE Employees
(
	Id INT PRIMARY KEY IDENTITY,
	FirstName VARCHAR(200) NOT NULL,
	LastName VARCHAR(200) NOT NULL,
	Title VARCHAR(100) NOT NULL,
	Notes VARCHAR(MAX) 
)

INSERT INTO Employees VALUES 
('A', 'B', 'Service', NULL),
('C', 'D', 'DRDR', 'Some Notes'),
('E', 'F', 'SerDHvice', NULL)


CREATE TABLE Customers
(
	Id INT PRIMARY KEY IDENTITY,
	DriverLicenceNumber VARCHAR(100) NOT NULL,
	FullName VARCHAR(200) NOT NULL,
	[Address] VARCHAR(200) NOT NULL,
	City VARCHAR(100) NOT NULL, 
	ZIPCode INT NOT NULL, 
	Notes VARCHAR(MAX) 
)

INSERT INTO Customers VALUES 
('123123', 'A', 'awdwadwad', 'Sofia', 12345, NULL),
('123163', 'B', 'RFSEFSE', 'Dupnica', 54321, 'Some Notes'),
('125123', 'C', 'SEFESF', 'Pleven', 53234, NULL)


CREATE TABLE RentalOrders
(
	Id INT PRIMARY KEY IDENTITY,
	EmployeeId INT NOT NULL,
	CustomerId INT NOT NULL,
	CarId INT NOT NULL,
	TankLevel INT NOT NULL, 
	KilometrageStart INT NOT NULL, 
	KilometrageEnd INT NOT NULL, 
	TotalKilometrage INT NOT NULL, 
	StartDate DATETIME NOT NULL, 
	EndDate DATETIME NOT NULL, 
	TotalDays INT NOT NULL, 
	RateApplied DECIMAL(15,2) NOT NULL, 
	TaxRate DECIMAL(15,2), 
	OrderStatus BIT NOT NULL, 
	Notes VARCHAR(MAX) 
)

INSERT INTO RentalOrders VALUES 
(1, 2, 3, 6, 100, 200, 100, 2020-12-31, 2021-1-31, 31, 20.5, 10.5, 1, NULL),
(2, 2, 1, 6, 100, 200, 100, 2020-12-31, 2021-1-31, 31, 50.5, 30.5, 0, 'Some Notes'),
(3, 2, 1, 6, 100, 200, 100, 2020-12-31, 2021-1-31, 31, 40.5, 20.5, 1, NULL)

