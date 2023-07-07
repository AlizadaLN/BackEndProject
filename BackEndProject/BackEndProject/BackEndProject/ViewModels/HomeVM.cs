using BackEndProject.Models;

namespace BackEndProject.ViewModels
{
    public class HomeVM
    {
        public List<Slider> Sliders { get; set; }
       
       
        public List<Banner> Banners { get; set; }
        public List<Brand> Brands { get; set; }

        public List<FeaturesBannerArea> FeaturesBannerAreas { get; set; }
        public List<Blog> Blogs { get; set; }
        public List<Product> Products { get; set; }

        public List<Category> Categories { get; set; }

    }
}
