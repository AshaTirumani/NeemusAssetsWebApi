using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace NeemusAssetWebAPI.Models
{
    public class DocumentMappingModel
    {
        [Table("AssetDocumentMapping")]
        public class AssetDocumentMapping
        {
            [Key]
            public int DocumentMapID { get; set; }

            public int DocumentID { get; set; }

            public string? Status { get; set; }

            public string? ImageLocation { get; set; }

            public string? Date { get; set; }

            public string? MainAssetNumber { get; set; }

            public int AssetID { get; set; }
        }

       
    }

    public class AssetDocumentMappingDto
    {
        public int DocumentID { get; set; }

        public string? Status { get; set; }

        public string? ImageLocation { get; set; }

        public string? Date { get; set; }

        public string? MainAssetNumber { get; set; }

        public int AssetID { get; set; }
    }
}
