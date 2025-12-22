INSERT INTO users (nom, prenom, email, login, motdepasse, datedenaissance) VALUES
('Rakoto', 'Jean', 'jean.rakoto@email.com', 'jrakoto', 'pass123', '1995-03-12'),
('Rabe', 'Marie', 'marie.rabe@email.com', 'mrabe', 'pass123', '1998-07-25'),
('Andrianina', 'Paul', 'paul.andrianina@email.com', 'pandrianina', 'pass123', '1992-11-03');

INSERT INTO genre (nom) VALUES
('Roman'),
('Science-fiction'),
('Histoire'),
('Informatique');


INSERT INTO auteur (nom) VALUES
('Victor Hugo'),
('Isaac Asimov'),
('Yuval Noah Harari'),
('Robert C. Martin');

INSERT INTO livre (photo,nom, idgenre, idauteur, dateedition) VALUES
('https://res.cloudinary.com/dcufspbrh/image/upload/v1766328927/82fdb93d4b3f19641dee5ccb48bb8173_ilsrhr.jpg','Les Misérables', 1, 1, '1862-01-01'),
('https://res.cloudinary.com/dcufspbrh/image/upload/v1766328927/82fdb93d4b3f19641dee5ccb48bb8173_ilsrhr.jpg','Fondation', 2, 2, '1951-06-01'),
('https://res.cloudinary.com/dcufspbrh/image/upload/v1766328927/82fdb93d4b3f19641dee5ccb48bb8173_ilsrhr.jpg','Sapiens', 3, 3, '2011-01-01'),
('https://res.cloudinary.com/dcufspbrh/image/upload/v1766328927/82fdb93d4b3f19641dee5ccb48bb8173_ilsrhr.jpg','Clean Code', 4, 4, '2008-08-01');

