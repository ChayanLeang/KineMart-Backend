using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KineMartAPI.ModelEntities
{
    [Table(name:"categories")]
    public class Category
    {
        [Key]
        [Column(name:"category_id")]
        public int CategoryId { get; set; }

        [StringLength(255)]
        [Column(name:"category_name")]
        public string CategoryName { get; set; } = null!;

        [Column(name: "isActive")]
        public bool IsActive { get; set; }
    }
}
