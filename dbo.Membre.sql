CREATE TABLE [dbo].[Membre] (
    [Id]    INT            NOT NULL,
    [Name]  NVARCHAR (255) NOT NULL,
    [Phone] NVARCHAR (50)  NULL,
    [Email] NVARCHAR (200) UNIQUE NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

