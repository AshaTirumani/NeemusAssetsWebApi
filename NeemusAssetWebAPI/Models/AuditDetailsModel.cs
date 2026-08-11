using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace NeemusAssetWebAPI.Models
{
    [Table("AuditDetails")]
    public class AuditDetailsModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AuditDetailsID { get; set; }
        public string? MainAssetNumber { get; set; }
        public string? AuditStatus { get; set; }
        public string? Comments { get; set; }
        public int? AuditID { get; set; }
        public DateTime? Date { get; set; }
        public int? AssetID { get; set; }
        public string? Status { get; set; }
        public string? AdminRemarks { get; set; }
        public string? CustodianID { get; set; }
        public string? AuditBy { get; set; }
        public string? Location { get; set; }

    }
}
