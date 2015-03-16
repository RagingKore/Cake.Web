using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.Web
{
    [CakeAliasCategory("Web")]
    public static class WebExtensions
    {
        [CakeMethodAlias]
        public static void DeployWebSite(this ICakeContext context, WebSiteSettings settings)
        {
            WebHelper
                .Using(context)
                .DeployWebSite(settings);
        }

        [CakeMethodAlias]
        public static void DeployWebSite(this ICakeContext context, WebSiteSettings settings, string source)
        {
            WebHelper
                .Using(context)
                .DeployWebSite(settings, source);
        }

        [CakeMethodAlias]
        public static void DeployFtpSite(this ICakeContext context, FtpSiteSettings settings)
        {
            WebHelper
                .Using(context)
                .DeployFtpSite(settings);
        }

        //[CakeMethodAlias]
        //public static bool DeleteSite(this ICakeContext context, string name)
        //{
        //    return WebHelper
        //        .Using(context)
        //        .DeleteSite(name);
        //}

        //[CakeMethodAlias]
        //public static bool CheckSite(this ICakeContext context, string name)
        //{
        //    return WebHelper
        //        .Using(context)
        //        .CheckSite(name);
        //}

        //[CakeMethodAlias]
        //public static bool StartSite(this ICakeContext context, string name)
        //{
        //    return WebHelper
        //        .Using(context)
        //        .StartSite(name);
        //}

        //[CakeMethodAlias]
        //public static bool StopSite(this ICakeContext context, string name)
        //{
        //    return WebHelper
        //        .Using(context)
        //        .StopSite(name);
        //}

        //[CakeMethodAlias]
        //public static void CreateApplicationPool(this ICakeContext context, string name)
        //{
        //    WebHelper
        //        .Using(context)
        //        .CreateApplicationPool(new ApplicationPoolSettings { Name = name });
        //}

        //[CakeMethodAlias]
        //public static void CreateApplicationPool(this ICakeContext context, ApplicationPoolSettings settings)
        //{
        //    WebHelper
        //        .Using(context)
        //        .CreateApplicationPool(settings);
        //}

        //[CakeMethodAlias]
        //public static bool DeleteApplicationPool(this ICakeContext context, string name)
        //{
        //    return WebHelper
        //        .Using(context)
        //        .DeleteApplicationPool(name);
        //}

        //[CakeMethodAlias]
        //public static bool RecycleApplicationPool(this ICakeContext context, string name)
        //{
        //    return WebHelper
        //        .Using(context)
        //        .RecycleApplicationPool(name);
        //}

        //[CakeMethodAlias]
        //public static bool CheckApplicationPool(this ICakeContext context, string name)
        //{
        //    return WebHelper
        //        .Using(context)
        //        .CheckApplicationPool(name);
        //}        
    }
}
