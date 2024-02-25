using Serilog.Events;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace KineMartAPI.ModelEntities
{
    [Table(name:"Logs")]
    public class Log
    {
        [Key]
        public int Id { get; set; }
        public string? Message { get; set; }
        public string? MessageTemplate { get; set; } = null!;
        public string? Level { get; set; } = null!;

        [Column(TypeName ="datetime")]
        public DateTime? TimeStamp { get; set; }
        public string? Exception { get; set; } = null!;
        public string? Properties { get; set; } = null!;
    }
}
