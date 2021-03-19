CREATE TABLE Students 
(
	StudentID INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(50)
)

INSERT INTO Students VALUES 
('Mila'),
('Toni'),
('Ron')

CREATE TABLE Exams
(
	ExamID INT PRIMARY KEY IDENTITY(101, 1),
	[Name] NVARCHAR(100)
)

INSERT INTO Exams VALUES 
('SpringMVC'),
('Neo4j'),	
('Oracle 11g')

CREATE TABLE StudentsExams 
(
	StudentID INT REFERENCES Students(StudentID),
	ExamID INT REFERENCES Exams(ExamID),

	CONSTRAINT PK_Students_Exams PRIMARY KEY(StudentID, ExamID),
)

INSERT INTO StudentsExams VALUES
(1, 101),
(1, 102),
(2, 101),
(3, 103),
(2, 102),
(2, 103)

--1 Mila	101 SpringMVC	1 101
--2 Toni	102 Neo4j		1 102
--3 Ron		103 Oracle 11g  2 101

--3 103
--2 102
--2 103