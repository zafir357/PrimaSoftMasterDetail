using AnnuaireEntreprise.Tests.Helpers;
using Xunit;

namespace AnnuaireEntreprise.Tests
{
    public class ValidationTests
    {
        // ── Nom de société ───────────────────────────────────────────────────
        // [Fact] = cas unique sans paramètre, suffisant quand il n'y a pas de variantes

        [Fact]
        public void NomSociete_Valide()
        {
            Assert.True(ValidationHelper.NomSocieteValide("Acme Corp"));
        }

        // [Theory] + [InlineData] = même test rejoué avec chaque valeur du tableau
        // Évite de dupliquer un [Fact] par cas invalide

        [Theory]
        [InlineData("")]          // chaîne vide
        [InlineData("   ")]       // espaces seuls
        [InlineData(null)]        // null
        public void NomSociete_Invalide(string? nom)
        {
            Assert.False(ValidationHelper.NomSocieteValide(nom));
        }


        // ── Standard téléphonique ────────────────────────────────────────────

        // [Theory] pour tous les cas valides : le champ est facultatif,
        // donc vide et null sont acceptés, et 7 chiffres est le seul format valide
        [Theory]
        [InlineData("0123456")]   // 7 chiffres exacts
        [InlineData("")]          // facultatif — vide accepté
        [InlineData(null)]        // facultatif — null accepté
        public void Standard_Valide(string? standard)
        {
            Assert.True(ValidationHelper.StandardValide(standard));
        }

        // [Theory] pour tous les cas invalides regroupés par type d'erreur
        [Theory]
        [InlineData("012345")]    // trop court (6)
        [InlineData("01234567")]  // trop long (8)
        [InlineData("012345a")]   // lettre en fin
        [InlineData("a123456")]   // lettre en début
        [InlineData("012 456")]   // espace au milieu
        [InlineData("01-23-45")]  // tirets
        public void Standard_Invalide(string standard)
        {
            Assert.False(ValidationHelper.StandardValide(standard));
        }


        // ── Code postal ──────────────────────────────────────────────────────

        [Theory]
        [InlineData("75001")]     // Paris 1er — cas nominal
        [InlineData("13000")]     // Marseille
        [InlineData("")]          // facultatif — vide accepté
        [InlineData(null)]        // facultatif — null accepté
        public void CodePostal_Valide(string? codePostal)
        {
            Assert.True(ValidationHelper.CodePostalValide(codePostal));
        }

        [Theory]
        [InlineData("7500")]      // trop court (4)
        [InlineData("750011")]    // trop long (6)
        [InlineData("7500A")]     // lettre
        [InlineData("750 1")]     // espace
        [InlineData("7500!")]     // caractère spécial
        public void CodePostal_Invalide(string codePostal)
        {
            Assert.False(ValidationHelper.CodePostalValide(codePostal));
        }
    }
}
