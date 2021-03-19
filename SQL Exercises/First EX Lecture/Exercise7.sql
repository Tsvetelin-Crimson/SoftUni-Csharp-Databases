CREATE TABLE People
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(200) NOT NULL,
	Picture NVARCHAR(MAX),
	Height DECIMAL(10, 2),
	[Weight] DECIMAL(10, 2),
	Gender VARCHAR(1) NOT NULL,
	Birthdate DATETIME NOT NULL,
	Biography NVARCHAR(MAX)
)

INSERT INTO People VALUES 
('Vanio', '1234', 1.8, 80.9, 'm', '11/8/2000', NULL),
('Galia', '1234', 1.7, 70.9, 'f', '10/20/2000', 'some biography'),
('Ivan', '1234', 1.9, 81.9, 'm', '9/18/2000', NULL),
('Bogdan', '1234', 1.7, 60.9, 'm', '11/8/2000', NULL),
('Jordan', '1234', 1.8, 80.9, 'f', '11/8/2000', NULL)

