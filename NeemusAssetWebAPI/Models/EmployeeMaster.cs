using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeemusAssetWebAPI.Models
{
    [Table("CustodianMaster")]
    public class EmployeeMaster
    {
        [Key]
        [Column("CustodianID")]
        public string CustodianID { get; set; } = string.Empty;

        [Column("CustodianDepartmentCode")]
        public string? CustodianDepartmentCode { get; set; }

        [Column("CustodianName")]
        public string? CustodianName { get; set; }

        [Column("Designation")]
        public string? Designation { get; set; }

        [Column("reporting_staff_no")]
        public string? ReportingStaffNo { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("CustodianStatus")]
        public string? CustodianStatus { get; set; }

        [Column("CreateDate")]
        public DateTime? CreateDate { get; set; }

        [Column("LDAP_USERID")]
        public string? LdapUserId { get; set; }

        [Column("InternalNumber")]
        public string? InternalNumber { get; set; }

        [Column("LDAP_PWD")]
        public string? LdapPwd { get; set; }
    }
}