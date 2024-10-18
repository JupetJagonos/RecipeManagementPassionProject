using System.ComponentModel.DataAnnotations;

namespace RecipeManagementSystem.Models
{
    public class Recipe
    {
        public int RecipeId { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; } = string.Empty;

        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }

        public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();

        public string CreatedBy { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public void Create(string createdBy)
        {
            CreatedBy = createdBy;
            CreatedAt = DateTime.UtcNow;
        }

        public void Update(Recipe newValue, string updatedBy)
        {
            Name = newValue.Name;
            RecipeIngredients = newValue.RecipeIngredients;
            CategoryId = newValue.CategoryId;

            UpdatedBy = updatedBy;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
