USE ExamJan2021

SELECT Id, Message, RepositoryId, ContributorId FROM Commits
ORDER BY Id, Message, RepositoryId, ContributorId