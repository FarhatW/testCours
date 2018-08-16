using System;

namespace jce.Server.UserModel.Controllers.Resources
{
    public class ChildResource
    {
           public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public int IdPersonne { get; set; }
        public DateTime? DateNaissance { get; set; }
        public byte? Age { get; set; }
        public string Sexe { get; set; }
        public bool? IsActif { get; set; }
        public decimal MontantParticipationCe { get; set; }
        public bool? IsRegrouper { get; set; }
    }
}