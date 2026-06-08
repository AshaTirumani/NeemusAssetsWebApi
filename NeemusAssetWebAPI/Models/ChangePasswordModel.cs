namespace NeemusAssetWebAPI.Models
{
    public class ChangePasswordModel
    {
        public string CustodianID { get; set; } = string.Empty;

        public string OldPassword { get; set; } = string.Empty;

        public string NewPassword { get; set; } = string.Empty;
    }
}
