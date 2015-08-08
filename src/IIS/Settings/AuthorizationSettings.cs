namespace Cake.IIS
{
    public class AuthorizationSettings
    {
        #region Constructor (1)
            public AuthorizationSettings()
            {
                this.AuthorizationType = AuthorizationType.AllUsers;

                this.CanRead           = true;
                this.CanWrite          = true;
            }
        #endregion





        #region Properties (5)
            public AuthorizationType AuthorizationType { get; set; }



            public string[] Users { get; set; }

            public string[] Roles { get; set; }



            public bool CanRead { get; set; }

            public bool CanWrite { get; set; }
        #endregion
    }
}