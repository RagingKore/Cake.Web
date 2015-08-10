#region Using Statements
    using System;
#endregion



namespace Cake.IIS
{
    public class ApplicationSettings
    {
        #region Constructor (1)
            public ApplicationSettings()
            {

            }
        #endregion





        #region Properties (7)
            public string SiteName { get; set; }

            public string ApplicationPath { get; set; }
        


            public string ApplicationPool { get; set; }
                


            public string VirtualDirectoryPath { get; set; }
                        
            public string PhysicalPath { get; set; }
          


            public string UserName { get; set; }
                  
            public string Password { get; set; }
        #endregion
    }
}