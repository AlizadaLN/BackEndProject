﻿using System.ComponentModel.DataAnnotations;

namespace BackEndProject.ViewModels.AdminVM.Category
{
    public class CategoryCreateVM
    {
        [Required]
        [MaxLength(10)]
        public string Name { get; set; }

        public bool IsMain { get; set; }

        public Nullable<int> ParentId { get; set; }

       
        public IFormFile Photo { get; set; }
    }
}
