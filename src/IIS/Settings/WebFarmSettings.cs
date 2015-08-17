namespace Cake.IIS
{
    public class WebFarmSettings
    {
        #region Constructor (1)
            public WebFarmSettings()
            {

            }
        #endregion





        #region Properties (2)
            public string Name { get; set; }

            public string[] Servers { get; set; }
        #endregion
    }
}