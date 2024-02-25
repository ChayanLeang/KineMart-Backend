using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KineMartAPI.ModelEntities
{
    [Table(name:"suppliers")]
    public class Supplier
    {
        [Key]
        [Column(name:"supplier_id")]
        public int SupplierId { get; set; }

        [StringLength(255)]
        [Column(name:"owner")]
        public string Owner { get; set; } = null!;

        [StringLength(255)]
        [Column(name: "company_name")]
        public string CompanyName { get; set; } = null!;

        [StringLength(255)]
        [Column(name: "phone_number")]
        public string PhoneNumber { get; set; } = null!;

        [StringLength(255)]
        [Column(name: "address")]
        public string Address { get; set; } = null!;

        [Column(name: "isActive")]
        public bool IsActive { get; set; }
    }
}
