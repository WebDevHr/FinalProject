using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalProject.Service.Models;
using FinalProject.Service.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;



namespace FinalProject.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductService _productService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductsController(ProductService productService, UserManager<ApplicationUser> userManager)
        {
            _productService = productService;
            _userManager = userManager;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAll();
            return products != null ?
                        View(products) :
                        Problem("Entity set 'AppDBContext.Product'  is null.");
        }

        // GET: Products/Category
        public async Task<IActionResult> Category(string? id)
        {
            var products = await _productService.GetAll();
            if (products != null) { 
                if(id == "Man")
                    return View(products.FindAll(item => item.Category == "Erkek"));
                else if (id == "Woman")
                    return View(products.FindAll(item => item.Category == "Kadın"));
                else if (id == "Jewelery")
                    return View(products.FindAll(item => item.Category == "Mücevher"));
                else if (id == "Electronics")
                    return View(products.FindAll(item => item.Category == "Elektronik"));
            } else
                Problem("Entity set 'AppDBContext.Product'  is null.");
                return RedirectToAction("Index");
        }

        public async Task<IActionResult> SearchAsync(string query)
        {
            var searchViewModel = new SearchViewModel { Query = query };

            if (!string.IsNullOrWhiteSpace(query))
            {
                searchViewModel.Results = await _productService.GetSearch(query);
            }

            return View(searchViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> SearchJson(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Json(new List<ProductViewModel>());
            }

            var results =  await _productService.GetSearch(query);

            return Json(results);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var product = await _productService.GetById(id);
            if (id == null || product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Price,Description,Category,Image,Rating")] ProductViewModel product)
        {
            if (ModelState.IsValid)
            {
                await _productService.CreateProduct(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var product = await _productService.GetById(id);
            if (id == null || product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Price,Description,Category,Image,Rating")] ProductViewModel product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _productService.EditProduct(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_productService.ProductExists(product.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var product = await _productService.GetById(id);
            if (id == null || product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool isExist = _productService.ProductDbExist();
            if (!isExist)
                return Problem("Entity set 'AppDBContext.Product'  is null.");

            await _productService.DeleteProduct(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
