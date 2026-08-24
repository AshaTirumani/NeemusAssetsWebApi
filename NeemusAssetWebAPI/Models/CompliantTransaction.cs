using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeemusAssetWebAPI.Models
{
    [Table("ComplaintTransaction")]
    public class ComplaintTransaction
    {
        [Key]
        [Column("ComplaintTransactionID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ComplaintTransactionID { get; set; }

        [Column("ComplaintID")]
        public int? ComplaintID { get; set; }

        [Column("Status")]
        public string? Status { get; set; }

        [Column("USR_ID")]
        public string? USR_ID { get; set; }

        [Column("Remarks")]
        public string? Remarks { get; set; }

        [Column("CreatedDate")]
        public DateTime? CreatedDate { get; set; }

        [Column("AssignedDate")]
        public DateTime? AssignedDate { get; set; }

        [Column("Comments")]
        public string? Comments { get; set; }

        [Column("AssignedTo")]
        public int? AssignedTo { get; set; }

        [Column("FileDocument")]
        public string? FileDocument { get; set; }

        [Column("ProgressDate")]
        public DateTime? ProgressDate { get; set; }

        [Column("ApproverComments")]
        public string? ApproverComments { get; set; }

        [Column("AsnStatus")]
        public string? AsnStatus { get; set; }

        [Column("ComplaintType")]
        public string? ComplaintType { get; set; }

        [Column("Sequence")]
        public string? Sequence { get; set; }

        [Column("planofActionDate")]
        public DateTime? PlanOfActionDate { get; set; }
    }
}
public class SolveTicketRequest
{
    public string? Status { get; set; }

    public string? Comments { get; set; }
}
public class UserTicketActionRequest
{
    public string? Status { get; set; }

    public string? Comments { get; set; }
}
public class AssignTicketRequest
{
    public int AssignedTo { get; set; }

    public string? ApproverComments { get; set; }
}