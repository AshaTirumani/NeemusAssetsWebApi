using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeemusAssetWebAPI.Models
{
    [Table("AuditMaster")]
    public class AuditMaster
    {
        [Key]
        [Column("AuditID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AuditID { get; set; }

        [Column("AuditDate")]
        public DateTime? AuditDate { get; set; }

        [Column("AuditName")]
        public string? AuditName { get; set; }

        [Column("AuditDescription")]
        public string? AuditDescription { get; set; }

        [Column("UnitNo")]
        public string? UnitNo { get; set; }

        [Column("AuditBy")]
        public string? AuditBy { get; set; }

        [Column("Status")]
        public string? Status { get; set; }

        [Column("AuditStatus")]
        public string? AuditStatus { get; set; }

        [Column("LocationID")]
        public int? LocationID { get; set; }

        [Column("TotalStock")]
        public decimal? TotalStock { get; set; }

        [Column("CustodianDepartment")]
        public string? CustodianDepartment { get; set; }

        [Column("CustDepartmentCode")]
        public string? CustDepartmentCode { get; set; }

        [Column("CustDesignation")]
        public string? CustDesignation { get; set; }

        [Column("CustodianName")]
        public string? CustodianName { get; set; }

        [Column("CompletionDate")]
        public DateTime? CompletionDate { get; set; }

        [Column("AdminRemarks")]
        public string? AdminRemarks { get; set; }
    }
}