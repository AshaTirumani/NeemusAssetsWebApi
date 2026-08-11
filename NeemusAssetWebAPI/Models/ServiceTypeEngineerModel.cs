using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeemusAssetWebAPI.Models
{
    [Table("servicetypeEngineer")]
    public class ServiceTypeEngineerModel
    {
        [Key]
        public int AssignEnginerid { get; set; }

        public int? ServiceTypeID { get; set; }

        public int? Custodianid { get; set; }

        public string? Status { get; set; }

        public DateTime? Createddate { get; set; }
    }
}
