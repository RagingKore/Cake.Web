namespace Cake.Web
{
    public class ApplicationPoolSettings
    {
        public ApplicationPoolSettings()
        {
            Name                  = "ASP.NET v4.0";
            ManagedRuntimeVersion = "v4.0";
            Autostart             = true;
            IdentityType          = ApplicationPoolIdentityType.ApplicationPoolIdentity;
        }

        public string Name { get; set; }

        public ApplicationPoolIdentityType IdentityType { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string ManagedRuntimeVersion { get; set; }

        public bool ClassicManagedPipelineMode { get; set; }

        public bool Enable32BitAppOnWin64 { get; set; }

        public bool Autostart { get; set; }
    }
}