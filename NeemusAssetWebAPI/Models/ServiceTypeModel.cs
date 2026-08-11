using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeemusAssetWebAPI.Models
{
    [Table("ServiceTypeMaster")]
    public class ServiceTypeModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ServiceTypeID { get; set; }

        public string? ServiceTypeName { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
