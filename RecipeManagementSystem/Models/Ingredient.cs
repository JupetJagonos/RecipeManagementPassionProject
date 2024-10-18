using System.ComponentModel.DataAnnotations;

namespace RecipeManagementSystem.Models
{
    public class Ingredient
    {
        public int IngredientId { get; set; }

        [Required(ErrorMessage = "The Name field is required.")]
        public string Name { get; set; } = string.Empty;

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

        public void Update(Ingredient newValue, string updatedBy)
        {
            Name = newValue.Name;

            // Tracking Properties
            UpdatedBy = updatedBy;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
