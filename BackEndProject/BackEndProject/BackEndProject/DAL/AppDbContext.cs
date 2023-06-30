using BackEndProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace BackEndProject.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Slider> Sliders { get; set; }
        public DbSet<SlidersContent> SlidersContent { get; set; }
        public DbSet<Banner> Banners { get; set; }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<FeaturesBannerArea> FeaturesBannerAreas { get; set; }

        public DbSet<Blog> Blogs { get; set; }



        //public DbSet<Product> Products { get; set; }
        //public DbSet<Category> Categories { get; set; }
        //public DbSet<Image> Images { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Slider>().HasData(
        //        new Slider
        //        {   Id=1,
        //            ImageUrl = "~/images/slider-1.jpg",
        //            SubTitle = "Save $120 when you buy",
        //            Title = "2020 Virtual Reality Fulldive VR.",
        //            Description = "Explore and immerse in exciting 360 content with Fulldive’s all-in-one virtual reality platform"
        //        },

        //        new Slider
        //        {
        //            Id=2,
        //            ImageUrl = "~/images/slider-2.jpg",
        //            SubTitle = "Save $120 when you buy",
        //            Title = "4K HDR Smart TV 43 Sony Bravia.",
        //            Description = "Explore and immerse in exciting 360 content with Fulldive’s all-in-one virtual reality platform"
        //        }
        //    ) ;
        //}
    }
}
