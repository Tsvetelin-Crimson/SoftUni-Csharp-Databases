USE ExamJan2021

SELECT ff.Id, ff.Name, CONVERT(VARCHAR, ff.Size) + 'KB' AS Size 
		FROM Files ff
		LEFT JOIN Files cf ON cf.ParentId = ff.Id
	WHERE cf.Id IS NULL
ORDER BY ff.Id, ff.Name, ff.Size DESC
