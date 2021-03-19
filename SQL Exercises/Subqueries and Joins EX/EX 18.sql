SELECT TOP(5) CountryName, PeakName AS [Highest Peak Name], HighestPeakElevation AS [Highest Peak Elevation], MountainRange AS Mountain
	FROM (
		SELECT c.CountryName,
		ISNULL(p.PeakName, '(no highest peak)') AS PeakName,
		ISNULL(MAX(p.Elevation), 0) AS HighestPeakElevation,
		ISNULL(m.MountainRange, '(no mountain)') AS MountainRange,
		DENSE_RANK() OVER (PARTITION BY c.CountryName ORDER BY MAX(p.Elevation) DESC) AS Ranked
			FROM Countries AS c
			LEFT JOIN MountainsCountries AS mc ON mc.CountryCode = c.CountryCode
			LEFT JOIN Mountains AS m ON mc.MountainId = m.Id
			LEFT JOIN Peaks AS p ON p.MountainId = m.Id
		GROUP BY c.CountryName, p.PeakName, m.MountainRange
	) AS e
	WHERE Ranked = 1
	ORDER BY CountryName