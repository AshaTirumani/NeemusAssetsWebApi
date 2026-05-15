using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeemusAssetWebAPI.Models
{
    [Table("DepartmentMaster")]
    public class Department
    {
        [Key]
        public int? DepartmentID { get; set; }

        public string? DepartmentCode { get; set; }
        public string? DepartmentName { get; set; }

        public string? DepartmentStatus { get; set;}


    }
}
