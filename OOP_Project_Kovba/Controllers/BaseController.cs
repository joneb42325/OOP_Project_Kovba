using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OOP_Project_Kovba.Models;

namespace OOP_Project_Kovba.Controllers
{
    public class BaseController : Controller
    {
        protected readonly UserManager<ApplicationUser> _userManager;

        public BaseController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            var user = _userManager.GetUserAsync(User).Result;
            if (user != null)
            {
                ViewData["UserFullName"] = user.FullName;
                ViewData["UserEmail"] = user.Email;
            }
        }
    }
}
