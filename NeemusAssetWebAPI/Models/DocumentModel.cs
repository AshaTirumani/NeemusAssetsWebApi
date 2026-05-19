using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace NeemusAssetWebAPI.Models
{
    [Table("DocumentMaster")]
    public class DocumentModel
    {
        [Key]
        public int DocumentID { get; set; }
        public string? DocumentName { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
