using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace KineMartAPI.ModelEntities
{
    [Table(name:"product_imports")]
    public class ProductImport
    {
        [Key]
        [Column(name:"product_import_id")]
        public int ProductImId { get; set; }

        [Column(name: "import_id")]
        public int ImportId { get; set; }

        [Column(name: "product_id")]
        public int ProductId { get; set; }

        [Column(name: "supplier_id")]
        public int SupplierId { get; set; }

        [Column(name: "cost")]
        public double Cost { get; set; }

        [Column(name: "price")]
        public double Price { get; set; }

        [Column(name: "qty")]
        public int Qty { get; set; }

        [Column(name: "remain")]
        public int Remain { get; set; }

        [Column(name: "amount")]
        public double Amount { get; set; }

        [StringLength(255)]
        [Column(name: "date")]
        public string Date { get; set; } = null!;

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;

        [ForeignKey(nameof(ImportId))]
        public Import Import { get; set; } = null!;

        [ForeignKey(nameof(SupplierId))]
        public Supplier Supplier { get; set; } = null!;
    }
}
