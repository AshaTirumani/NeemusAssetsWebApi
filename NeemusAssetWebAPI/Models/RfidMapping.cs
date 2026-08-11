using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NeemusAssetWebAPI.Models
{
    [Table("RFIDMappingHistory")]
    public class RfidMapping
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RFIDMappinghistoryID { get; set; }

        public string? RFIDCardNumber { get; set; }

        public DateTime? RFIDoldMAPDATE { get; set; }

        public int? SRNO { get; set; }

        public string? RFIDHistoryDate { get; set; }

        public string? RFIDStatus { get; set; }
        //public DateTime? RFIDMAPDATE { get; set; }
    }
}