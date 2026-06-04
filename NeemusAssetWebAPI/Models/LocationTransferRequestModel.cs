namespace NeemusAssetWebAPI.Models
{
    public class LocationTransferRequestModel
    {
        public int AssetID { get; set; }

        public int AssetClassID { get; set; }

        public int LocationID { get; set; }

        public string? ToLocation { get; set; }

        public string? CustodianComments { get; set; }

        public string? EmployeeID { get; set; }

        public string? RequestBy { get; set; }

        public string? CustodianDepartment { get; set; }

        public string? CustDesignation { get; set; }

        public string? ApproverID { get; set; }
    }
}
