CREATE TABLE [dbo].[Livres] (
    [id]          INT            IDENTITY (1, 1) NOT NULL,
    [titre]       NVARCHAR (100) NOT NULL,
    [annee]       INT            NULL,
    [nom_auteur]  VARCHAR (100)  NULL,
    [idcateg]     INT            NULL,
    [exemplaires] INT            DEFAULT ((1)) NULL,
    PRIMARY KEY CLUSTERED ([id] ASC),
    FOREIGN KEY ([idcateg]) REFERENCES [dbo].[categories] ([idc]) ON DELETE CASCADE
);

