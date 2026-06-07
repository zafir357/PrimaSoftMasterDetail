USE AnnuaireEntreprise;
GO

-- =============================================
-- DUMMY DATA — Annuaire Contacts / Sociétés
-- =============================================

-- 1. SOCIÉTÉS
INSERT INTO societe (nom, adresse, adresse2, codePostal, ville, standard) VALUES
('TECHCORP',         '12 Avenue de la République', 'Bâtiment B', '75011', 'Paris',     '0145678901'),
('INNOV SOLUTIONS',  '3 Rue des Lilas',            NULL,         '69003', 'Lyon',      '0472345678'),
('NEXASOFT',         '8 Boulevard Haussmann',      'Étage 4',    '75009', 'Paris',     '0155443322'),
('ALPHATECH',        '22 Rue du Commerce',         NULL,         '33000', 'Bordeaux',  '0556789012'),
('DATASPHERE',       '15 Rue Nationale',           NULL,         '59000', 'Lille',     '0320112233');
GO

-- 2. CONTACTS
INSERT INTO contact (idSociete, civilite, nom, prenom, fonction) VALUES
-- TECHCORP (id=1)
(1, 'Mr.',  'DURAND',   'Thomas',   'Développeur Backend'),
(1, 'Mme.', 'LEFEBVRE', 'Camille',  'UX Designer'),
(1, 'Mr.',  'BERNARD',  'Lucas',    'DevOps Engineer'),
-- INNOV SOLUTIONS (id=2)
(2, 'Mme.', 'PETIT',    'Emma',     'Directrice Technique'),
(2, 'Mr.',  'MOREAU',   'Antoine',  'Développeur Full Stack'),
-- NEXASOFT (id=3)
(3, 'Mr.',  'SIMON',    'Hugo',     'Architecte Logiciel'),
(3, 'Mme.', 'LAURENT',  'Léa',      'Product Owner'),
-- ALPHATECH (id=4)
(4, 'Mr.',  'MICHEL',   'Pierre',   'Développeur Senior'),
(4, 'Mme.', 'GARCIA',   'Sofia',    'Responsable QA'),
-- DATASPHERE (id=5)
(5, 'Mr.',  'ROBERT',   'Nicolas',  'Data Engineer'),
(5, 'Mme.', 'HENRY',    'Julie',    'Chef de Projet');
GO

-- 3. INFOS CONTACT
INSERT INTO infoContact (idContact, typeInfo, info) VALUES
-- DURAND Thomas (id=1)
(1,  'TELEPHONE', '0622334455'),
(1,  'EMAIL',     'tdurand@techcorp.fr'),
-- LEFEBVRE Camille (id=2)
(2,  'EMAIL',     'clefebvre@techcorp.fr'),
(2,  'TELEPHONE', '0633445566'),
-- BERNARD Lucas (id=3)
(3,  'EMAIL',     'lbernard@techcorp.fr'),
(3,  'TELEPHONE', '0644556677'),
(3,  'GITHUB',    'github.com/lucasbernard'),
-- PETIT Emma (id=4)
(4,  'TELEPHONE', '0655667788'),
(4,  'EMAIL',     'epetit@innovsolutions.fr'),
-- MOREAU Antoine (id=5)
(5,  'EMAIL',     'amoreau@innovsolutions.fr'),
(5,  'TELEPHONE', '0666778899'),
(5,  'LINKEDIN',  'linkedin.com/in/antoinemoreau'),
-- SIMON Hugo (id=6)
(6,  'TELEPHONE', '0677889900'),
(6,  'EMAIL',     'hsimon@nexasoft.fr'),
-- LAURENT Léa (id=7)
(7,  'EMAIL',     'llaurent@nexasoft.fr'),
(7,  'TELEPHONE', '0688990011'),
-- MICHEL Pierre (id=8)
(8,  'TELEPHONE', '0699001122'),
(8,  'EMAIL',     'pmichel@alphatech.fr'),
-- GARCIA Sofia (id=9)
(9,  'EMAIL',     'sgarcia@alphatech.fr'),
(9,  'TELEPHONE', '0611223344'),
(9,  'LINKEDIN',  'linkedin.com/in/sofiagarcia'),
-- ROBERT Nicolas (id=10)
(10, 'TELEPHONE', '0320998877'),
(10, 'EMAIL',     'nrobert@datasphere.fr'),
-- HENRY Julie (id=11)
(11, 'EMAIL',     'jhenry@datasphere.fr'),
(11, 'TELEPHONE', '0321334455'),
(11, 'LINKEDIN',  'linkedin.com/in/juliehenry');
GO

-- Vérification
SELECT s.nom AS Société, c.prenom + ' ' + c.nom AS Contact, i.typeInfo, i.info
FROM societe s
JOIN contact c ON c.idSociete = s.id
JOIN infoContact i ON i.idContact = c.id
ORDER BY s.nom, c.nom, i.typeInfo;