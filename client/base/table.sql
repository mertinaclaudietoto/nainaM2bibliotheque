-- TABLE USERS
CREATE TABLE users (
    id INT IDENTITY(1,1) PRIMARY KEY,
    nom NVARCHAR(255) NOT NULL,
    prenom NVARCHAR(255) NOT NULL,
    email NVARCHAR(255) UNIQUE,
    login NVARCHAR(255) UNIQUE NOT NULL,
    motdepasse NVARCHAR(255) NOT NULL,
    datedenaissance DATE
);
ALTER TABLE [users]
ADD dateinscription DATE DEFAULT GETDATE();


-- TABLE GENRE
CREATE TABLE genre (
    id INT IDENTITY(1,1) PRIMARY KEY,
    nom NVARCHAR(255) NOT NULL
);

-- TABLE AUTEUR
CREATE TABLE auteur (
    id INT IDENTITY(1,1) PRIMARY KEY,
    nom NVARCHAR(255) NOT NULL
);

-- TABLE LIVRE
CREATE TABLE livre (
    id INT IDENTITY(1,1) PRIMARY KEY,
    photo TEXT,
    nom NVARCHAR(255) NOT NULL,
    idgenre INT NOT NULL,
    idauteur INT NOT NULL,
    dateentrebibliotheque DATE DEFAULT GETDATE(),
    dateedition DATE,

    CONSTRAINT fk_livre_genre
        FOREIGN KEY (idgenre) REFERENCES genre(id),

    CONSTRAINT fk_livre_auteur
        FOREIGN KEY (idauteur) REFERENCES auteur(id)
);

-- TABLE EMPRUNT
CREATE TABLE emprunt (
    id INT IDENTITY(1,1) PRIMARY KEY,
    iduser INT NOT NULL,
    idlivre INT NOT NULL,
    dateemprunt DATE DEFAULT GETDATE(),
    datelimite DATE NOT NULL,
    dateretouredelivre DATE,

    CONSTRAINT fk_emprunt_user
        FOREIGN KEY (iduser) REFERENCES users(id),

    CONSTRAINT fk_emprunt_livre
        FOREIGN KEY (idlivre) REFERENCES livre(id)
);
