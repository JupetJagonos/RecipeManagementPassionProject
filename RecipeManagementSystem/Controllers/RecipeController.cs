using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RecipeManagementSystem.Data;
using RecipeManagementSystem.Models;

namespace RecipeManagementSystem.Controllers
{
    [Authorize]
    public class RecipeController : RecipeManagementSystemController
    {
        private readonly ApplicationDbContext _context;

        public RecipeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Recipes.Include(r => r.Category).ToListAsync());
        }

        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,CategoryId")] Recipe recipe)
        {
            if (ModelState.IsValid)
            {
                recipe.Create(GetUserName());

                _context.Add(recipe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "Name", recipe.CategoryId);

            return View(recipe);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var recipe = await GetRecipeWithDetailsAsync(id);

            if (recipe == null)
                return NotFound();
            
            ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "Name", recipe.CategoryId);
            ViewBag.Ingredients = new SelectList(_context.Ingredients, "IngredientId", "Name");

            return View(recipe);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RecipeId,Name,CategoryId,RecipeIngredients")] Recipe recipe)
        {
            if (id != recipe.RecipeId)
                return NotFound();

            if (ModelState.IsValid)
            {
                var existing = await _context.Recipes.FindAsync(id);

                if (existing is null)
                    return NotFound();

                var existingRecipeIngredients = _context.RecipeIngredients
                    .Where(ri => ri.RecipeId == existing.RecipeId);

                _context.RecipeIngredients.RemoveRange(existingRecipeIngredients);

                // Remove Duplicates from RecipeIngredients
                recipe.RecipeIngredients = recipe.RecipeIngredients
                    .Where(ri => ri.IngredientId != 0)
                    .GroupBy(ri => ri.IngredientId)                    
                    .Select(g => g.First())
                    .ToList();

                existing.Update(recipe, GetUserName());

                _context.Update(existing);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.CategoryId = new SelectList(_context.Categories, "CategoryId", "Name", recipe.CategoryId);
            ViewBag.Ingredients = new SelectList(_context.Ingredients, "IngredientId", "Name");

            return View(recipe);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var recipe = await GetRecipeWithDetailsAsync(id);

            if (recipe == null)
                return NotFound();

            return View(recipe);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);

            if (recipe is not null)
            {
                _context.Recipes.Remove(recipe);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<Recipe?> GetRecipeWithDetailsAsync(int id)
        {
            return await _context.Recipes
                .Include(r => r.Category)
                .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
                .FirstOrDefaultAsync(r => r.RecipeId == id);
        }
    }
}
