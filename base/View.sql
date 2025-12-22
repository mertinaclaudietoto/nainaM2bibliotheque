CREATE VIEW vw_LivreDetails AS
SELECT 
    l.id AS LivreId,
    l.nom AS LivreNom,
    l.photo AS LivrePhoto,
    l.dateentrebibliotheque AS DateEntree,
    l.dateedition AS DateEdition,
    g.id AS GenreId,
    g.nom AS GenreNom,
    a.id AS AuteurId,
    a.nom AS AuteurNom
FROM livre l
INNER JOIN genre g ON l.idgenre = g.id
INNER JOIN auteur a ON l.idauteur = a.id;
