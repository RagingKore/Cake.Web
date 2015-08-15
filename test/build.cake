#addin "Cake.IIS"
#addin "Microsoft.Web.Administration"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");





///////////////////////////////////////////////////////////////////////////////
// TASK DEFINITIONS
///////////////////////////////////////////////////////////////////////////////

Task("ApplicationPool-Create")
    .Description("Create a ApplicationPool")
    .Does(() =>
{
    CreatePool(new ApplicationPoolSettings()
    {
        Name = "Test",
        IdentityType = IdentityType.NetworkService
    });
});

Task("Website-Create")
    .Description("Create a Website")
    .IsDependentOn("ApplicationPool-Create")
    .Does(() =>
{
    CreateWebsite(new WebsiteSettings()
    {
        Name = "MyBlog",
        HostName = "blog.website.com",
        PhysicalDirectory = "C:/Websites/Blog",

        ApplicationPool = new ApplicationPoolSettings()
        {
            Name = "Test"
        }
    });
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("Website-Create");





///////////////////////////////////////////////////////////////////////////////
// EXECUTION
///////////////////////////////////////////////////////////////////////////////

RunTarget(target);