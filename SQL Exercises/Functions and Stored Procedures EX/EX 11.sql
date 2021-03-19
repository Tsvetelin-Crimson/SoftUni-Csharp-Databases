CREATE FUNCTION ufn_CalculateFutureValue (@InitSum DECIMAL(15,2), @YearlyRate FLOAT, @Years INT)
RETURNS DECIMAL(17, 4)
AS
BEGIN
DECLARE @Result DECIMAL(17, 4)  = @InitSum * (POWER(1 + @YearlyRate, @Years))

RETURN @Result
END
-- I X ((1 + R)^T)

SELECT dbo.ufn_CalculateFutureValue(1000, 0.1, 5)