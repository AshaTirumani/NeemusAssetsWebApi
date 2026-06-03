using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace NeemusAssetWebAPI.Models
{
    [Table("AssetTypeMaster")]
    public class AssetTypeModel
    {
        [Key]
        public int AssetTypeID { get; set; }
        public string? AssetTypeName { get; set; }
        public string? AssetTypeCode { get; set; }
        public string? AssetClassName { get; set; }
        public int AssetClassID { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
