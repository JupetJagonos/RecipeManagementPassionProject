using Microsoft.AspNetCore.Mvc;

namespace RecipeManagementSystem.Controllers
{
    public class RecipeManagementSystemController : Controller
    {
        protected string GetUserName()
        {
            return User.Identity.Name ?? "Unknown";
        }
    }
}
