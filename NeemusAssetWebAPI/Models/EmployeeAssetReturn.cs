using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeemusAssetWebAPI.Models
{
    [Table("EmployeeAssetReturn")]
    public class EmployeeAssetReturn
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AssetReturnID { get; set; }

        public int? AssetID { get; set; }

        public string? EmployeeID { get; set; }

        public string? ToLocation { get; set; }

        public string? ToCustodian { get; set; }

        public string? CustodianComments { get; set; }

        public string? ApproverComments { get; set; }

        public string? AdminComments { get; set; }

        public DateTime? Date { get; set; }

        public string? Status { get; set; }

        public string? CustodianDepartment { get; set; }

        public string? CustDepartmentCode { get; set; }

        public string? CustDesignation { get; set; }

        public string? RequestBy { get; set; }

        public string? ApproverID { get; set; }

        public string? ApproverName { get; set; }

        public string? ApproverDesignation { get; set; }

        public string? ApproverDeptCode { get; set; }

        public string? ApproverDepartment { get; set; }

        public string? AdminID { get; set; }

        public string? AdminName { get; set; }

        public string? AdminDesignation { get; set; }

        public string? AdminDeptCode { get; set; }

        public string? AdminDepartment { get; set; }

        public string? RequestType { get; set; }

        public int? AssetClassID { get; set; }

        public DateTime? AdminDate { get; set; }

        public int? ReturnSequene { get; set; }
    }
}