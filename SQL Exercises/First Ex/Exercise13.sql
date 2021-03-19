CREATE TABLE Directors
(
	Id INT PRIMARY KEY IDENTITY,
	DirectorName VARCHAR(200) NOT NULL,
	Notes VARCHAR(MAX)
)

INSERT INTO Directors VALUES 
('A', 'some notes'),
('B', NULL),
('C', NULL),
('D', 'some other notes'),
('E', NULL)

CREATE TABLE Genres
(
	Id INT PRIMARY KEY IDENTITY,
	GenreName VARCHAR(200) NOT NULL,
	Notes VARCHAR(MAX)
)

INSERT INTO Genres VALUES 
('Action', 'some notes'),
('Basic', NULL),
('Comedy', NULL),
('Drama', 'some other notes'),
('Extra', NULL)

CREATE TABLE Categories
(
	Id INT PRIMARY KEY IDENTITY,
	CategoryName VARCHAR(200) NOT NULL,
	Notes VARCHAR(MAX)
)

INSERT INTO Categories VALUES 
('A', 'some notes'),
('C', NULL),
('B', NULL),
('E', 'some other notes'),
('D', NULL)


CREATE TABLE Movies
(
	Id INT PRIMARY KEY IDENTITY,
	Title VARCHAR(200) NOT NULL,
	DirectorId INT NOT NULL,
	CopyrightYear DATETIME NOT NULL,
	[Length] INT NOT NULL,
	GenreId INT NOT NULL,
	CategoryId INT NOT NULL,
	Rating INT,
	Notes VARCHAR(MAX)
)

INSERT INTO Movies VALUES 
('Superman', 1, 11/8/2000, 100, 1, 1, NULL, NULL),
('gdrx', 2, 11/10/2022, 104, 3, 4, NULL, NULL),
('Spiderman', 5, 18/10/2022, 102, 2, 4, NULL, NULL),
('awdaw', 4, 19/10/2022, 110, 1, 5, NULL, NULL),
('dadawd', 3, 20/10/2022, 111, 2, 5, NULL, NULL)