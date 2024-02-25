using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace KineMartAPI.ModelEntities
{
    [Table(name:"product_properties")]
    public class ProductProperty
    {
        [Key]
        [Column(name:"product_property_id")]
        public int ProductProId { get; set; }

        [JsonIgnore]
        [Column(name:"product_id")]
        public int ProductId { get; set; }

        [Column(name: "cost")]
        public double Cost { get; set; }

        [Column(name: "price")]
        public double Price { get; set; }

        [Column(name: "qty")]
        public int Qty { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;
    }
}
