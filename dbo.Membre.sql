CREATE TABLE [dbo].[Membre] (
    [idm]      INT           IDENTITY (1, 1) NOT NULL,
    [nom]      VARCHAR (100) NOT NULL,
    [prenom]   VARCHAR (100) NOT NULL,
    [courriel] VARCHAR (100) NOT NULL,
    [daten]    DATE          NULL,
    PRIMARY KEY CLUSTERED ([idm] ASC)
);

