using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KineMartAPI.ModelEntities
{
    [Table(name:"exports")]
    public class Export
    {
        [Key]
        [Column(name:"export_id")]
        public int ExportId { get; set; }

        [StringLength(450)]
        [Column(name: "user_id")]
        public string UserId { get; set; } = null!;

        [StringLength(255)]
        [Column(name: "date")]
        public string Date { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public ApplicationUser ApplicationUser { get; set; } = null!;
    }
}
