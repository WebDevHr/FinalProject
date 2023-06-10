using FinalProject.Service.Data;
using FinalProject.Service.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalProject.Service.Core
{
    public class ProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ProductViewModel>> GetAll()
        {
            var products = await _context.Products.Include(p => p.Favorites).ToListAsync();
            var productsViewModel = products.Select( c => new ProductViewModel 
            { 
                Id = c.Id,
                Title = c.Title,
                Price = c.Price,
                Description = c.Description,
                Category = c.Category,
                Image = c.Image,
                Rating = c.Rating,
                Count = c.Count,
                Favorites = c.Favorites,
            }).ToList();
            return productsViewModel;
        }

        public async Task<List<ProductViewModel>> GetSearch(string query)
        {
            var searchpProducts = await _context.Products.Where(p => p.Title.Contains(query)).ToListAsync();
            var searchProductsViewModel = searchpProducts.Select(c => new ProductViewModel
            {
                Id = c.Id,
                Title = c.Title,
                Price = c.Price,
                Description = c.Description,
                Category = c.Category,
                Image = c.Image,
                Rating = c.Rating,
                Count = c.Count,
                Favorites = c.Favorites,
            }).ToList();
            return searchProductsViewModel;
        }

        public async Task<ProductViewModel?> GetById(int? id)
        {
            var isProduct = ProductDbExist();
            if (!isProduct)
                return null;

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            var productViewModel =  new ProductViewModel
            {
                Id = product.Id,
                Title = product.Title,
                Price = product.Price,
                Description = product.Description,
                Category = product.Category,
                Image = product.Image,
                Rating = product.Rating,
                Count = product.Count,
                Favorites = product.Favorites,
            };
            return productViewModel;
        }

        public async Task CreateProduct(ProductViewModel productViewModel)
        {
            var product = new Product
            {
                Title = productViewModel.Title,
                Price = productViewModel.Price,
                Description = productViewModel.Description,
                Category = productViewModel.Category,
                Image = productViewModel.Image,
                Rating = productViewModel.Rating,
                Count = productViewModel.Count,
            };
            _context.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task EditProduct(ProductViewModel productViewModel)
        {
            var product = new Product
            {
                Id = productViewModel.Id,
                Title = productViewModel.Title,
                Price = productViewModel.Price,
                Description = productViewModel.Description,
                Category = productViewModel.Category,
                Image = productViewModel.Image,
                Rating = productViewModel.Rating,
                Count = productViewModel.Count,
                Favorites= productViewModel.Favorites,
            };
            _context.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            await _context.SaveChangesAsync();
        }

        public bool ProductDbExist()
        {
            var isProduct = _context.Products;
            if (isProduct != null)
                return true;
            return false;
        }


        public bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }


    }
}