INSERT INTO livre (photo, nom, idgenre, idauteur, dateedition) VALUES
('https://res.cloudinary.com/dcufspbrh/image/upload/v1766328927/82fdb93d4b3f19641dee5ccb48bb8173_ilsrhr.jpg','Les Misérables',1,1,'1862-01-01'),
('https://res.cloudinary.com/dcufspbrh/image/upload/v1766328927/82fdb93d4b3f19641dee5ccb48bb8173_ilsrhr.jpg','Fondation',2,2,'1951-06-01'),
('https://res.cloudinary.com/dcufspbrh/image/upload/v1766328927/82fdb93d4b3f19641dee5ccb48bb8173_ilsrhr.jpg','Sapiens',3,3,'2011-01-01'),
('https://res.cloudinary.com/dcufspbrh/image/upload/v1766328927/82fdb93d4b3f19641dee5ccb48bb8173_ilsrhr.jpg','Clean Code',4,4,'2008-08-01'),
('https://res.cloudinary.com/dcufspbrh/image/upload/v1766328927/82fdb93d4b3f19641dee5ccb48bb8173_ilsrhr.jpg','1984',1,1,'1949-06-08'),
('https://res.cloudinary.com/dcufspbrh/image/upload/v1766328927/82fdb93d4b3f19641dee5ccb48bb8173_ilsrhr.jpg','Le Petit Prince',2,2,'1943-04-06'),
('https://res.cloudinary.com/dcufspbrh/image/upload/v1766328927/82fdb93d4b3f19641dee5ccb48bb8173_ilsrhr.jpg','Harry Potter à l’école des sorciers',3,3,'1997-06-26'),
('https://res.cloudinary.com/dcufspbrh/image/upload/v1766328927/82fdb93d4b3f19641dee5ccb48bb8173_ilsrhr.jpg','Le Seigneur des Anneaux',4,4,'1954-07-29'),
('https://res.cloudinary.com/dcufspbrh/image/upload/v1766328927/82fdb93d4b3f19641dee5ccb48bb8173_ilsrhr.jpg','Le Comte de Monte-Cristo',1,1,'1844-08-28'),
('https://res.cloudinary.com/dcufspbrh/image/upload/v1766328927/82fdb93d4b3f19641dee5ccb48bb8173_ilsrhr.jpg','Les Fleurs du mal',2,2,'1857-06-25'),
('https://res.cloudinary.com/dcufspbrh/image/upload/v1766328927/82fdb93d4b3f19641dee5ccb48bb8173_ilsrhr.jpg','Pride and Prejudice',3,3,'1813-01-28'),
('https://res.cloudinary.com/dcufspbrh/image/upload/v1766328927/82fdb93d4b3f19641dee5ccb48bb8173_ilsrhr.jpg','Moby Dick',4,4,'1851-10-18'),
('https://res.cloudinary.com/dcufspbrh/image/upload/v1766328927/82fdb93d4b3f19641dee5ccb48bb8173_ilsrhr.jpg','Les Trois Mousquetaires',1,1,'1844-03-14'),
('https://res.cloudinary.com/dcufspbrh/image/upload/v1766328927/82fdb93d4b3f19641dee5ccb48bb8173_ilsrhr.jpg','Don Quichotte',2,2,'1605-01-16'),
('https://res.cloudinary.com/dcufspbrh/image/upload/v1766328927/82fdb93d4b3f19641dee5ccb48bb8173_ilsrhr.jpg','Anna Karénine',3,3,'1877-01-01'),
('https://res.cloudinary.com/dcufspbrh/image/upload/v1766328927/82fdb93d4b3f19641dee5ccb48bb8173_ilsrhr.jpg','Crime et Châtiment',4,4,'1866-01-01'),
('https://res.cloudinary.com/dcufspbrh/image/upload/v1766328927/82fdb93d4b3f19641dee5ccb48bb8173_ilsrhr.jpg','Le Parfum',1,1,'1985-10-18'),
('https://res.cloudinary.com/dcufspbrh/image/upload/v1766328927/82fdb93d4b3f19641dee5ccb48bb8173_ilsrhr.jpg','Dracula',2,2,'1897-05-26'),
('https://res.cloudinary.com/dcufspbrh/image/upload/v1766328927/82fdb93d4b3f19641dee5ccb48bb8173_ilsrhr.jpg','Frankenstein',3,3,'1818-01-01'),
('https://res.cloudinary.com/dcufspbrh/image/upload/v1766328927/82fdb93d4b3f19641dee5ccb48bb8173_ilsrhr.jpg','Le Hobbit',4,4,'1937-09-21'),
('https://res.cloudinary.com/dcufspbrh/image/upload/v1766328927/82fdb93d4b3f19641dee5ccb48bb8173_ilsrhr.jpg','L’Alchimiste',1,1,'1988-04-01'),
('https://res.cloudinary.com/dcufspbrh/image/upload/v1766328927/82fdb93d4b3f19641dee5ccb48bb8173_ilsrhr.jpg','Twilight',2,2,'2005-10-05'),
('https://res.cloudinary.com/dcufspbrh/image/upload/v1766328927/82fdb93d4b3f19641dee5ccb48bb8173_ilsrhr.jpg','Le Livre des Baltimore',3,3,'2015-09-15'),
('https://res.cloudinary.com/dcufspbrh/image/upload/v1766328927/82fdb93d4b3f19641dee5ccb48bb8173_ilsrhr.jpg','La Vérité sur l’Affaire Harry Quebert',4,4,'2012-08-01'),
('https://res.cloudinary.com/dcufspbrh/image/upload/v1766328927/82fdb93d4b3f19641dee5ccb48bb8173_ilsrhr.jpg','Les Piliers de la Terre',1,1,'1989-09-01'),
('https://res.cloudinary.com/dcufspbrh/image/upload/v1766328927/82fdb93d4b3f19641dee5ccb48bb8173_ilsrhr.jpg','Shining',2,2,'1977-01-28'),
('https://res.cloudinary.com/dcufspbrh/image/upload/v1766328927/82fdb93d4b3f19641dee5ccb48bb8173_ilsrhr.jpg','Le Cycle des Robots',3,3,'1950-12-02'),
('https://res.cloudinary.com/dcufspbrh/image/upload/v1766328927/82fdb93d4b3f19641dee5ccb48bb8173_ilsrhr.jpg','La Planète des Singes',4,4,'1963-10-01'),
('https://res.cloudinary.com/dcufspbrh/image/upload/v1766328927/82fdb93d4b3f19641dee5ccb48bb8173_ilsrhr.jpg','Les Chroniques de Narnia',1,1,'1950-10-16'),
('https://res.cloudinary.com/dcufspbrh/image/upload/v1766328927/82fdb93d4b3f19641dee5ccb48bb8173_ilsrhr.jpg','Game of Thrones',2,2,'1996-08-06'),
('https://res.cloudinary.com/dcufspbrh/image/upload/v1766328927/82fdb93d4b3f19641dee5ccb48bb8173_ilsrhr.jpg','Hunger Games',3,3,'2008-09-14'),
('https://res.cloudinary.com/dcufspbrh/image/upload/v1766328927/82fdb93d4b3f19641dee5ccb48bb8173_ilsrhr.jpg','Divergente',4,4,'2011-04-25'),
('https://res.cloudinary.com/dcufspbrh/image/upload/v1766328927/82fdb93d4b3f19641dee5ccb48bb8173_ilsrhr.jpg','Percy Jackson',1,1,'2005-06-28'),
('https://res.cloudinary.com/dcufspbrh/image/upload/v1766328927/82fdb93d4b3f19641dee5ccb48bb8173_ilsrhr.jpg','Les Gardiens de Ga’Hoole',2,2,'2003-09-01'),
('https://res.cloudinary.com/dcufspbrh/image/upload/v1766328927/82fdb93d4b3f19641dee5ccb48bb8173_ilsrhr.jpg','Eragon',3,3,'2002-06-01'),
('https://res.cloudinary.com/dcufspbrh/image/upload/v1766328927/82fdb93d4b3f19641dee5ccb48bb8173_ilsrhr.jpg','Le Cycle de Fondation',4,4,'1951-06-01'),
('https://res.cloudinary.com/dcufspbrh/image/upload/v1766328927/82fdb93d4b3f19641dee5ccb48bb8173_ilsrhr.jpg','Les Rouges et les Noirs',1,1,'1830-11-01'),
('https://res.cloudinary.com/dcufspbrh/image/upload/v1766328927/82fdb93d4b3f19641dee5ccb48bb8173_ilsrhr.jpg','La Métamorphose',2,2,'1915-11-01');


INSERT INTO emprunt (iduser, idlivre, datelimite, dateretouredelivre) VALUES
(1, 1, '2025-01-15', NULL),     -- Jean emprunte Les Misérables (non retourné)
(2, 2, '2025-01-20', '2025-01-10'), -- Marie a retourné Fondation
(3, 4, '2025-01-18', NULL);     -- Paul emprunte Clean Code
