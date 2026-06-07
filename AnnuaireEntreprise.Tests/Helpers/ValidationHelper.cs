using System.Linq;

namespace AnnuaireEntreprise.Tests.Helpers
{
    // Logique de validation extraite de FormSociete.ValiderChamps()
    // pour pouvoir être testée sans dépendre des contrôles WinForms
    public static class ValidationHelper
    {
        public static bool NomSocieteValide(string? nom) =>
            !string.IsNullOrWhiteSpace(nom);

        // Standard téléphonique : exactement 7 chiffres (numéro local France)
        public static bool StandardValide(string? standard)
        {
            if (string.IsNullOrWhiteSpace(standard)) return true; // champ facultatif
            string s = standard.Trim();
            return s.Length == 7 && s.All(char.IsDigit);
        }

        // Code postal : exactement 5 chiffres
        public static bool CodePostalValide(string? codePostal)
        {
            if (string.IsNullOrWhiteSpace(codePostal)) return true; // champ facultatif
            string cp = codePostal.Trim();
            return cp.Length == 5 && cp.All(char.IsDigit);
        }
    }
}
