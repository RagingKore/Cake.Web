namespace Cake.Web
{
    public class ApplicationPoolSettings
    {
        public ApplicationPoolSettings()
        {
            // defaults
            ManagedRuntimeVersion = "v4.0";
            Autostart             = true;
        }

        public string Name { get; set; }

        public string ManagedRuntimeVersion { get; set; }

        public bool ClassicManagedPipelineMode { get; set; }

        public bool Enable32BitAppOnWin64 { get; set; }

        public bool Autostart { get; set; }
    }
}