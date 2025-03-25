CREATE TABLE [dbo].[Pret] (
    [Id]         INT      IDENTITY (1, 1) NOT NULL,
    [BookId]     INT      NULL,
    [MembreId]   INT      NULL,
    [DatePret]   DATETIME DEFAULT (getdate()) NULL,
    [DateRetour] DATETIME NULL,
    [NombrePret] INT      NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([BookId]) REFERENCES [dbo].[Livres] ([Id]),
    FOREIGN KEY ([MembreId]) REFERENCES [dbo].[Membre] ([Id])
);
/*need to add trigger to control book numbers <=3*/
