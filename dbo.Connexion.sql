CREATE TABLE [dbo].[Connexion] (
    [idm]      INT           NULL,
    [courriel] VARCHAR (100) NOT NULL,
    [pass]     VARCHAR (12)  NOT NULL,
    [role]     CHAR (1)      DEFAULT ('M') NULL,
    [statut]   CHAR (1)      DEFAULT ('A') NULL,
    FOREIGN KEY ([idm]) REFERENCES [dbo].[Membre] ([idm]) ON DELETE CASCADE
);

