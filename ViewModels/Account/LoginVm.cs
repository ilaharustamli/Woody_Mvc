namespace Woody_Mvc.ViewModels.Account
{
    public record LoginVm
    {
        public string EmailOrUsername { get; set; }

        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
