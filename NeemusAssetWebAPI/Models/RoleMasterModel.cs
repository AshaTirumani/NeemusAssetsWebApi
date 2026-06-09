using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeemusAssetWebAPI.Models
{
    [Table("RoleMaster")]
    public class RoleMasterModel
    {
        public int ROLE_ID { get; set; }
        public string? ROLE_NAME { get; set; }
        public string? CustodianID { get; set; }
        public string? ROLE_STATUS { get; set; }
        public DateTime? CREATE_DATE { get; set; }
    }
}
