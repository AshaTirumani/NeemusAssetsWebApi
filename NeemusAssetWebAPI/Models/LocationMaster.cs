using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        //public string? DepartmentCode { get; set; }
    }
}
