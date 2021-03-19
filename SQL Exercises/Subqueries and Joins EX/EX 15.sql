SELECT ContinentCode, CurrencyCode, Usage AS CurrencyUsage FROM 
(SELECT ContinentCode, CurrencyCode, COUNT(CurrencyCode) AS Usage,
	DENSE_RANK() OVER (PARTITION BY ContinentCode ORDER BY COUNT(CurrencyCode) DESC) AS Ranked
	FROM Countries
	GROUP BY CurrencyCode, ContinentCode
	) AS u
	WHERE Ranked = 1 AND Usage > 1
	ORDER BY ContinentCode

	--ROW_NUMBER() OVER (PARTITION BY Usage ORDER BY Usage) AS Ranked