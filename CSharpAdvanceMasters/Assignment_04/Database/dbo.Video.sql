CREATE TABLE [dbo].[Table]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
    [Title] NVARCHAR(250) NULL, 
    [Year] INT NULL, 
    [IsRented] BIT NOT NULL, 
    [IsDeleted] BIT NOT NULL, 
    CONSTRAINT [PK_Video] PRIMARY KEY ([Id])
)
