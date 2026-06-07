# Annuaire Entreprise

1. Application Windows Forms (master-detail) permettant de gérer un annuaire de contacts professionnels regroupés par société.
2. Projet Tests
## Prérequis

- Windows 10/11
- .NET 10 (net10.0-windows)
- SQL Server (local ou distant)
- DevExpress 25.2 installé sur la machine

## Mise en place de la base de données

### 1. Créer la base et les tables

Ouvrir **SQL Server Management Studio** (ou Azure Data Studio), puis exécuter le script :

```
AnnuaireEntreprise/Scripts/Creation_des_tables.sql
```

Ce script crée la base `AnnuaireEntreprise` et les trois tables :

| Table | Description |
|---|---|
| `societe` | Fiche société (nom, adresse, standard) |
| `contact` | Contact rattaché à une société |
| `infoContact` | Informations du contact (téléphone, email, LinkedIn…) |

### 2. Insérer les données de démonstration (optionnel)

```
AnnuaireEntreprise/Scripts/Insertion.sql
```

Insère 5 sociétés, 11 contacts et leurs informations de contact à titre d'exemple.

### 3. Vérifier la chaîne de connexion

Ouvrir le fichier `Dataset/AnnuaireDataset.Designer.cs` et remplacer les **3 occurrences** de :

```csharp
"Data Source=localhost;Initial Catalog=AnnuaireEntreprise;Integrated Security=True;TrustServerCertificate=True"
```

par ta propre connection string. Exemples :

- **LocalDB** : `Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=AnnuaireEntreprise;Integrated Security=True;TrustServerCertificate=True`
- **SQL Express** : `Data Source=.\SQLEXPRESS;Initial Catalog=AnnuaireEntreprise;Integrated Security=True;TrustServerCertificate=True`
- **SQL Server distant** : `Data Source=MON-SERVEUR;Initial Catalog=AnnuaireEntreprise;User Id=monuser;Password=monpwd;TrustServerCertificate=True`

> ⚠️ Cette modification est écrasée si le DataSet est régénéré depuis le concepteur.


### Option B — Reconfiguration via le concepteur (propre)
- Ouvrir `Dataset/AnnuaireDataset.xsd` dans Visual Studio.
-  Pour chaque TableAdapter (`societe`, `contact`, `infoContact`) : clic droit →
   **Configurer…**, puis dans l'assistant, **Nouvelle connexion…** et pointer vers
   votre serveur SQL.
  - Terminer l'assistant : Visual Studio régénère `Designer.cs` avec votre chaîne.


### 4. Projet de démarrage
- **CLick droit sur la solution AnnuaireEntreprise-> Configurer des projet Start-Up, Choisir AnnuaireEntreprise**


## Fonctionnalités

- **Liste principale** : affichage des informations de contact groupées par société puis par contact, avec barre de recherche intégrée.
- **Nouveau contact** : bouton pour ajouter un contact (et sa société si elle n'existe pas encore).
- **Double-clic pour modification** sur une ligne : ouvre la fiche complète de la société avec ses contacts pour modification.
- **Petite croix Rouge sur info-contact** pour la suppression de info-contact.

## Tests unitaires (AnnuaireEntreprise.Tests)

Le projet `AnnuaireEntreprise.Tests` contient des tests xUnit qui vérifient les règles de validation des champs (nom de société, standard téléphonique, code postal).

### Exécuter les tests

**Depuis Visual Studio** : click-droit sur le projet AnnuaireEntreprise.Tests ** > Exécuter tous les tests** (ou `Ctrl+R, A`).


Les tests couvrent :

| Méthode testée | Cas valides | Cas invalides |
|---|---|---|
| `NomSocieteValide` | Nom non vide | Vide, espaces, null |
| `StandardValide` | 7 chiffres exacts, vide, null | Trop court/long, lettres, espaces |
| `CodePostalValide` | 5 chiffres exacts, vide, null | Trop court/long, lettres, espaces |

## Structure du projet

```
AnnuaireEntreprise/
├── Forms/
│   ├── FormListe.cs       # Fenêtre principale (liste master-detail)
│   ├── FormContact.cs     # Formulaire d'ajout de contact
│   └── FormSociete.cs     # Fiche société + contacts
├── Dataset/
│   └── AnnuaireDataset.xsd  # DataSet typé (TableAdapters générés)
└── Scripts/
    ├── Creation_des_tables.sql
    └── Insertion.sql
AnnuaireEntreprise.Tests/
├── ValidationTests.cs     # Tests xUnit sur les règles de validation
└── Helpers/
    └── ValidationHelper.cs
```
