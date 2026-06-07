using AnnuaireEntreprise.Forms;
using DevExpress.XtraGrid.Localization;

namespace AnnuaireEntreprise
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            //instancier la classe de localisation personnalisée et pour charger les bon textes en francais
            DevExpress.XtraGrid.Localization.GridLocalizer.Active = new AnnuaireEntreprise.Forms.FormListe.MonGridLocalizer();
            Application.Run(new FormListe());
        }
    }
}