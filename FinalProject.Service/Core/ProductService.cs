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
        private readonly AppDBContext _context;

        public ProductService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAll()
        {
            return await _context.Products.ToListAsync(); ;
        }

        public async Task<List<Product>> GetSearch(string query)
        {
            return await _context.Products.Where(p => p.Title.Contains(query)).ToListAsync();
        }

        public async Task<Product?> GetById(int? id)
        {
            var isProduct = ProductDbExist();
            if (!isProduct)
                return null;

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            return product;
        }

        public async Task CreateProduct(Product product)
        {
            _context.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task EditProduct(Product product)
        {

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
