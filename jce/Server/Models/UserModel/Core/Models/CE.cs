using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace jce.Server.UserModel.Core.Models
{
    [Table("CEs")]
    public class CE
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Logo { get; set; }
        public string NomResp { get; set; }
        public string PrenomResp { get; set; }
        public string EmailResp { get; set; }
        public string EmailCopie { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Adresse1 { get; set; }
        public string Adresse2 { get; set; }
        public string CodePostal { get; set; }
        public string Ville { get; set; }
        public bool? IsMiniCat { get; set; }
        public int? IdAdmin { get; set; }
        public bool? Actif { get; set; }
        public bool? IsLimitationChoixparAge { get; set; }
        public bool? IsFichesCollectivites { get; set; }
        public DateTime? DateLimite { get; set; }
        public bool? IsGestionLettres { get; set; }
        public string TxtObjetMailIdent { get; set; }
        public string TxtObjetMailConfirm { get; set; }
        public bool AfficherPrix { get; set; }
        public string Telephone { get; set; }
        public string Fax { get; set; }
        public bool? AffichageNplus1 { get; set; }
        public bool? AffichageNmoins1 { get; set; }
        public bool IsGestionLivraisons { get; set; }
        public bool IsGestionSites { get; set; }
        public string TxtMailIdent { get; set; }
        public string TxtMailConfirm { get; set; }
        public string TxtMailRelance { get; set; }
        public string TxtObjetMailRelance { get; set; }
        public bool AfficherLogoPintel { get; set; }
        public bool AfficherLogoCe { get; set; }
        public bool AffichageN { get; set; }
        public bool? IsParticipationSalarie { get; set; }
        public bool? IsConfirmationCommande { get; set; }
        public int? IdTaille { get; set; }
        public bool? IsConfirmationCommandePourRespCe { get; set; }
        public bool? IsParticipationCe { get; set; }
        public int? NbChoix { get; set; }
        public string InfoChoix { get; set; }
        public bool? IsPlusieursProduitsPossibles { get; set; }
        public int? NbProduitsCumules { get; set; }
        public int? SelectMode { get; set; }
        public string TypeProduitObligatoire { get; set; }
        public DateTime? AncienneDateLimite { get; set; }
        public string TxtObjectMailRetardataires { get; set; }
        public string TxtMailRetardataires { get; set; }
        public string MsgAccueil { get; set; }
        public bool? IsDelete { get; set; }
        public string TxtObjetMailIdentRc { get; set; }
        public string TxtMailIdentRc { get; set; }
        public string TypageProduit { get; set; }
        public bool? IsRegroupementPossible { get; set; }
        public bool? IsAcceptLivraisonAdomicile { get; set; }
        public bool? IsRefClient { get; set; } 
        public virtual ICollection<User> Employee { get; set; }

        public CE()
        {
            Employee = new  Collection<User>();
        }

    }
}