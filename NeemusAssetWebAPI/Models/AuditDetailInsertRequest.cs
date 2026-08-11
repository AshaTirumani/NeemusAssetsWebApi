using System;

namespace NeemusAssetWebAPI.Models
{
    public class AuditDetailInsertRequest
    {
        public int AssetId { get; set; }

        public int AuditId { get; set; }

        public string? AuditName { get; set; }

        public string? Location { get; set; }

        public string? AssetClass { get; set; }

        public string? MainAssetNumber { get; set; }

        public string? UpdatedLocation { get; set; }

        public string? ChangedCustodian { get; set; }

        public string? UpdatedStatus { get; set; }

        public string? Comments { get; set; }

        public DateTime AuditDate { get; set; }
    }
}