using System.Collections.Generic;

namespace NeemusAssetWebAPI.Models
{
    public class AuditApprovalRequest
    {
        public int AuditId { get; set; }

        public List<AssetApprovalData> Assets { get; set; }

        public string GlobalRemarks { get; set; }

        public string Action { get; set; }
    }
}