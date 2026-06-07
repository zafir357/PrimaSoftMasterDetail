CREATE DATABASE AnnuaireEntreprise;
GO
USE AnnuaireEntreprise;
GO

CREATE TABLE societe (
    id INT IDENTITY(1,1) PRIMARY KEY,
    nom NVARCHAR(100), adresse NVARCHAR(100), adresse2 NVARCHAR(100),
    codePostal NVARCHAR(100), ville NVARCHAR(100), standard NVARCHAR(100)
);

CREATE TABLE contact (
    id INT IDENTITY(1,1) PRIMARY KEY,
    idSociete INT NOT NULL FOREIGN KEY REFERENCES societe(id),
    civilite NVARCHAR(100), nom NVARCHAR(100),
    prenom NVARCHAR(100), fonction NVARCHAR(100)
);

CREATE TABLE infoContact (
    id INT IDENTITY(1,1) PRIMARY KEY,
    idContact INT NOT NULL FOREIGN KEY REFERENCES contact(id),
    typeInfo NVARCHAR(100), info NVARCHAR(100)
);