#region Using Statements
    using Cake.Core.IO;
#endregion



namespace Cake.IIS
{
    public class ApplicationSettings : IDirectorySettings
    {
        #region Constructor (1)
            public ApplicationSettings()
            {

            }
        #endregion





        #region Properties (9)
            public string ComputerName { get; set; }



            public string SiteName { get; set; }

            public string ApplicationPath { get; set; }
        


            public string ApplicationPool { get; set; }
                


            public string VirtualDirectory { get; set; }

            public DirectoryPath WorkingDirectory { get; set; }

            public DirectoryPath PhysicalDirectory { get; set; }
          


            public string UserName { get; set; }
                  
            public string Password { get; set; }
        #endregion
    }
}