using System.ComponentModel.DataAnnotations;

namespace BackEndProject.ViewModels.AdminVM.Category
{
    public class CategoryUpdateVM
    {
       
            public int Id { get; set; }

            [Required]
            [MaxLength(10)]
            public string Name { get; set; }

            public bool IsMain { get; set; }

            public int? ParentId { get; set; }
        

    }
}
