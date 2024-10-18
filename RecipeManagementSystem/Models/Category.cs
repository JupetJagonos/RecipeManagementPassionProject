using System.ComponentModel.DataAnnotations;

namespace RecipeManagementSystem.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;

        public ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();

        public string CreatedBy { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public void Create(string createdBy)
        {
            // Tracking Properties
            CreatedBy = createdBy;
            CreatedAt = DateTime.UtcNow;
        }

        public void Update(Category newValue, string updatedBy)
        {
            Name = newValue.Name;
            Recipes = newValue.Recipes;

            // Tracking Properties
            UpdatedBy = updatedBy;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
