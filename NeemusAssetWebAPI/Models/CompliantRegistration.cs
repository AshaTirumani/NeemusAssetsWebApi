using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;


namespace NeemusAssetWebAPI.Models
{
    [Table("ComplaintRegistration")]
    public class ComplaintRegistration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ComplaintID")]
        public int ComplaintID { get; set; }

        [Column("AssetClassID")]
        public int? AssetClassID { get; set; }

        [Column("AssetID")]
        public int? AssetID { get; set; }

        [Column("EmployeeID")]
        public string? EmployeeID { get; set; }

        [Column("ApproverID")]
        public string? ApproverID { get; set; }

        [Column("ApproverName")]
        public string? ApproverName { get; set; }

        [Column("ApproverComments")]
        public string? ApproverComments { get; set; }

        [Column("EmployeeName")]
        public string? EmployeeName { get; set; }

        [Column("EmployeeDepartment")]
        public string? EmployeeDepartment { get; set; }

        [Column("EmployeeDesignation")]
        public string? EmployeeDesignation { get; set; }

        [Column("ServiceTypeID")]
        public int? ServiceTypeID { get; set; }

        [Column("OccupantID")]
        public int? OccupantID { get; set; }

        [Column("FilePath")]
        public string? FilePath { get; set; }

        [Column("CreatedDate")]
        public DateTime? CreatedDate { get; set; }

        [Column("Status")]
        public string? Status { get; set; }

        [Column("ComplaintCode")]
        public string? ComplaintCode { get; set; }

        [Column("Complaint_Description")]
        public string? Complaint_Description { get; set; }

        [Column("EscalateRemarks")]
        public string? EscalateRemarks { get; set; }

        [Column("FileDocument")]
        public string? FileDocument { get; set; }

        [Column("AccommodationID")]
        public int? AccommodationID { get; set; }

        [Column("OfficeID")]
        public int? OfficeID { get; set; }

        [Column("Comments")]
        public string? Comments { get; set; }

        [Column("Re_OpenDate")]
        public DateOnly? Re_OpenDate { get; set; }

        [Column("ComplaintType")]
        public string? ComplaintType { get; set; }

        [Column("Sequence")]
        public string? Sequence { get; set; }

        [Column("ComplainPriority")]
        public string? ComplainPriority { get; set; }

        [Column("closedDate")]
        public DateTime? ClosedDate { get; set; }

        [Column("closedcomments")]
        public string? ClosedComments { get; set; }
        [NotMapped]
        public IFormFile? File { get; set; }
    }
}