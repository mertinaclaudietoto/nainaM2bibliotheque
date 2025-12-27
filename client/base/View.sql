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

CREATE VIEW v_livre_user AS
select  nom,
        prenom,
        email,
        login,
        e.id as IdEmprunt ,
        iduser,
        idlivre,
        dateemprunt,
        datelimite,
        dateretouredelivre
        from emprunt e join users u on e.iduser=u.id;

CREATE VIEW v_livre_emprunt AS
select 
    v.LivreId,
    v.LivreNom,
    v.LivrePhoto,
    v.DateEntree,
    v.DateEdition,
    v.GenreId,
    v.GenreNom,
    v.AuteurId,
    v.AuteurNom,
    dateemprunt,
    datelimite,
    dateretouredelivre,
    nom,
    prenom,
    email,
    login
    from vw_LivreDetails v left join v_livre_user e
    on v.LivreId=e.idlivre;