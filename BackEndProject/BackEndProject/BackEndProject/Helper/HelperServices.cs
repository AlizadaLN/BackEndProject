using System.IO;

namespace BackEndProject.Helper
{
    public class HelperServices
    {
        public static void DeleteFile(string path)
        {
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }
    }
}
