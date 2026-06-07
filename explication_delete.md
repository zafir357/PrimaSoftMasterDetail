# Implémentation de la suppression d'une ligne dans une GridView DevExpress

## Contexte

Dans le formulaire `FormSociete`, la grille `gridView2` affiche les informations de contact
(téléphone, email, etc.) liées à un contact sélectionné. L'objectif était d'ajouter une colonne
de suppression avec une croix rouge, visible sur chaque ligne, qui supprime l'information en base
quand on clique dessus.

---

## Étape 1 — Déclarer le champ `Delete` dans la classe

DevExpress ne permet pas de référencer une colonne entre plusieurs méthodes si elle n'est pas
déclarée au niveau de la classe. On déclare donc `Delete` comme champ :

```csharp
// Colonne Delete déclarée ici car PopulateColumns() la supprime à chaque chargement.
// On la recrée et reconfigure manuellement dans FormSociete_Load.
private readonly DevExpress.XtraGrid.Columns.GridColumn Delete =
    new DevExpress.XtraGrid.Columns.GridColumn();
```

> Pourquoi `readonly` ? La référence vers l'objet colonne ne change jamais — seul son contenu
> (caption, width…) est modifié. `readonly` rend ça explicite.

---

## Étape 2 — Pourquoi `PopulateColumns()` pose problème

Quand on assigne un `DataSource` à la grille puis qu'on appelle `PopulateColumns()`, DevExpress
régénère toutes les colonnes depuis les champs du DataSource. Toute colonne ajoutée manuellement
avant (y compris `Delete`) est **supprimée** de la collection.

```csharp
gridView2.PopulateColumns(); // Régénère les colonnes depuis le DataSource → Delete disparaît
gridView2.Columns.Add(Delete); // On la remet manuellement à la fin
```

Il faut donc toujours ajouter `Delete` **après** `PopulateColumns()`.

---

## Étape 3 — Configurer la colonne Delete

```csharp
// Dernière position visible dans la grille
Delete.VisibleIndex = gridView2.VisibleColumns.Count;

// Pas de titre — la croix rouge est l'indicateur visuel suffisant
Delete.Caption = "";

// Largeur fixe, juste assez large pour contenir la croix
Delete.Width = 40;

// Pas d'éditeur standard DevExpress — on dessine nous-mêmes
Delete.ColumnEdit = null;

// Empêche l'utilisateur de modifier la cellule directement (double-clic, F2…)
Delete.OptionsColumn.AllowEdit = false;
```

---

## Étape 4 — Dessiner la croix rouge avec `CustomDrawCell`

DevExpress expose l'événement `CustomDrawCell` qui se déclenche **pour chaque cellule** au moment
du rendu. On l'utilise pour dessiner une croix rouge à la place du contenu par défaut.

```csharp
// s = gridView2 (l'objet qui déclenche l'événement, non utilisé ici)
// e = CustomRowCellDrawEventArgs — donne accès à :
//     e.Column     → quelle colonne est en train d'être dessinée
//     e.RowHandle  → index de la ligne (négatif = ligne spéciale, ex: header)
//     e.Bounds     → rectangle pixel de la cellule
//     e.Graphics   → surface de dessin GDI+
//     e.Handled    → si true, DevExpress n'applique plus son rendu par-dessus
gridView2.CustomDrawCell += (s, e) => DrawDeleteCell(e);
```

La méthode de dessin elle-même :

```csharp
private void DrawDeleteCell(DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
{
    // On ne dessine que dans la colonne Delete et uniquement sur de vraies lignes de données
    if (e.Column != Delete || e.RowHandle < 0) return;

    // Laisse DevExpress dessiner le fond de la cellule (couleur de sélection, etc.)
    e.DefaultDraw();

    // Calcule le centre de la cellule
    int cx = e.Bounds.Left + e.Bounds.Width / 2;
    int cy = e.Bounds.Top + e.Bounds.Height / 2;

    // Dessine les deux diagonales de la croix avec GDI+
    using var pen = new Pen(Color.Red, 2);
    e.Graphics.DrawLine(pen, cx - 5, cy - 5, cx + 5, cy + 5); // \ 
    e.Graphics.DrawLine(pen, cx + 5, cy - 5, cx - 5, cy + 5); // /

    // Indique à DevExpress de ne pas redessiner par-dessus notre croix
    e.Handled = true;
}
```

> La logique de centrage : `e.Bounds` donne le rectangle de la cellule en pixels à l'écran.
> On prend le milieu en X et en Y, puis on trace deux lignes de ±5 pixels autour de ce centre.

---

## Étape 5 — Gérer le clic avec `RowCellClick`

`CustomDrawCell` ne gère que l'affichage. Pour la suppression, on branche `RowCellClick` :

```csharp
// s = gridView2 (non utilisé)
// e = RowCellClickEventArgs — donne accès à :
//     e.Column    → colonne sur laquelle le clic s'est produit
//     e.RowHandle → index de la ligne cliquée
gridView2.RowCellClick += (s, e) =>
{
    // Ignorer les clics sur toutes les autres colonnes
    if (e.Column != Delete) return;

    // Demander confirmation avant de supprimer
    if (MessageBox.Show("Supprimer cette information ?", "Confirmation",
        MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

    // 1. Supprimer la ligne du DataSet en mémoire
    gridView2.DeleteRow(e.RowHandle);

    // 2. Propager la suppression vers SQL Server via le TableAdapter
    _taI.Update(_ds.infoContact);
};
```

---

## Résumé du flux complet

```
FormSociete_Load()
    │
    ├── PopulateColumns()          → régénère les colonnes depuis le DataSource
    ├── Columns.Add(Delete)        → remet la colonne Delete à la fin
    ├── Configuration de Delete    → largeur, caption, pas d'éditeur, non éditable
    │
    ├── CustomDrawCell ──────────► DrawDeleteCell(e)
    │                                  → dessine la croix rouge pixel par pixel (GDI+)
    │
    └── RowCellClick ────────────► vérifie e.Column == Delete
                                       → confirmation MessageBox
                                       → gridView2.DeleteRow(e.RowHandle)   [mémoire]
                                       → _taI.Update(_ds.infoContact)       [SQL Server]
```

---

## Points clés à retenir pour l'entretien

| Question possible | Réponse courte |
|---|---|
| Pourquoi `PopulateColumns()` avant d'ajouter Delete ? | `PopulateColumns()` supprime toutes les colonnes manuelles — on doit la remettre après. |
| Pourquoi `CustomDrawCell` et pas un `RepositoryItemButtonEdit` ? | On voulait une vraie croix dessinée, pas un bouton — plus fidèle au cahier des charges. |
| Que fait `e.Handled = true` ? | Empêche DevExpress de redessiner par-dessus notre rendu GDI+ personnalisé. |
| Pourquoi `_taI.Update()` après `DeleteRow()` ? | `DeleteRow()` ne touche qu'au DataSet en mémoire. `Update()` envoie le DELETE SQL réel. |
| Pourquoi deux événements séparés ? | Séparation des responsabilités : `CustomDrawCell` = affichage, `RowCellClick` = logique métier. |
