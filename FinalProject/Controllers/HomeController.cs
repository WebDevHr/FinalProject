using FinalProject.Service.Data;
using FinalProject.Service.Models;
using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

namespace FinalProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDBContext _db;

        public HomeController(ILogger<HomeController> logger, AppDBContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Search(User SubmitedForm)
        {
            if (SubmitedForm != null)
            {
                var user = _db.Users.FirstOrDefault(User => User.Email == SubmitedForm.Email && User.Password == SubmitedForm.Password);
                if (user == null)
                {
                    TempData["ErrorMessage"] = "Kullanci adı veya Şifre yanlış!";
                    return RedirectToAction("Login");
                }
            }
            return RedirectToAction("Index");
        }

        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Save(User SubmitedForm)
        {
            if (ModelState.IsValid)
            {
                var user = _db.Users.FirstOrDefault(User => User.Email == SubmitedForm.Email || User.PhoneNumber == SubmitedForm.PhoneNumber);
                if (user != null)
                {
                    TempData["ErrorMessage"] = "Bu kullancı kayıtlı!";
                    return RedirectToAction("Signup");
                }

                _db.Users.Add(SubmitedForm);
                _db.SaveChanges();
                TempData["Success"] = "Kayıt başarılı";
                return RedirectToAction("Index");
            }
            TempData["Unsuccess"] = "Kayıt başarısız";
            return View("Signup", SubmitedForm);
        }

        public IActionResult Cart()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}