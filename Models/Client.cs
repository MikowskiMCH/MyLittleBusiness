using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace MyLittleBusiness.Models
{
    public class Client
    {
        [Key]
        public int ClientId { get; set; }

        [Required(ErrorMessage = "Pole *Nazwa klienta* jest wymagane")]
        [DisplayName("Nazwa klienta")]
        [MaxLength(50, ErrorMessage = "Pole *Nazwa klienta* może zawierać maksymalnie 50 znaków")]
        public string ClientName { get; set; }

        [Required(ErrorMessage = "Pole *Adres* jest wymagane")]
        [DisplayName("Adres")]
        [MaxLength(100, ErrorMessage = "Pole *Adres* może zawierać maksymalnie 100 znaków")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Pole *Nr. NIP* jest wymagane")]
        [DisplayName("Nr. NIP")]
        [MaxLength(11, ErrorMessage = "Pole *Nr. NIP* musi zawierać 11 cyfr")]
        [MinLength(11, ErrorMessage = "Pole *Nr. NIP* musi zawierać 11 cyfr")]
        public string NIP { get; set; }

        public virtual ICollection<Facture> Factures { get; set; }
    }
}
