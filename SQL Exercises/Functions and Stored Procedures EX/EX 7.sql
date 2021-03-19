CREATE FUNCTION ufn_IsWordComprised(@setOfLetters VARCHAR(MAX), @word VARCHAR(200)) 
RETURNS BIT
AS
BEGIN
	DECLARE @WordLen INT = LEN(@word)
	WHILE (@WordLen > 0)
	BEGIN 
		DECLARE @CurrLetter VARCHAR(1) = SUBSTRING(@word, @WordLen, 1)
		IF CHARINDEX(@CurrLetter, @setOfLetters) = 0
			RETURN 0
		SET @WordLen -= 1;
	END
	RETURN 1
END