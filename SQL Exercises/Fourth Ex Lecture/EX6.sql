SELECT TownID ,[Name] 
FROM Towns
WHERE LEFT([Name], 1)  LIKE '[M,K,B,E]'
ORDER BY [Name]