using BackEndProject.Models;
using BackEndProject.ViewModels.AdminVM;
using BackEndProject.ViewModels.AdminVM.Product;
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
            string path = Path.Combine(webHostEnvironment.WebRootPath,"assets", folder, fileName);
            // string path=_webHostEnvironment.WebRootPath + @"\img\" + sliderCreateVM.Photo.FileName;
            

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                file.CopyTo(stream);
            };

            return fileName;
            //
            //string path = _webHostEnvironment.ContentRootPath + @"wwwroot\assets\images\" + "EHEHHEHEH1231321" + productCreateVM.Photos[0].FileName;
            //image.ImageUrl = "EHEHHEHEH1231321" + productCreateVM.Photos[0].FileName;
            //image.ProductId = productCreateVM.Id;
            //using (FileStream stream = new FileStream(path, FileMode.Create))
            //{
            //    productCreateVM.Photos[0].CopyTo(stream);
            //};
        }
        public static void DeleteImage(this Image image,string path)
        {
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

        }
    }
}
