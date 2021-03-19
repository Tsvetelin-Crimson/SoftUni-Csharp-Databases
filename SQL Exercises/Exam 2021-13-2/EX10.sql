USE ExamJan2021

SELECT u.Username, AVG(f.Size) AS [Size]
	FROM Commits c
	JOIN Users u ON u.Id = c.ContributorId
	JOIN Files f ON f.CommitId = c.Id
GROUP BY u.Username
ORDER BY Size DESC, u.Username