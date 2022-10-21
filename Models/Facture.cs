using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyLittleBusiness.Models
{
    public class Facture
    {
        [Key]
        public int FactureId { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane!")]
        [DataType(DataType.Date)]
        [DisplayName("Data wydania")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [ForeignKey("Client")]
        [DisplayName("Nazwa klienta")]
        public int ClientId { get; set; }

        public virtual ICollection<FactureHasItem> FactureHasItems { get; set; }

        [DisplayName("Nazwa klienta")]
        public virtual Client Client { get; set; }
    }
}
