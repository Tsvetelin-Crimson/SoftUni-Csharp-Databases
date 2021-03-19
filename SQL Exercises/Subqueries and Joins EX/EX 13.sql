SELECT c.CountryCode, COUNT(*)
	FROM Countries c
	JOIN MountainsCountries mc ON mc.CountryCode = c.CountryCode
		WHERE c.CountryCode IN ('US', 'RU', 'BG')
	GROUP BY c.CountryCode
