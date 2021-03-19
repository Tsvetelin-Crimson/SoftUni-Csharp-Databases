CREATE TABLE Users
(
	Id BIGINT PRIMARY KEY IDENTITY,
	Username VARCHAR(30) NOT NULL,
	[Password] VARCHAR(26) NOT NULL,
	ProfilePicture NVARCHAR(MAX),
	LastLoginTime DATETIME,
	IsDeleted BIT
)

INSERT INTO Users VALUES 
('Vanio', 'Pass123', 'http/...', '11/8/2000', 0),
('Galia', 'Pass1235', 'http/...', GETDATE(), 0),
('Ivan', 'Pass123778', 'http/...', '9/18/2000', 0),
('Bogdan', 'Pass123213', 'http/...', '11/8/2000', 1),
('Jordan', 'Pass1232342', 'http/...', '11/8/2000', 0)

