using DevExpress.XtraEditors;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using AnnuaireEntreprise.Dataset;
using AnnuaireEntreprise.Dataset.AnnuaireDataSetTableAdapters;

namespace AnnuaireEntreprise.Forms
{
    public partial class FormSociete : DevExpress.XtraEditors.XtraForm
    {
        // ── Données partagées depuis FormContact ou FormListe ────────────────
        private AnnuaireDataSet _ds;
        private societeTableAdapter _taS;
        private contactTableAdapter _taC;
        private infoContactTableAdapter _taI = new infoContactTableAdapter();

        // 0 = mode création, >0 = mode modification
        private int _societeId;

        public FormSociete(int societeId, AnnuaireDataSet ds,
            societeTableAdapter taS, contactTableAdapter taC)
        {
            InitializeComponent();
            _societeId = societeId;
            _ds = ds;
            _taS = taS;
            _taC = taC;
        }

        // Colonne delete pour la grille contacts (créée programmatiquement, contrairement à Delete qui est dans le designer pour infoContact)
        private readonly DevExpress.XtraGrid.Columns.GridColumn _deleteContact = new DevExpress.XtraGrid.Columns.GridColumn();

        private void FormSociete_Load(object sender, EventArgs e)
        {
            // Charger les infoContact dans le DataSet
            _taI.Fill(_ds.infoContact);

            // Titre selon le mode
            // Si on est en mode création (_societeId == 0), le titre est "Nouvelle société".
            // Sinon, on affiche "Modification : " suivi du nom de la société récupéré depuis le DataSet.
            this.Text = _societeId == 0
                ? "Nouvelle société"
                : "Modification : " + _ds.societe.FindByid(_societeId).nom;
            //Si on est en mode modification, on pré-remplit les champs avec les données de la société existante.
            if (_societeId > 0)
            {
                // ── Mode modification : pré-remplir les champs ───────────────
                var soc = _ds.societe.FindByid(_societeId);
                if (soc != null)
                {
                    //Si on double click et que c'est null, on affiche une chaîne vide pour éviter les "NullReferenceException"
                    textEdit1.Text = soc.IsnomNull() ? "" : soc.nom;
                    textEdit5.Text = soc.IsstandardNull() ? "" : soc.standard;
                    textEdit2.Text = soc.IsadresseNull() ? "" : soc.adresse;
                    textEdit4.Text = soc.Isadresse2Null() ? "" : soc.adresse2;
                    textEdit6.Text = soc.IscodePostalNull() ? "" : soc.codePostal;
                    textEdit3.Text = soc.IsvilleNull() ? "" : soc.ville;
                }
            }

            // En mode création, aucune societe n'existe encore dans le DataSet donc la FK
            // contact.idSociete → societe.id rejetterait toute nouvelle ligne de contact.
            // On suspend les contraintes le temps de la saisie ; le vrai contrôle FK
            // se fera côté SQL lors du commit.
            if (_societeId == 0)
                _ds.EnforceConstraints = false;

            // ── Binding MASTER : contacts de cette société ───────────────────
            var bsContacts = new BindingSource(_ds, "contact");
            bsContacts.Filter = "idSociete = " + (_societeId > 0 ? _societeId : -1);
            gridContacts.DataSource = bsContacts;

            gridView1.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            // Afficher la ligne "+" d'ajout (sinon aucune nouvelle ligne possible).
            // C'est ce réglage — pas AllowAddRows — qui fait apparaître la ligne d'ajout,
            // exactement comme gridView2 (Infos Contact) qui l'a dans le Designer.
            gridView1.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
            // Pré-affecter idSociete sur chaque nouvelle ligne contact
            gridView1.InitNewRow += (s, re) =>
                gridView1.SetRowCellValue(re.RowHandle, "idSociete", _societeId > 0 ? _societeId : -1);

            // ── Colonne Delete pour la grille contacts ───────────────────────
            gridView1.Columns.Add(_deleteContact);
            _deleteContact.VisibleIndex = gridView1.VisibleColumns.Count;
            _deleteContact.Caption = "";
            _deleteContact.Width = 40;
            _deleteContact.ColumnEdit = null;
            _deleteContact.OptionsColumn.AllowEdit = false;
            _deleteContact.OptionsColumn.TabStop = false;

            gridView1.CustomDrawCell += (s, ce) =>
            {
                if (ce.Column != _deleteContact || ce.RowHandle < 0) return;
                ce.DefaultDraw();
                int cx = ce.Bounds.Left + ce.Bounds.Width / 2;
                int cy = ce.Bounds.Top + ce.Bounds.Height / 2;
                using var pen = new Pen(Color.Red, 2);
                ce.Graphics.DrawLine(pen, cx - 5, cy - 5, cx + 5, cy + 5);
                ce.Graphics.DrawLine(pen, cx + 5, cy - 5, cx - 5, cy + 5);
                ce.Handled = true;
            };

            gridView1.RowCellClick += (s, ce) =>
            {
                if (ce.Column != _deleteContact) return;
                if (MessageBox.Show("Supprimer ce contact ?", "Confirmation",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

                // Récupérer la ligne contact AVANT de la supprimer, pour atteindre ses
                // infoContact enfants. Les relations sont créées sans contrainte
                // (createConstraints:false) → aucun cascade : on doit donc supprimer
                // les enfants nous-mêmes, sinon SQL refuse le DELETE du contact (FK).
                if (gridView1.GetRow(ce.RowHandle) is System.Data.DataRowView drv &&
                    drv.Row is AnnuaireDataSet.contactRow contact)
                {
                    foreach (AnnuaireDataSet.infoContactRow info in contact.GetinfoContactRows())
                        info.Delete();
                    contact.Delete();
                }

                if (_societeId > 0)
                {
                    // Ordre INVERSE de l'insertion pour une suppression :
                    // les enfants (infoContact) d'abord, puis le parent (contact).
                    _taI.Update(_ds.infoContact);
                    _taC.Update(_ds.contact);
                }
            };

            // ── Binding DETAIL : infos du contact sélectionné ────────────────
            var bsInfos = new BindingSource(bsContacts, "FK__infoConta__idCon__4F7CD00D");
            gridInfoContact.DataSource = bsInfos;

            gridView2.OptionsBehavior.Editable = true;
            gridView2.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            // idContact est renseigné automatiquement par la relation FK (bsInfos est
            // dérivé de bsContacts). Pas besoin de InitNewRow : c'est pour ça que la
            // modification fonctionnait déjà. Il faut juste qu'un contact soit
            // sélectionné dans la grille de gauche pour servir de parent.

            // ── Colonne Delete pour la grille infos contact ──────────────────
            gridView2.Columns.Add(Delete);
            Delete.VisibleIndex = gridView2.VisibleColumns.Count;
            Delete.Caption = "";
            Delete.Width = 40;
            Delete.ColumnEdit = null;
            Delete.OptionsColumn.AllowEdit = false;
            Delete.OptionsColumn.TabStop = false;

            gridView2.CustomDrawCell += (s, ce) => DrawDeleteCell(ce);

            gridView2.RowCellClick += (s, ce) =>
            {
                if (ce.Column != Delete) return;
                if (MessageBox.Show("Supprimer cette information ?", "Confirmation",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                gridView2.DeleteRow(ce.RowHandle);
                if (_societeId > 0)
                    _taI.Update(_ds.infoContact);
            };

            // Cacher les colonnes techniques dans la grille contacts
            if (gridView1.Columns["id"] != null)
                gridView1.Columns["id"].Visible = false;
            if (gridView1.Columns["idSociete"] != null)
                gridView1.Columns["idSociete"].Visible = false;

            // Cacher les colonnes techniques dans la grille infos contact
            if (gridView2.Columns["id"] != null)
                gridView2.Columns["id"].Visible = false;
            if (gridView2.Columns["idContact"] != null)
                gridView2.Columns["idContact"].Visible = false;
            if (gridView2.Columns["NomSociete"] != null)
                gridView2.Columns["NomSociete"].Visible = false;
            if (gridView2.Columns["NomContact"] != null)
                gridView2.Columns["NomContact"].Visible = false;
        }

        /**
         * pour dessiner une croix rouge dans la colonne Delete, j'ai utilisé fait l'utilisation de l'IA
         * pour générer une méthode de dessin personnalisée. Copilot et chatgpt
         * je voulais que ca ressemble à ce qui avait dans le cahier de charges
         * 
             * 
         * **/
        private void DrawDeleteCell(DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            // Si la colonne est pas 'Delete', ou si c'est une ligne d'en-tête (RowHandle < 0),
            // on dessine pas delete
            if (e.Column != Delete || e.RowHandle < 0) return;
            e.DefaultDraw();
            int cx = e.Bounds.Left + e.Bounds.Width / 2;
            int cy = e.Bounds.Top + e.Bounds.Height / 2;
            using var pen = new Pen(Color.Red, 2);
            e.Graphics.DrawLine(pen, cx - 5, cy - 5, cx + 5, cy + 5);
            e.Graphics.DrawLine(pen, cx + 5, cy - 5, cx - 5, cy + 5);
            e.Handled = true;
        }

        // ── Validation avant sauvegarde ──────────────────────────────────────
        private bool ValiderChamps()
        {
            // Nom obligatoire
            if (string.IsNullOrWhiteSpace(textEdit1.Text))
            {
                MessageBox.Show("Le nom de la société est obligatoire.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textEdit1.Focus();
                return false;
            }

            // Standard téléphonique : 10 chiffres si renseigné (numéro local France sans indicatif)
            if (!string.IsNullOrWhiteSpace(textEdit5.Text))
            {
                string standard = textEdit5.Text.Trim();
                if (standard.Length != 10 || !standard.All(char.IsDigit))
                {
                    MessageBox.Show("Le standard téléphonique doit contenir exactement 7 chiffres.",
                        "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textEdit5.Focus();
                    return false;
                }
            }

            // Code postal : 5 chiffres si renseigné
            if (!string.IsNullOrWhiteSpace(textEdit6.Text))
            {
                string cp = textEdit6.Text.Trim();
                if (cp.Length != 5 || !cp.All(char.IsDigit))
                {
                    MessageBox.Show("Le code postal doit contenir exactement 5 chiffres.",
                        "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textEdit6.Focus();
                    return false;
                }
            }

            // Vérifier que les contacts ajoutés ont au moins un nom
            foreach (AnnuaireDataSet.contactRow row in _ds.contact.Rows)
            {
                if ((row.RowState == DataRowState.Added ||
                     row.RowState == DataRowState.Modified) &&
                     row.IsNull("nom"))
                {
                    MessageBox.Show("Un contact doit avoir au moins un nom.",
                        "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    gridContacts.Focus();
                    return false;
                }
            }

            return true;
        }

        private void btnValider_Click(object sender, EventArgs e)
        {
            if (!ValiderChamps()) return;

            var connection = _taS.Connection;
            connection.Open();
            try
            {

                //J'utilise pour garantir que toutes les modifications sont
                //enregistrées ensemble si jamais une erreur survient (ex: perte de connexion, erreur SQL, etc.)
                //le rollback annule toutes les modifications pour éviter d'avoir des données incohérentes dans la base.
                using var transaction = connection.BeginTransaction();

                try
                {
                    // Partager la connexion et la transaction sur les 3 TableAdapter
                    _taS.Connection = connection;
                    _taS.Transaction = transaction;
                    _taC.Connection = connection;
                    _taC.Transaction = transaction;
                    _taI.Connection = connection;
                    _taI.Transaction = transaction;
                    if (_societeId == 0)
                    {
                        // ── CRÉATION ─────────────────────────────────────────────
                        var r = _ds.societe.NewsocieteRow();
                        r.nom = textEdit1.Text.Trim();
                        r.standard = textEdit5.Text.Trim();
                        r.adresse = textEdit2.Text.Trim();
                        r.adresse2 = textEdit4.Text.Trim();
                        r.codePostal = textEdit6.Text.Trim();
                        r.ville = textEdit3.Text.Trim();
                        _ds.societe.AddsocieteRow(r);
                        _taS.Update(_ds.societe);

                        // Affecter le vrai id de la société aux contacts ajoutés via la grille (idSociete = -1)
                        int newId = r.id;
                        foreach (AnnuaireDataSet.contactRow cr in _ds.contact.Rows)
                        {
                            if (cr.RowState != DataRowState.Deleted &&
                                cr.idSociete == -1)
                                cr.idSociete = newId;
                        }

                        MessageBox.Show("Société créée avec succès.",
                            "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        // ── MODIFICATION ──────────────────────────────────────────
                        var r = _ds.societe.FindByid(_societeId);
                        r.nom = textEdit1.Text.Trim();
                        r.standard = textEdit5.Text.Trim();
                        r.adresse = textEdit2.Text.Trim();
                        r.adresse2 = textEdit4.Text.Trim();
                        r.codePostal = textEdit6.Text.Trim();
                        r.ville = textEdit3.Text.Trim();
                        _taS.Update(_ds.societe);

                        MessageBox.Show("Société modifiée avec succès.",
                            "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    // Sauvegarder contacts et infos modifiés dans les grilles
                    /*
                     Note important: l'ordre des Update est crucial.
                    On a mis à jour en premier les sociétés pour s'assurer que
                    les idSociete des contacts soient exists avant de mettre à jour les contacts.
                     */

                    // Les relations du DataSet sont créées avec createConstraints:false
                    // (voir AnnuaireDataset.Designer.cs) → AUCUN cascade automatique.
                    // Quand _taC.Update remplace l'id temporaire (négatif) d'un nouveau
                    // contact par le vrai id SQL (identity), le idContact des infoContact
                    // enfants n'est PAS mis à jour → violation de FK côté SQL.
                    // On capture donc la correspondance id temporaire → vrai id ajouter par moi, nouvelle ligne.
                    var contactsAjoutes = new List<AnnuaireDataSet.contactRow>();
                    var idsTemporaires = new Dictionary<AnnuaireDataSet.contactRow, int>();
                    foreach (AnnuaireDataSet.contactRow cr in _ds.contact.Rows)
                    {
                        if (cr.RowState == DataRowState.Added)
                        {
                            contactsAjoutes.Add(cr);
                            idsTemporaires[cr] = cr.id; // id temporaire avant l'Update
                        }
                    }

                    _taC.Update(_ds.contact); // Ici que les id temporaires sont remplacés par les vrais id
                                              // générérés par SQL Server (identity) lors de l'insertion. 

                    // Après l'Update, cr.id contient le vrai id SQL. On construit la table
                    // de correspondance ancien id (temporaire) → nouvel id (réel).
                    var mapIds = new Dictionary<int, int>();
                    foreach (var cr in contactsAjoutes)
                        mapIds[idsTemporaires[cr]] = cr.id;

                    // On réaffecte le vrai idContact à chaque infoContact nouvellement ajouté
                    // qui pointait vers un id de contact temporaire.
                    foreach (AnnuaireDataSet.infoContactRow ir in _ds.infoContact.Rows)
                    {
                        if (ir.RowState == DataRowState.Added &&
                            mapIds.TryGetValue(ir.idContact, out int vraiId))
                            ir.idContact = vraiId;
                    }

                    // Si on avait fait _taI.Update() avant _taC.Update() → SQL Server retourne
                    // on aurait une erreur FK car l'infoContact référence un idContact qui n'existe pas encore
                    _taI.Update(_ds.infoContact);
                    //Si on fait la suppresion l'odre serait inverse.

                    transaction.Commit();
                    connection.Close();
                    _ds.EnforceConstraints = true;
                    DialogResult = DialogResult.OK;
                    Close();

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    connection.Close();
                    MessageBox.Show("Erreur lors de l'enregistrement : " + ex.Message,
                            "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Erreur de connexion : " + ex.Message,
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnAnnuler_Click(object sender, EventArgs e)
        {
            // Demander confirmation si des modifications non sauvegardées
            if (_ds.HasChanges())
            {
                var result = MessageBox.Show(
                    "Des modifications non sauvegardées seront perdues. Continuer ?",
                    "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes) return;
            }
            _ds.RejectChanges();
            _ds.EnforceConstraints = true;
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void gridInfoContact_Click(object sender, EventArgs e)
        {

        }
    }
}