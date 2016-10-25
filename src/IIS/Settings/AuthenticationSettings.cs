namespace Cake.IIS
{
    public class AuthenticationSettings
    {
        #region Properties (5)
            public string Username { get; set; }

            public string Password { get; set; }



            public bool EnableBasicAuthentication { get; set; }

            public bool EnableWindowsAuthentication { get; set; }

            public bool EnableAnonymousAuthentication { get; set; }
        #endregion
    }
}