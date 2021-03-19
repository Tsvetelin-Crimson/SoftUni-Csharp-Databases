CREATE OR ALTER FUNCTION udf_AllUserCommits(@username VARCHAR(30))
RETURNS INT
AS
BEGIN
	DECLARE @UserId INT = (SELECT Id FROM Users WHERE Username = @username)
	RETURN (SELECT COUNT(*) FROM Commits WHERE ContributorId = @UserId)
END
GO

SELECT dbo.udf_AllUserCommits('UnderSinduxrein')