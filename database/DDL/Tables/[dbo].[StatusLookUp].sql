IF (SELECT COUNT(*) FROM Sys.objects WHERE name ='StatusLookUp' and type='U') = 0
BEGIN
	CREATE TABLE [dbo].[StatusLookUp]
	(
		Code CHAR(2) NOT NULL PRIMARY KEY,
		Status CHAR(15) NOT NULL UNIQUE
	)
END
GO



