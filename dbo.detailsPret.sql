CREATE TABLE [dbo].[DetailsPret]
(
	Id INT PRIMARY KEY IDENTITY(1,1),
    IdPret INT NOT NULL,
    IdLivre INT NOT NULL,
    FOREIGN KEY (IdPret) REFERENCES Pret(Id),
    FOREIGN KEY (IdLivre) REFERENCES Livres(Id)

);
