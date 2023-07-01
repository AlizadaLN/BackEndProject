using System.ComponentModel.DataAnnotations;

namespace BackEndProject.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        public bool IsMain { get; set; }

        public Nullable<int> ParentId { get; set; }
        public Category Parent { get; set; }

        public ICollection<Category> Children { get; set; }
    }
}
