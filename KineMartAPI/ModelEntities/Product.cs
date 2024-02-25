using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace KineMartAPI.ModelEntities
{
    [Table(name:"products")]
    public class Product
    {
        [Key]
        [Column(name:"product_id")]
        public int ProductId { get; set; }

        [JsonIgnore]
        [Column(name: "category_id")]
        public int CategoryId { get; set; }

        [StringLength(255)]
        [Column(name:"product_name")]
        public string ProductName { get; set; } = null!;

        [Column(name: "isActive")]
        public bool IsActive { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;
    }
}
