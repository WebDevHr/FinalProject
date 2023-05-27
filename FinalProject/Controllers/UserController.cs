using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FinalProject.Service.Models;
using FinalProject.Service.Data;
using FinalProject.Service.Core;
using Microsoft.AspNetCore.Authorization;

namespace FinalProject.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var users = await _userService.GetAllAsync();
            return users != null ?
                        View(users) :
                        Problem("Entity set 'AppDBContext.Product'  is null.");
        }
    }
}
