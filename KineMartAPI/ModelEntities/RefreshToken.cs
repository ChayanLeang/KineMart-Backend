using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace KineMartAPI.ModelEntities
{
    [Table(name:"refresh_tokens")]
    public class RefreshToken
    {
        [Key]
        [Column(name: "re_token_id")]
        public int RefreshId { get; set; }

        [StringLength(450)]
        [Column(name: "user_id")]
        public string UserId { get; set; } = null!;

        [StringLength(255)]
        [Column(name: "token", TypeName = "varchar")]
        public string Token { get; set; } = null!;

        [StringLength(255)]
        [Column(name: "jwt_id", TypeName = "varchar")]
        public string JwtId { get; set; } = null!;

        [Column(name: "isRevoked")]
        public bool IsRevoked { get; set; }

        [Column(name: "date_add")]
        public DateTime DateAdd { get; set; }

        [Column(name: "date_expire")]
        public DateTime DateExpire { get; set; }

        [ForeignKey(nameof(UserId))]
        public ApplicationUser ApplicationUser { get; set; } = null!;
    }
}
