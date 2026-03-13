using BusinessLayer.Enum;
using BusinessLayer;
using DataLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly SignInManager<User> signInManager;
        public HomeController(SignInManager<User> signInManager)
        {
            this.signInManager = signInManager;
        }
        public IActionResult Index()
        {
            if (signInManager.IsSignedIn(User))
            {
            return User.IsInRole("Administrator") ?RedirectToPage("/Account/Manage/Administration", new { area = "Identity" })
                    : RedirectToPage("/Account/Manage/Hardware", new { area = "Identity" });
            }

            return View();
        }
    }
}
    

