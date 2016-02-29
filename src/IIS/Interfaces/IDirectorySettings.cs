#region Using Statements
    using Cake.Core.IO;
#endregion



namespace Cake.IIS
{
    /// <summary>
    ///  Common interface for location based settings
    /// </summary>
    public interface IDirectorySettings
    {
        string ComputerName { get; set; }



        DirectoryPath WorkingDirectory { get; set; }
        
        DirectoryPath PhysicalDirectory { get; set; }
    }
}