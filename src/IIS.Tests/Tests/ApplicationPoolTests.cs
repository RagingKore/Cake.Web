﻿#region Using Statements
    using Microsoft.Web.Administration;
    using Xunit;
#endregion



namespace Cake.IIS.Tests
{
    public class ApplicationPoolTests
    {
        [Fact]
        public void Should_Create_AppPool()
        {
            // Arrange
            var settings = CakeHelper.GetAppPoolSettings();
            CakeHelper.DeletePool(settings.Name);
            
            // Act 
            CakeHelper.CreateApplicationPoolManager().Create(settings);

            // Assert
            Assert.NotNull(CakeHelper.GetPool(settings.Name));
        }

        [Fact]
        public void Should_Delete_AppPool()
        {
            // Arrange
            var settings = CakeHelper.GetAppPoolSettings();
            CakeHelper.CreatePool(settings);

            // Act
            CakeHelper.CreateApplicationPoolManager().Delete(settings.Name);

            // Assert
            Assert.Null(CakeHelper.GetPool(settings.Name));
        }

        [Fact]
        public void Should_Start_AppPool()
        {
            // Arrange
            var settings = CakeHelper.GetAppPoolSettings();

            CakeHelper.CreatePool(settings);
            CakeHelper.StopPool(settings.Name);

            // Act
            CakeHelper.CreateApplicationPoolManager().Start(settings.Name);

            // Assert
            ApplicationPool pool = CakeHelper.GetPool(settings.Name);

            Assert.NotNull(pool);
            Assert.True(pool.State == ObjectState.Started);
        }

        [Fact]
        public void Should_Stop_AppPool()
        {
            // Arrange
            var settings = CakeHelper.GetAppPoolSettings();

            CakeHelper.CreatePool(settings);
            CakeHelper.StartPool(settings.Name);

            // Act
            CakeHelper.CreateApplicationPoolManager().Stop(settings.Name);

            // Assert
            ApplicationPool pool = CakeHelper.GetPool(settings.Name);

            Assert.NotNull(pool);
            Assert.True(pool.State == ObjectState.Stopped);
        }
    }
}