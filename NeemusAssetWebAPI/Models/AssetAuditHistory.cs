using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeemusAssetWebAPI.Models
{
    [Table("AssetAuditHistory")]
    public class AssetAuditHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("AuditHistoryID")]
        public int AuditHistoryID { get; set; }

        [Column("AuditID")]
        public int? AuditID { get; set; }

        [Column("AssetID")]
        public int? AssetID { get; set; }

        [Column("MainAssetNumber")]
        public string? MainAssetNumber { get; set; }

        [Column("AssetLocation")]
        public string? AssetLocation { get; set; }

        [Column("AssetCustodian")]
        public string? AssetCustodian { get; set; }

        [Column("AssetStatus")]
        public string? AssetStatus { get; set; }

        [Column("LocationChangedTo")]
        public string? LocationChangedTo { get; set; }

        [Column("CustodianChangedTo")]
        public string? CustodianChangedTo { get; set; }

        [Column("StatusChangedTo")]
        public string? StatusChangedTo { get; set; }

        [Column("AuditBy")]
        public string? AuditBy { get; set; }

        [Column("AuditorRemarks")]
        public string? AuditorRemarks { get; set; }

        [Column("AuditedDate")]
        public DateTime? AuditedDate { get; set; }

        [Column("ApprovedBy")]
        public string? ApprovedBy { get; set; }

        [Column("ApproverRemarks")]
        public string? ApproverRemarks { get; set; }

        [Column("ApprovedDate")]
        public DateTime? ApprovedDate { get; set; }

        [Column("AuditDetailsID")]
        public int? AuditDetailsID { get; set; }

        [Column("Status")]
        public string? Status { get; set; }

        [Column("AdminDate")]
        public DateTime? AdminDate { get; set; }
    }
}