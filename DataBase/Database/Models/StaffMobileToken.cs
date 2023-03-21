using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Contexts.Models
{
    public class StaffMobileToken : BaseEntity
    {
        [Column("ISSUER ")]
        public string Issuer { get; set; }

        [Column("AUDIENCE ")]
        public string Audience { get; set; }

        [Column("TOKEN")]
        public string Token { get; set; } = "";

        [Column("TOKEN_NOT_BEFORE")]
        public DateTime TokenNotBefore{ get; set; }

        [Column("TOKEN_VALID_TO")]
        public DateTime TokenValidTo { get; set; }

        [Column("REFRESH_TOKEN")]
        public string RefreshToken { get; set; } = "";

        [Column("REFRESH_TOKEN_VALID_TO")]
        public DateTime RefreshTokenValidTo { get; set; }

        [Column("DATA_VALID_UPDATE")]
        public DateTime? DataValidUpdate { get; set; }

        [Column("VALID")]
        public bool Valid { get; set; }

        [Column("DATA_VALID_OFF")]
        public DateTime? DataValidOff { get; set; }

        public int? StaffMobileId { get; set; }
        public virtual StaffMobile StaffMobile { get; set; }
    }
}
