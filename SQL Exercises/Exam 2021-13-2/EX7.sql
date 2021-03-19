USE ExamJan2021

SELECT i.Id, u.Username + ' : ' + i.Title AS IssueAssignee
	FROM Issues i
	JOIN Users u ON u.Id = i.AssigneeId
ORDER BY i.Id DESC, i.AssigneeId 