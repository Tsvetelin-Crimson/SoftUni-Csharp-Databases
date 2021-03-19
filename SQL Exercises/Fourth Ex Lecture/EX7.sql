SELECT TownID ,[Name] 
FROM Towns
WHERE LEFT([Name], 1)  LIKE '[^R,B,D]'
ORDER BY [Name]