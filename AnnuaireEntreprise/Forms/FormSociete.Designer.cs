namespace AnnuaireEntreprise.Forms
{
    partial class FormSociete
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            textEdit1 = new DevExpress.XtraEditors.TextEdit();
            textEdit2 = new DevExpress.XtraEditors.TextEdit();
            textEdit3 = new DevExpress.XtraEditors.TextEdit();
            textEdit4 = new DevExpress.XtraEditors.TextEdit();
            textEdit5 = new DevExpress.XtraEditors.TextEdit();
            textEdit6 = new DevExpress.XtraEditors.TextEdit();
            lblNom = new DevExpress.XtraEditors.LabelControl();
            lblAdresse = new DevExpress.XtraEditors.LabelControl();
            lblCodePostal = new DevExpress.XtraEditors.LabelControl();
            lblTel = new DevExpress.XtraEditors.LabelControl();
            addr2 = new DevExpress.XtraEditors.LabelControl();
            Ville = new DevExpress.XtraEditors.LabelControl();
            groupContacts = new GroupBox();
            gridContacts = new DevExpress.XtraGrid.GridControl();
            gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            groupInfosContact = new GroupBox();
            gridInfoContact = new DevExpress.XtraGrid.GridControl();
            gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            btnValider = new DevExpress.XtraEditors.SimpleButton();
            btnAnnuler = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)textEdit1.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)textEdit2.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)textEdit3.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)textEdit4.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)textEdit5.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)textEdit6.Properties).BeginInit();
            groupContacts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridContacts).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridView1).BeginInit();
            groupInfosContact.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridInfoContact).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridView2).BeginInit();
            SuspendLayout();
            // 
            // textEdit1
            // 
            textEdit1.EditValue = "";
            textEdit1.Location = new Point(145, 24);
            textEdit1.Name = "textEdit1";
            textEdit1.Size = new Size(145, 22);
            textEdit1.TabIndex = 0;
            // 
            // textEdit2
            // 
            textEdit2.Location = new Point(145, 52);
            textEdit2.Name = "textEdit2";
            textEdit2.Size = new Size(145, 22);
            textEdit2.TabIndex = 1;
            // 
            // textEdit3
            // 
            textEdit3.Location = new Point(671, 98);
            textEdit3.Name = "textEdit3";
            textEdit3.Size = new Size(138, 22);
            textEdit3.TabIndex = 2;
            // 
            // textEdit4
            // 
            textEdit4.Location = new Point(671, 69);
            textEdit4.Name = "textEdit4";
            textEdit4.Size = new Size(138, 22);
            textEdit4.TabIndex = 3;
            // 
            // textEdit5
            // 
            textEdit5.Location = new Point(671, 41);
            textEdit5.Name = "textEdit5";
            textEdit5.Size = new Size(138, 22);
            textEdit5.TabIndex = 4;
            // 
            // textEdit6
            // 
            textEdit6.Location = new Point(145, 81);
            textEdit6.Name = "textEdit6";
            textEdit6.Size = new Size(145, 22);
            textEdit6.TabIndex = 5;
            // 
            // lblNom
            // 
            lblNom.Location = new Point(43, 31);
            lblNom.Name = "lblNom";
            lblNom.Size = new Size(26, 16);
            lblNom.TabIndex = 6;
            lblNom.Text = "Nom";
            // 
            // lblAdresse
            // 
            lblAdresse.Location = new Point(43, 58);
            lblAdresse.Name = "lblAdresse";
            lblAdresse.Size = new Size(46, 16);
            lblAdresse.TabIndex = 7;
            lblAdresse.Text = "Adresse";
            // 
            // lblCodePostal
            // 
            lblCodePostal.Location = new Point(43, 87);
            lblCodePostal.Name = "lblCodePostal";
            lblCodePostal.Size = new Size(67, 16);
            lblCodePostal.TabIndex = 8;
            lblCodePostal.Text = "Code Postal";
            // 
            // lblTel
            // 
            lblTel.Location = new Point(532, 44);
            lblTel.Name = "lblTel";
            lblTel.Size = new Size(133, 16);
            lblTel.TabIndex = 9;
            lblTel.Text = "Standard Téléphonique";
            // 
            // addr2
            // 
            addr2.Location = new Point(612, 72);
            addr2.Name = "addr2";
            addr2.Size = new Size(53, 16);
            addr2.TabIndex = 10;
            addr2.Text = "Adresse2";
            // 
            // Ville
            // 
            Ville.Location = new Point(641, 101);
            Ville.Name = "Ville";
            Ville.Size = new Size(24, 16);
            Ville.TabIndex = 11;
            Ville.Text = "Ville";
            // 
            // groupContacts
            // 
            groupContacts.Controls.Add(gridContacts);
            groupContacts.Location = new Point(43, 153);
            groupContacts.Name = "groupContacts";
            groupContacts.Size = new Size(466, 314);
            groupContacts.TabIndex = 12;
            groupContacts.TabStop = false;
            groupContacts.Text = "Contacts";
            // 
            // gridContacts
            // 
            gridContacts.Location = new Point(6, 22);
            gridContacts.MainView = gridView1;
            gridContacts.Name = "gridContacts";
            gridContacts.Size = new Size(454, 250);
            gridContacts.TabIndex = 0;
            gridContacts.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { gridView1 });
            // 
            // gridView1
            // 
            gridView1.GridControl = gridContacts;
            gridView1.Name = "gridView1";
            // 
            // groupInfosContact
            // 
            groupInfosContact.Controls.Add(gridInfoContact);
            groupInfosContact.Location = new Point(541, 153);
            groupInfosContact.Name = "groupInfosContact";
            groupInfosContact.Size = new Size(462, 314);
            groupInfosContact.TabIndex = 13;
            groupInfosContact.TabStop = false;
            groupInfosContact.Text = "Infos Contact";
            // 
            // gridInfoContact
            // 
            gridInfoContact.Location = new Point(0, 22);
            gridInfoContact.MainView = gridView2;
            gridInfoContact.Name = "gridInfoContact";
            gridInfoContact.Size = new Size(456, 250);
            gridInfoContact.TabIndex = 0;
            gridInfoContact.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { gridView2 });
            gridInfoContact.Click += gridInfoContact_Click;
            // 
            // gridView2
            // 
            gridView2.GridControl = gridInfoContact;
            gridView2.Name = "gridView2";
            gridView2.OptionsView.NewItemRowPosition = DevExpress.XtraGrid.Views.Grid.NewItemRowPosition.Bottom;
            // 
            // btnValider
            // 
            btnValider.Location = new Point(761, 521);
            btnValider.Name = "btnValider";
            btnValider.Size = new Size(118, 36);
            btnValider.TabIndex = 14;
            btnValider.Text = "Valider";
            btnValider.Click += btnValider_Click;
            // 
            // btnAnnuler
            // 
            btnAnnuler.Location = new Point(885, 521);
            btnAnnuler.Name = "btnAnnuler";
            btnAnnuler.Size = new Size(118, 36);
            btnAnnuler.TabIndex = 15;
            btnAnnuler.Text = "Annuler";
            btnAnnuler.Click += btnAnnuler_Click;
            // 
            // FormSociete
            // 
            AutoScaleDimensions = new SizeF(7F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1054, 569);
            Controls.Add(btnAnnuler);
            Controls.Add(btnValider);
            Controls.Add(groupInfosContact);
            Controls.Add(groupContacts);
            Controls.Add(Ville);
            Controls.Add(addr2);
            Controls.Add(lblTel);
            Controls.Add(lblCodePostal);
            Controls.Add(lblAdresse);
            Controls.Add(lblNom);
            Controls.Add(textEdit6);
            Controls.Add(textEdit5);
            Controls.Add(textEdit4);
            Controls.Add(textEdit3);
            Controls.Add(textEdit2);
            Controls.Add(textEdit1);
            Name = "FormSociete";
            Text = "FormSociete";
            Load += FormSociete_Load;
            ((System.ComponentModel.ISupportInitialize)textEdit1.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)textEdit2.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)textEdit3.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)textEdit4.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)textEdit5.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)textEdit6.Properties).EndInit();
            groupContacts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridContacts).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridView1).EndInit();
            groupInfosContact.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridInfoContact).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridView2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }



        #endregion

        private DevExpress.XtraEditors.TextEdit textEdit1;
        private DevExpress.XtraEditors.TextEdit textEdit2;
        private DevExpress.XtraEditors.TextEdit textEdit3;
        private DevExpress.XtraEditors.TextEdit textEdit4;
        private DevExpress.XtraEditors.TextEdit textEdit5;
        private DevExpress.XtraEditors.TextEdit textEdit6;
        private DevExpress.XtraEditors.LabelControl lblNom;
        private DevExpress.XtraEditors.LabelControl lblAdresse;
        private DevExpress.XtraEditors.LabelControl lblCodePostal;
        private DevExpress.XtraEditors.LabelControl lblTel;
        private DevExpress.XtraEditors.LabelControl addr2;
        private DevExpress.XtraEditors.LabelControl Ville;
        private GroupBox groupContacts;
        private DevExpress.XtraGrid.GridControl gridContacts;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private GroupBox groupInfosContact;
        private DevExpress.XtraGrid.GridControl gridInfoContact;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraEditors.SimpleButton btnValider;
        private DevExpress.XtraEditors.SimpleButton btnAnnuler;
        private readonly DevExpress.XtraGrid.Columns.GridColumn Delete = new
        DevExpress.XtraGrid.Columns.GridColumn();

    }
}