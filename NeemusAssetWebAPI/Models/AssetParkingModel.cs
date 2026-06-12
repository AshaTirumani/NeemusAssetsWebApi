using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeemusAssetWebAPI.Models
{
    [Table("AssetParking")]
    public class AssetParkingModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AssetParkingID { get; set; }

        public int AssetID { get; set; }

        public string? MainAssetNumber { get; set; }

        public string? CustodianID { get; set; }

        public string? RequestBy { get; set; }

        public string? CustodianDepartment { get; set; }

        public string? CustDepartmentCode { get; set; }

        public string? CustDesignation { get; set; }

        public int? AssetTypeID { get; set; }

        public DateTime? Date { get; set; }

        public string? Status { get; set; }

        public string? Location { get; set; }

        public string? LocationCode { get; set; }

        public int? AssetClassID { get; set; }

        public string? AdminID { get; set; }
    }
}