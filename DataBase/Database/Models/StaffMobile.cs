using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Contexts.Models
{
    public class StaffMobile : BaseEntity
    {
        public StaffMobile()
        {

        }

        public StaffMobile(string realIP)
        {
            this.realIP = realIP;
        }
        
        [Required]
        public string realIP { get; set; } = "";

        [Required]
        public string Name { get; set; } = "";
        [Required]
        public string Surname { get; set; } = "";
        public string Patronymic { get; set; } = "";

        [Required]
        public string Number { get; set; } = "";
        public string Password { get; set; } = "";

        /*[DataType(DataType.Date)]
        [Column("DATE_HIRED")]
        public DateTime DateHired { get; set; }

        [DataType(DataType.Date)]
        [Column("DATE_FIRED")]
        public DateTime? DateFired { get; set; }

        [DataType(DataType.Date)]
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }*/

        // [Column("REFRESH_TOKEN")]
        // public string? RefreshToken { get; set; }

        // [Column("TOKEN_VALID_TO")]
        // public DateTime TokenValidTo { get; set; }

        public virtual List<StaffMobileToken> StaffMobileTokens { get; set; } = new List<StaffMobileToken>();
        public virtual List<GPSPosition> GPSPositions { get; set; } = new List<GPSPosition>();

    }
}
