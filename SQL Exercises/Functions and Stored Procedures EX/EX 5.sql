CREATE FUNCTION ufn_GetSalaryLevel(@Salary DECIMAL(18,4))
RETURNS NVARCHAR(20)
AS
BEGIN
	DECLARE @Result NVARCHAR(20) = '' ;
	
	IF @Salary < 30000
		BEGIN
			SET @Result = 'Low';
		END
	ELSE IF @Salary <= 50000
		BEGIN
			SET @Result = 'Average';
		END
	ELSE IF @Salary > 50000
		BEGIN
			SET @Result = 'High';
		END

	RETURN @Result;
END;

SELECT Salary, dbo.ufn_GetSalaryLevel(Salary) FROM Employees

