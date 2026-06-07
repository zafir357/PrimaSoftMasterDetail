using AnnuaireEntreprise.Dataset.AnnuaireDataSetTableAdapters;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using AnnuaireEntreprise.Models;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AnnuaireEntreprise.Dataset;
namespace AnnuaireEntreprise.Forms
{
    public partial class FormListe : DevExpress.XtraEditors.XtraForm
    {
        // ── DataSet = représentation en mémoire des 3 tables SQL ─────────────
        // Fill() charge les données SQL dans le DataSet
        // Update() envoie les modifications du DataSet vers SQL
        private AnnuaireDataSet _ds;
        // ── TableAdapter = pont entre le DataSet et SQL Server ───────────────
        // Génère automatiquement SELECT, INSERT, UPDATE, DELETE
        private societeTableAdapter _taS;
        private contactTableAdapter _taC;
        private infoContactTableAdapter _taI;

        public FormListe()
        {
            InitializeComponent();

            // Initialisation des TableAdapter dans le constructeur
            _ds = new AnnuaireDataSet();
            _taS = new societeTableAdapter();
            _taC = new contactTableAdapter();
            _taI = new infoContactTableAdapter();
        }

        private void FormListe_Load(object sender, EventArgs e)
        {
            LoadData();

            // Lier la grille à la table infoContact du DataSet
            gridControl1.DataSource = _ds.infoContact;

            // Barre de recherche intégrée DevExpress toujours visible
            gridView1.OptionsFind.AlwaysVisible = true;

            // Recherche sur toutes les colonnes
            gridView1.OptionsFind.FindFilterColumns = "*";

            // Affiche le bouton Find
            gridView1.OptionsFind.ShowFindButton = true;

            // Affiche le bouton Clear (X) dans la barre de recherche
            // le x s'affiche uniquement lorsque du texte est présent dans la barre de recherche, il permet de vider le champ de recherche d'un clic
            gridView1.OptionsFind.ShowClearButton = true;

            // Affiche le panneau de regroupement avec les boutons Société et Contact
            gridView1.OptionsView.ShowGroupPanel = true;

            // Grouper par société puis par contact
            gridView1.Columns["NomSociete"].Group();
            gridView1.Columns["NomContact"].Group();

            // Cacher les colonnes techniques
            gridView1.Columns["id"].Visible = false;
            gridView1.Columns["idContact"].Visible = false;

            // Renommer les colonnes pour correspondre au cahier des charges
            gridView1.Columns["typeInfo"].Caption = "Type";
            gridView1.Columns["info"].Caption = "Info";
            // Désactiver l'édition directe dans la grille — lecture seule
            gridView1.OptionsBehavior.Editable = false;

        }
        public class MonGridLocalizer : DevExpress.XtraGrid.Localization.GridLocalizer
        {

            //Change les textes par défaut en francais  
            public override string GetLocalizedString(DevExpress.XtraGrid.Localization.GridStringId id)
            {
                if (id == DevExpress.XtraGrid.Localization.GridStringId.FindControlFindButton) return "Trouver";
                if (id == DevExpress.XtraGrid.Localization.GridStringId.FindControlClearButton) return "Effacer";
                if (id == DevExpress.XtraGrid.Localization.GridStringId.FindNullPrompt) return "Entrez le texte à rechercher...";
                return base.GetLocalizedString(id);
            }
        }
        private void LoadData()
        {
            try
            {
                // Vide le DataSet avant de recharger pour éviter les doublons
                _ds.Clear();

                // Remplit le DataSet depuis SQL Server via les TableAdapter
                // Chaque Fill() envoie un SELECT et charge les lignes dans la DataTable correspondante
                _taS.Fill(_ds.societe);
                _taC.Fill(_ds.contact);
                _taI.Fill(_ds.infoContact);


                // Ajoute les colonnes calculées "NomSociete" et "NomContact" à infoContact
                if (!_ds.infoContact.Columns.Contains("NomSociete"))
                    _ds.infoContact.Columns.Add("NomSociete", typeof(string));

                if (!_ds.infoContact.Columns.Contains("NomContact"))
                    _ds.infoContact.Columns.Add("NomContact", typeof(string));

                foreach (AnnuaireDataSet.infoContactRow row in _ds.infoContact.Rows)
                {
                    var contact = _ds.contact.FindByid(row.idContact);
                    if (contact != null)
                    {
                        // Construit NomContact depuis les champs du contact trouvé
                        row["NomContact"] = contact.prenom + " " + contact.nom;
                        // "Requête 2" — cherche la société liée dans _ds.societe (mémoire)
                        var soc = _ds.societe.FindByid(contact.idSociete);
                        // Remplit NomSociete
                        row["NomSociete"] = soc != null ? soc.nom : "";
                    }
                }
            }
            catch (Exception ex)
            {
                // Affiche l'erreur SQL sans crasher l'application, on affiche l'erreur dans le messageBox
                MessageBox.Show("Erreur : " + ex.Message,
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNouveauContact_Click(object sender, EventArgs e)
        {
            // Ouvre FormContact en popup — l'utilisateur choisit ou crée une société
            // FormContact ouvre ensuite FormSociete pour saisir le contact dans la grille
            using var frm = new FormContact(_ds, _taS, _taC);
            // Si l'utilisateur a validé (pas annulé), recharger la liste
            // pour afficher le nouveau contact ajouté
            if (frm.ShowDialog() == DialogResult.OK)
                LoadData();
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            int rowHandle = gridView1.FocusedRowHandle;
            if (rowHandle < 0) return;

            // GetRow peut retourner un DataRowView au lieu de infoContactRow
            var rowObj = gridView1.GetRow(rowHandle);
            if (rowObj == null) return;

            // Récupère la DataRow sous-jacente
            var dataRow = (rowObj as System.Data.DataRowView)?.Row
                          ?? rowObj as System.Data.DataRow;
            if (dataRow == null) return;

            int idContact = (int)dataRow["idContact"];
            var contact = _ds.contact.FindByid(idContact);
            if (contact == null) return;

            using var frm = new FormSociete(contact.idSociete, _ds, _taS, _taC);
            frm.ShowDialog();
            // Recharger dans tous les cas — OK ou Annuler pour eviter que la liste soit vide si
            //par exemple je fait annuler
            LoadData();
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }


    }
}
