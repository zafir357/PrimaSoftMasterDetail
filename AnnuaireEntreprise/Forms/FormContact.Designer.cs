namespace AnnuaireEntreprise.Forms
{
    partial class FormContact
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
            labelControl1 = new DevExpress.XtraEditors.LabelControl();
            cboSociete = new DevExpress.XtraEditors.ComboBoxEdit();
            btnValider = new DevExpress.XtraEditors.SimpleButton();
            btnAnnuler = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)cboSociete.Properties).BeginInit();
            SuspendLayout();
            // 
            // labelControl1
            // 
            labelControl1.Location = new Point(235, 97);
            labelControl1.Name = "labelControl1";
            labelControl1.Size = new Size(108, 16);
            labelControl1.TabIndex = 0;
            labelControl1.Text = "Choisir une société";
            labelControl1.Click += labelControl1_Click;
            // 
            // cboSociete
            // 
            cboSociete.Location = new Point(175, 119);
            cboSociete.Name = "cboSociete";
            cboSociete.Properties.Buttons.Clear();
            cboSociete.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[]
            {
              new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
              new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Plus)
            });
            cboSociete.Properties.ButtonClick += cboSociete_ButtonClick;
            cboSociete.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            cboSociete.Size = new Size(255, 22);
            cboSociete.TabIndex = 1;
            cboSociete.SelectedIndexChanged += cboSociete_SelectedIndexChanged_1;
            // 
            // btnValider
            // 
            btnValider.Location = new Point(175, 156);
            btnValider.Name = "btnValider";
            btnValider.Size = new Size(118, 36);
            btnValider.TabIndex = 2;
            btnValider.Text = "Valider";
            btnValider.Click += btnValider_Click;
            // 
            // btnAnnuler
            // 
            btnAnnuler.Location = new Point(312, 156);
            btnAnnuler.Name = "btnAnnuler";
            btnAnnuler.Size = new Size(118, 36);
            btnAnnuler.TabIndex = 3;
            btnAnnuler.Text = "Annuler";
            btnAnnuler.Click += btnAnnuler_Click;
            // 
            // FormContact
            // 
            AutoScaleDimensions = new SizeF(7F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(856, 411);
            Controls.Add(btnAnnuler);
            Controls.Add(btnValider);
            Controls.Add(cboSociete);
            Controls.Add(labelControl1);
            Name = "FormContact";
            Text = "FormContact";
            Load += FormContact_Load;
            ((System.ComponentModel.ISupportInitialize)cboSociete.Properties).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.ComboBoxEdit cboSociete;
        private DevExpress.XtraEditors.SimpleButton btnValider;
        private DevExpress.XtraEditors.SimpleButton btnAnnuler;
    }
}