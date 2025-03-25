


CREATE TABLE [dbo].[Livres] (
    [Id]    INT           IDENTITY (1, 1) NOT NULL,
    [Titre] NVARCHAR (60) NULL,
	[Categorie] NVARCHAR(60) 
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

