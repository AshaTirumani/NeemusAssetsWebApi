using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NeemusAssetWebAPI.Models
{
    [Table("LocationMaster")]
    public class LocationMaster
    {
        [Key]
        public int LocationID { get; set; }

        public string? Location { get; set; }

        public string? LocationCode { get; set; }

        public string? Status { get; set; }

        public string? Block { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
