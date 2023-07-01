using BackEndProject.ViewModels.AdminVM;
using Microsoft.AspNetCore.Hosting;


namespace BackEndProject.Helper
{
    public static class Extension
    {
        public static bool CheckFileType(this IFormFile file)
        {
            return file.ContentType.Contains("image");
        }

        public static bool CheckFileSize(this IFormFile file, int size)
        {
            return file.Length > size;
        }

        public static string SaveImage(this IFormFile file, IWebHostEnvironment webHostEnvironment, string folder)
        {
            string fileName = Guid.NewGuid() + file.FileName;
            string path = Path.Combine(webHostEnvironment.WebRootPath, folder, fileName);
            // string path=_webHostEnvironment.WebRootPath + @"\img\" + sliderCreateVM.Photo.FileName;


            using (FileStream stream = new FileStream("path", FileMode.Create))
            {
                file.CopyTo(stream);
            };

            return fileName;
        }
    }
}
