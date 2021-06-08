IF (SELECT COUNT(*) FROM Sys.objects WHERE name ='Tasks' and type='U') = 0
BEGIN
	CREATE TABLE [dbo].[Tasks]
	( 
		Id INT IDENTITY(1,1) PRIMARY KEY,
		Name VARCHAR(100) NOT NULL,
		Description VARCHAR(1000) NULL,
		AssignedTo CHAR(50) NOT NULL,
		StartDate DATE NOT NULL,
		FinishDate DATE NULL,
		State CHAR(2) NOT NULL FOREIGN KEY REFERENCES StatusLookUp(Code),
		ParentTask INT NULL
	)
END
GO

