using AnnuaireEntreprise.Models;
using AnnuaireEntreprise.Dataset.AnnuaireDataSetTableAdapters;
using AnnuaireEntreprise.Dataset;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AnnuaireEntreprise.Forms
{
    public partial class FormContact : DevExpress.XtraEditors.XtraForm
    {
        // On reçoit le DataSet et les TableAdapter depuis FormListe
        // pour travailler sur les mêmes données en mémoire
        private AnnuaireDataSet _ds;
        private societeTableAdapter _taS;
        private contactTableAdapter _taC;

        // ── Constructeur ─────────────────────────────────────────────────────
        // Reçoit le DataSet et les TableAdapter depuis FormListe
        public FormContact(AnnuaireDataSet ds, societeTableAdapter taS, contactTableAdapter taC)
        {
            InitializeComponent();
            _ds = ds;
            _taS = taS;
            _taC = taC;
        }

        // ── Chargement du formulaire ─────────────────────────────────────────
        private void FormContact_Load(object sender, EventArgs e)
        {
            // Remplir le ComboBox avec toutes les sociétés existantes
            cboSociete.Properties.Items.Clear();
            foreach (AnnuaireDataSet.societeRow row in _ds.societe.Rows)
                cboSociete.Properties.Items.Add(row.nom);
        }

        private void labelControl1_Click(object sender, EventArgs e)
        {

        }

        // ── Bouton "+" : créer une société à la volée ────────────────────────
        // L'utilisateur peut créer une nouvelle société sans fermer ce formulaire
        // ── Bouton "+" : créer une société à la volée ────────────────────────
        // L'utilisateur peut créer une nouvelle société sans fermer ce formulaire
        private void cboSociete_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            //Je verifie si c'est bien le bouton "+" qui a été cliqué, car il y a maintenant le dropdown pour les sociétés
            if (e.Button.Kind != DevExpress.XtraEditors.Controls.ButtonPredefines.Plus)
                return;
            // Ouvre FormSociete en mode création(societeId = 0)
            using var frm = new FormSociete(0, _ds, _taS, _taC);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                // Recharger les sociétés dans le DataSet
                _taS.Fill(_ds.societe);
                // Recharger le ComboBox avec les nouvelles sociétés
                cboSociete.Properties.Items.Clear();
                foreach (AnnuaireDataSet.societeRow row in _ds.societe.Rows)
                    cboSociete.Properties.Items.Add(row.nom);
                // Sélectionner automatiquement la dernière société créée
                cboSociete.SelectedIndex = cboSociete.Properties.Items.Count - 1;
            }
        }

        // ── Bouton Valider ───────────────────────────────────────────────────
        private void btnValider_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cboSociete.Text))
            {
                MessageBox.Show("Sélectionnez ou créez une société.",
                    "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Retrouver l'id de la société sélectionnée par son nom
            string nomChoisi = cboSociete.Text;
            var soc = _ds.societe.AsEnumerable()
                .FirstOrDefault(r => r["nom"].ToString() == nomChoisi);
            //Meme si je sais que si la societé est dans le combo, elle doit être dans le dataset,
            //je préfère ajouter une vérification juste au cas où

            if (soc == null)
            {
                MessageBox.Show("Société introuvable.", "Erreur",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // Vérifier que l'ID existe et est valide
            int idSoc;
            try
            {
                idSoc = (int)soc["id"];
                if (idSoc <= 0)
                {
                    MessageBox.Show("ID de société invalide.",
                        "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (InvalidCastException)
            {
                MessageBox.Show("Erreur : l'ID de la société n'est pas un nombre entier.",
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

             //Vérifier que FormSociete s'ouvre correctement
            try
            {
                using var frm = new FormSociete(idSoc, _ds, _taS, _taC);
                if (frm == null)
                {
                    MessageBox.Show("Impossible d'ouvrir le formulaire de société.",
                        "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ouverture du formulaire : {ex.Message}",
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Ferme FormContact — FormListe rechargera la liste
            DialogResult = DialogResult.OK;
            Close();
        }

        // ── Bouton Annuler ───────────────────────────────────────────────────
        private void btnAnnuler_Click(object sender, EventArgs e)
        {
            // Ferme sans sauvegarder
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void cboSociete_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cboSociete_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }
    }
}