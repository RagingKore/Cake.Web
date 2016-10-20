#region Using Statements
    using Microsoft.Web.Administration;
    using Xunit;
#endregion



namespace Cake.IIS.Tests
{
    public class WebFarmTests
    {
        [Fact]
        public void Should_Create_WebFarm()
        {
            // Arrange
            var settings = CakeHelper.GetWebFarmSettings();
            CakeHelper.DeleteWebFarm(settings.Name);

            // Act
            WebFarmManager manager = CakeHelper.CreateWebFarmManager();
            manager.Create(settings);

            // Assert
            Assert.NotNull(CakeHelper.GetWebFarm(settings.Name));
        }

        [Fact]
        public void Should_Delete_WebFarm()
        {
            // Arrange
            var settings = CakeHelper.GetWebFarmSettings();
            CakeHelper.CreateWebFarm(settings);

            // Act
            CakeHelper.CreateWebFarmManager().Delete(settings.Name);

            // Assert
            Assert.Null(CakeHelper.GetWebFarm(settings.Name));
        }



        [Fact]
        public void Should_Set_Server_Available()
        {
            // Arrange
            var settings = CakeHelper.GetWebFarmSettings();
            CakeHelper.CreateWebFarm(settings);

            // Act
            WebFarmManager manager = CakeHelper.CreateWebFarmManager();
            manager.SetServerAvailable(settings.Name, settings.Servers[0]);

            // Assert
            Assert.True(manager.GetServerState(settings.Name, settings.Servers[0]) == "Avaiable");
        }

        [Fact]
        public void Should_Set_Server_Unavailable()
        {
            // Arrange
            var settings = CakeHelper.GetWebFarmSettings();
            CakeHelper.CreateWebFarm(settings);

            // Act
            WebFarmManager manager = CakeHelper.CreateWebFarmManager();
            manager.SetServerUnavailable(settings.Name, settings.Servers[0]);

            // Assert
            Assert.True(manager.GetServerState(settings.Name, settings.Servers[0]) == "Unavailable");
        }
    }
}