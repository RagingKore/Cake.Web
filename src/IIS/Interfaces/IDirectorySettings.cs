#region Using Statements
    using System.Collections.Generic;

    using Microsoft.Web.Administration;
#endregion


#region Using Statements
    using Cake.Core.IO;
#endregion



namespace Cake.IIS
{
    public interface IDirectorySettings
    {
        string ComputerName { get; set; }



        DirectoryPath WorkingDirectory { get; set; }
        
        DirectoryPath PhysicalDirectory { get; set; }
    }
}