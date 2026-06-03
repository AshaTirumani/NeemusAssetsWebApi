using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace NeemusAssetWebAPI.Models
{
    [Table("AssetClassMaster")]
    public class AssetClass
    {
        [Key]
        public int AssetClassID { get; set; }

        public string? AssetClassName { get; set; }
        public string? AssetClassCode { get; set; }

        public string? Depreciation { get; set; }

        public string? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
