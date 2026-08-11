using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeemusAssetWebAPI.Models
{
    [Table("servicetypeApprover")]
    public class ServiceTypeApproverModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Assginserviceid { get; set; }

        public int? Servicetypeid { get; set; }

        public string? Custodianid { get; set; }

        public string? Status { get; set; }

        public DateTime? Createddate { get; set; }
    }
}