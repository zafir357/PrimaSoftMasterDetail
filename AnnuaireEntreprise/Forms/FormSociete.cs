using DevExpress.XtraEditors;
using System;
using System.Drawing;
using System.Windows.Forms;
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

        private void FormSociete_Load(object sender, EventArgs e)
        {
            // Charger les infoContact dans le DataSet
            _taI.Fill(_ds.infoContact);

            // Titre selon le mode
            this.Text = _societeId == 0
                ? "Nouvelle société"
                : "Modification : " + _ds.societe.FindByid(_societeId).nom;

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
            // ── Binding MASTER : contacts de cette société ───────────────────
            var bsContacts = new BindingSource(_ds, "contact");
            bsContacts.Filter = "idSociete = " + (_societeId > 0 ? _societeId : -1);
            gridContacts.DataSource = bsContacts;

            // ── Binding DETAIL : infos du contact sélectionné ────────────────
            // Quand on clique sur un contact dans gridContacts,
            // gridInfoContact se met à jour automatiquement via la relation FK
            var bsInfos = new BindingSource(bsContacts,
                "FK__infoConta__idCon__4F7CD00D");
            gridInfoContact.DataSource = bsInfos;
            // ajouter la colonne delete
            gridView2.Columns.Add(Delete);

            gridView2.OptionsBehavior.Editable = true;

            Delete.VisibleIndex = gridView2.VisibleColumns.Count;
            // Pas de titre — la croix rouge sert d'indicateur visuel
            Delete.Caption = "";
            // Largeur fixe juste assez large pour la croix
            Delete.Width = 40;
            // Pas d'éditeur — la cellule n'est pas interactive comme un bouton standard
            Delete.ColumnEdit = null;
            // Empêche l'utilisateur de modifier la valeur de la cellule directement
            Delete.OptionsColumn.AllowEdit = false;
            // Quand je vais faire entré ou tab pour ajouter une nouvelle ligne,
            // je ne veux pas que delete soit traité comme une colonne éditable
            Delete.OptionsColumn.TabStop = false;

            // CustomDrawCell : s = gridView2 (déclencheur), e = CustomRowCellDrawEventArgs
            // e donne accès à : e.Column (colonne dessinée), e.Bounds (rectangle de la cellule),
            // e.Graphics (surface de dessin), e.RowHandle (index de la ligne), e.Handled (court-circuite le rendu DevExpress)
            gridView2.CustomDrawCell += (s, e) => DrawDeleteCell(e);

            // RowCellClick : s = gridView2 (déclencheur), e = RowCellClickEventArgs
            // e donne accès à : e.Column (colonne cliquée), e.RowHandle (index de la ligne cliquée)
            gridView2.RowCellClick += (s, e) =>
            {
                // On n'agit que si le clic est sur la colonne Delete
                if (e.Column != Delete) return;
                if (MessageBox.Show("Supprimer cette information ?", "Confirmation",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                // Supprime la ligne du DataSet en mémoire puis synchronise avec SQL
                gridView2.DeleteRow(e.RowHandle);
                _taI.Update(_ds.infoContact);
            };
            // Après chaque mise à jour d'une ligne dans gridView2, on rafraîchit les données pour recalculer les colonnes calculées(delete par exemple)
            gridView2.RowUpdated += (s, e) => gridView2.RefreshData();

            // Cacher les colonnes techniques dans la grille contacts
            if (gridView2.Columns["id"] != null)
                gridView1.Columns["id"].Visible = false;
            gridView1.Columns["idSociete"].Visible = false;

            // Cacher les colonnes techniques dans la grille infos contact
            if (gridView2.Columns["id"] != null)
                gridView2.Columns["id"].Visible = false;

            if (gridView2.Columns["idContact"] != null)
                gridView2.Columns["idContact"].Visible = false;

            // Cacher les colonnes calculées qui ne doivent pas être éditables
            if (gridView2.Columns["NomSociete"] != null)
                gridView2.Columns["NomSociete"].Visible = false;


            if (gridView2.Columns["NomContact"] != null)
                gridView2.Columns["NomContact"].Visible = false;
            // Connecter l'événement du bouton Delete de la grille infos contact
            // car on ne peut pas le faire via le designer

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
                if ((row.RowState == System.Data.DataRowState.Added ||
                     row.RowState == System.Data.DataRowState.Modified) &&
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
                    _taC.Update(_ds.contact); // 2. Ensuite les contacts (enfants de société)
                    // Si on avait fait _taI.Update() avant _taC.Update() → SQL Server retourne
                    // on aurait une erreur FK car l'infoContact référence un idContact qui n'existe pas encore
                    _taI.Update(_ds.infoContact);
                    //Si on fait la suppresion l'odre serait inverse.

                    transaction.Commit();
                    connection.Close();// on ferme la connexion après le commit pour libérer les ressources
                    // si ou utilise le using var transaction, la connexion sera fermée automatiquement.
                    //Mais j'ai deja pas mal de lignes de code dans le try, je préfère fermer explicitement
                    DialogResult = DialogResult.OK;
                    Close();

                }
                catch (Exception ex)
                {
                    //rollback si erreur lors de l'enregistrement pour annuler toutes les modifications
                    transaction.Rollback();
                    connection.Close();// on ferme la connexion même en cas d'erreur pour libérer les ressources
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
            // Annule les modifications en mémoire
            //Par  ligne vides mais pas enregistré.
            _ds.RejectChanges(); 
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void gridInfoContact_Click(object sender, EventArgs e)
        {

        }
    }
}