using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeemusAssetWebAPI.Models
{
    [Table("SAPUpdateLogInfo")]
    public class SAPUpdateLogInfoModel
    {
        [Key]
        public int SAPUpdateID { get; set; }

        public string? Log_Message { get; set; }

        public string? EmployeeID { get; set; }

        public DateTime? PerformedDate { get; set; }

        public string? MainAssetNumber { get; set; }

        public string? AssetSubNumber { get; set; }

        public string? Location { get; set; }

        public string? Status { get; set; }

        public string? CustodianID { get; set; }

        public string? StatusDesc { get; set; }

        public string? CustodianDepartment { get; set; }
    }
}