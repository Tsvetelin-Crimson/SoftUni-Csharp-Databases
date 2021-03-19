CREATE PROC usp_SearchForFiles(@fileExtension VARCHAR(20))
AS
	SELECT Id, Name, CONVERT(VARCHAR, Size) + 'KB' AS Size  
		FROM Files
	WHERE Name LIKE '%' + @fileExtension

GO

EXEC usp_SearchForFiles 'txt'