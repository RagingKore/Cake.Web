### New in 0.0.7 (Released 2015/08/17)
* Path fixes by Jake Scott
* Use GetPhysicalDirectory for ApplicationSettings and WebsiteSettings

### New in 0.0.6 (Released 2015/08/14)
* AppVeyor install script for IIS and WAS
* Renamed PhysicalPath to PhysicalDirectory in SiteSettings
* Use DirectoryPath instead of string to allow for paths relative to the working directory
* Change manager constructors to include the cake environment
* Add Thread.Sleep exception to start / Stop methods to take into account for IIS delays

### New in 0.0.5 (Released 2015/08/13)
* New UnitTests and Bugfixes by Jake Scott
* AppPool TimeSpan checks
* Added cake build example
* Added troubleshooting info

### New in 0.0.4 (Released 2015/08/10)
* Seperate out BindingSettings
* Add / Remove site binding
* Add / Remove site application
* Change Debug to Information logs

### New in 0.0.3 (Released 2015/08/09)
* ProcessModel settings
* RequestTracing settings
* Shared Web and Ftp create function

### New in 0.0.2 (Released 2015/08/06)
* Rewrite by Phillip Sharpe
* Add support for remote servers
* Replace mixed functionality deploy methods with focused create / remove methods
* Add missing Aliases for sites and pools

### New in 0.0.1 (Released 2015/03/31)
* First release by Sergio Silveira.