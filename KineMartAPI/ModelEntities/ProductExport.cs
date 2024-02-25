using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace KineMartAPI.ModelEntities
{
    [Table(name:"product_exports")]
    public class ProductExport
    {
        [Key]
        [Column(name:"product_export_id")]
        public int ProductExId { get; set; }

        [Column(name: "product_id")]
        public int ProductId { get; set; }

        [Column(name: "export_id")]
        public int ExportId { get; set; }

        [Column(name: "price")]
        public double Price { get; set; }

        [Column(name: "qty")]
        public int Qty { get; set; }

        [Column(name: "amount")]
        public double Amount { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;

        [ForeignKey(nameof(ExportId))]
        public Export Export { get; set; } = null!;
    }
}
