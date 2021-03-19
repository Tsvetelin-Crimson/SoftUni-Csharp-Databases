SELECT e.EmployeeID, e.FirstName,
	CASE 
		WHEN p.StartDate >= '2005' THEN NULL
		ELSE p.Name
	END
		AS [ProjectName]
	FROM Employees AS e
 	JOIN EmployeesProjects AS ep ON e.EmployeeID = ep.EmployeeID 
 	LEFT JOIN Projects AS p ON p.ProjectID = ep.ProjectID  
	WHERE e.EmployeeID = 24 --p.StartDate > '2002-08-13' AND p.EndDate IS NULL 
ORDER BY e.EmployeeID 