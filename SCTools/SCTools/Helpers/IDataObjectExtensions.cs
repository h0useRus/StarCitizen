using System.IO;
using System.Windows.Forms;

namespace NSW.StarCitizen.Tools.Helpers
{
    public static class IDataObjectExtensions
    {
        public static string? GetSingleDirectoryPath(this IDataObject dataObject)
        {
            if (dataObject.GetDataPresent(DataFormats.FileDrop) &&
               dataObject.GetData(DataFormats.FileDrop) is string[] filesList &&
               filesList.Length == 1 && Directory.Exists(filesList[0]))
            {
                return filesList[0];
            }
            return null;
        }
    }
}
