using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.WebPages;

namespace MyLittleBusiness.Models
{
    public class FactureHasItem
    {
        [Key]
        public int FactureHasItemId { get; set; }

        [Required]
        [DisplayName("Ilość")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Błędnie wpisane dane!(Spróbuj np.: 50.05)")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        [DisplayName("Wartość netto")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Błędnie wpisane dane!(Spróbuj np.: 50.05)")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PriceNetto { get; set; }

        [DisplayName("Wartość brutto")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Błędnie wpisane dane!(Spróbuj np.: 50.05)")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PriceGross { get; set; }

        [ForeignKey("Facture")]
        public int FactureId { get; set; }

        [ForeignKey("Item")]
        public int ItemId { get; set; }

        [DisplayName("Faktura")]
        public virtual Facture Facture { get; set; }

        [DisplayName("Towar")]
        public virtual Item Item { get; set; }

    }
}
