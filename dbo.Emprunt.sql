CREATE TABLE [dbo].[Emprunt] (
    [id]           INT  IDENTITY (1, 1) NOT NULL,
    [id_livre]     INT  NULL,
    [id_membre]    INT  NULL,
    [date_emprunt] DATE NULL,
    [date_retour]  DATE NULL,
	[date_retour_eff]  DATE NULL,

    PRIMARY KEY CLUSTERED ([id] ASC),
    FOREIGN KEY ([id_livre]) REFERENCES [dbo].[Livres] ([id]) ON DELETE CASCADE,
    FOREIGN KEY ([id_membre]) REFERENCES [dbo].[Membre] ([idm]) ON DELETE CASCADE
);

