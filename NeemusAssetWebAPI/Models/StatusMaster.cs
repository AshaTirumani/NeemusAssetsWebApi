using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeemusAssetWebAPI.Models
{
    [Table("StatusMaster")]
    public class StatusMaster
    {
        [Key]
        public int StatusID { get; set; }

        public string? StatusName { get; set; }

        public string? StatusCode { get; set; }

        public string? Status { get; set; }
    }
}