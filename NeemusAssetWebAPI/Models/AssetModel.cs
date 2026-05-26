using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace NeemusAssetWebAPI.Models
{
    [Table("AssetMaster")]
    public class AssetModel
    {
        [Key]
        public int SLNO { get; set; }
        public int AssetID { get; set; }
        public string? MainAssetNumber { get; set; }
        public string? AssetSubNumber { get; set; }
        public string? CustodianDepartment { get; set; }
        public string? AssetClass { get; set; }
        public string? AssetType { get; set; }
        public string? AssetDesc { get; set; }
        public string? SerialNumber { get; set; }
        public string? Model { get; set; }
        public string? Make { get; set; }
        public string? YearofPurchase { get; set; }
        public DateTime? FirstAcquisitionDate { get; set; }
        public DateTime? AssetCapitalizationDate { get; set; }
        public string? Unit { get; set; }
        public string? CustodianID { get; set; }
        public string? Location { get; set; }
        public string? Cost { get; set; }
        public string? Component { get; set; }
        public string? GRNumber { get; set; }
        public string? Indentor { get; set; }
        public DateTime? WarrantyDate { get; set; }
        public string? Remarks { get; set; }

        public DateTime? CreationDate { get; set; }
    }
}
