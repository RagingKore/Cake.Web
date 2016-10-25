# Cake.IIS
Cake-Build addin that extends Cake with IIS extensions

[![Build status](https://ci.appveyor.com/api/projects/status/eqvnf0dk25rqsh44?svg=true)](https://ci.appveyor.com/project/SharpeRAD/cake-iis)

[![cakebuild.net](https://img.shields.io/badge/WWW-cakebuild.net-blue.svg)](http://cakebuild.net/)

[![Join the chat at https://gitter.im/cake-build/cake](https://badges.gitter.im/Join%20Chat.svg)](https://gitter.im/cake-build/cake)



## Table of contents

1. [Implemented functionality](https://github.com/SharpeRAD/Cake.IIS#implemented-functionality)
2. [Referencing](https://github.com/SharpeRAD/Cake.IIS#referencing)
3. [Usage](https://github.com/SharpeRAD/Cake.IIS#usage)
4. [Example](https://github.com/SharpeRAD/Cake.IIS#example)
5. [TroubleShooting](https://github.com/SharpeRAD/Cake.IIS#troubleshooting)
6. [Plays well with](https://github.com/SharpeRAD/Cake.IIS#plays-well-with)
7. [License](https://github.com/SharpeRAD/Cake.IIS#license)
8. [Share the love](https://github.com/SharpeRAD/Cake.IIS#share-the-love)



## Implemented functionality

* Create Website / Ftpsite
* Delete Site
* Start Site
* Stop Site
* Site Exists
* Add site binding
* Remove site binding
* Create Application Pool
* Delete Pool
* Start Pool
* Stop Pool
* Pool Exists
* Recycle Pool
* Create site applications
* Create WebFarm
* Delete WebFarm
* Add server to WebFarm
* Delete server from WebFarm
* Server exists
* Set server Healthy
* Set server Unhealthy
* Set server Available
* Set server Unavailable
* Is server Healthy
* Get server State
* Create Virtual Directory
* Delete Virtual Directory
* Virtual Directory Exists



## Referencing

[![NuGet Version](http://img.shields.io/nuget/v/Cake.IIS.svg?style=flat)](https://www.nuget.org/packages/Cake.IIS/) [![NuGet Downloads](http://img.shields.io/nuget/dt/Cake.IIS.svg?style=flat)](https://www.nuget.org/packages/Cake.IIS/)

Cake.IIS is available as a nuget package from the package manager console:

```csharp
Install-Package Cake.IIS
```

or directly in your build script via a cake addin directive:

```csharp
#addin "Cake.IIS"
```



## Usage

```csharp
#addin "Cake.IIS"

Task("ApplicationPool-Create")
    .Description("Create a ApplicationPool")
    .Does(() =>
{
    CreatePool("remote-server-name", new ApplicationPoolSettings()
    {
        Name = "Production",

        Username = "Admin",
        Password = "pass1"
    });
});

Task("ApplicationPool-Stop")
    .Description("Stops a local ApplicationPool")
    .Does(() =>
{
    StopPool("Production");
});

Task("ApplicationPool-Start")
    .Description("Starts a remote ApplicationPool")
    .Does(() =>
{
    StartPool("remote-server-name", "Production");
});



Task("Website-Create")
    .Description("Create a Website")
    .Does(() =>
{
    CreateWebsite("remote-server-name", new WebsiteSettings()
    {
        Name = "MyBlog",
        HostName = "blog.website.com",
        PhysicalDirectory = "C:/Websites/Blog",

        ApplicationPool = new ApplicationPoolSettings()
        {
            Name = "Production"
        }
    });
});

Task("Website-Stop")
    .Description("Stops a remote Website")
    .Does(() =>
{
    StopSite("remote-server-name", "MyBlog");
});

Task("Website-Start")
    .Description("Starts a local Website")
    .Does(() =>
{
    StartSite("MyBlog");
});



Task("Binding-Add")
    .Description("Adds a binding to a website")
    .Does(() =>
{
    AddBinding("MyBlog", new BindingSettings(BindingProtocol.Http)
    {
        HostName = "myblog.com"
    });
});

Task("Binding-Add-Fluent")
    .Description("Adds a binding to a website using a fluent interface")
    .Does(() =>
{
    AddBinding("remote-server-name", "MyBlog", IISBindings.Http
                                                .SetIpAddress("127.0.0.1")
                                                .SetPort(8080));
});

Task("Binding-Remove")
    .Description("Removes a binding from a website")
    .Does(() =>
{
    RemoveBinding("remote-server-name", "MyBlog", new BindingSettings(BindingProtocol.Http)
    {
        HostName = "myblog.com"
    });
});



Task("Application-Create")
    .Description("Adds an application to a site")
    .Does(() =>
{
    AddSiteApplication(new ApplicationSettings()
    {
        SiteName = "Default Website",

        ApplicationPath = "/NestedApp",
        PhysicalDirectory = "C:/Apps/KillerApp/"
    });
});

Task("Application-Remove")
    .Description("Remove an application from a site")
    .Does(() =>
{
    RemoveSiteApplication(new ApplicationSettings()
    {
        SiteName = "Default Website",

        ApplicationPath = "/NestedApp",
        PhysicalDirectory = "C:/Apps/KillerApp/"
    });
});



Task("WebFarm-Create")
    .Description("Create a WebFarm")
    .Does(() =>
{
    CreateWebFarm("remote-server-name", new WebFarmSettings()
    {
        Name = "Batman",
        Servers = new string[] { "Gotham", "Metroplis" }
    });
});

Task("WebFarm-Server-Available")
    .Description("Sets a WebFarm server as available")
    .Does(() =>
{
    SetServerAvailable("remote-server-name", "Batman", "Gotham");
});

Task("WebFarm-Server-Unavailable")
    .Description("Sets a WebFarm server as unavailable")
    .Does(() =>
{
    SetServerUnavailable("remote-server-name", "Batman", "Gotham");
});

Task("VirtualDirectory-Create")
    .Description("Creates a Virtual Directory")
    .Does(() => 
{
    AddSiteVirtualDirectory("remote-server-name", new VirtualDirectorySettings(){
        PhysicalDirectory = "C:/Apps/Directory/",
        SiteName = "Default Website",
        ApplicationPath = "/",
        Path = "/Directory"
    });
});

Task("VirtualDirectory-Remove")
    .Description("Removes a Virtual Directory")
    .Does(() => 
{
    RemoveSiteVirtualDirectory("remote-server-name", new VirtualDirectorySettings(){
        SiteName = "Default Website",
        ApplicationPath = "/",
        Path = "/Directory"
    });
});

Task("VirtualDirectory-Exists")
    .Description("Checks if a Virtual Directory exists")
    .Does(() => 
{
    SiteVirtualDirectoryExists("remote-server-name", new VirtualDirectorySettings(){
        SiteName = "Default Website",
        ApplicationPath = "/",
        Path = "/Directory"
    });
});
RunTarget("Website-Create");
```



## Example

A complete Cake example can be found [here](https://github.com/SharpeRAD/Cake.IIS/blob/master/test/build.cake).



## TroubleShooting

A few pointers for managing IIS can be found [here](https://github.com/SharpeRAD/Cake.IIS/blob/master/TroubleShooting.md).



## Plays well with

If your looking to deploy to IIS its worth checking out [Cake.WebDeploy](https://github.com/SharpeRAD/Cake.WebDeploy) or if your running a WebFarm inside AWS then check out [Cake.AWS.ElasticLoadBalancing](https://github.com/SharpeRAD/Cake.AWS.ElasticLoadBalancing).

If your looking for a way to trigger cake tasks based on windows events or at scheduled intervals then check out [CakeBoss](https://github.com/SharpeRAD/CakeBoss).



## License

Copyright (c) 2015 - 2016 Sergio Silveira, Phillip Sharpe

Cake.IIS is provided as-is under the MIT license. For more information see [LICENSE](https://github.com/SharpeRAD/Cake.IIS/blob/master/LICENSE).



## Share the love

If this project helps you in anyway then please :star: the repository.