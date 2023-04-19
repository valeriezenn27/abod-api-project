using System;
using abod_api_project.Exceptions;
using abod_api_project.Models;
using Microsoft.EntityFrameworkCore;

namespace abod_api_project.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _dbContext;

        public ProductService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Product>> GetAllProducts(bool includeDeleted)
        {
            if (includeDeleted)
            {
                return await _dbContext.Products.ToListAsync();
            }
            else
            {
                return await _dbContext.Products.Where(p => !p.Deleted).ToListAsync();
            }
        }

        public async Task<Product> GetProductById(int id, bool includeDeleted)
        {
            var product = await _dbContext.Products.FindAsync(id);

            if (product == null)
            {
                throw new CustomException($"Product with id {id} not found");
            }

            if (!includeDeleted && product.Deleted)
            {
                throw new CustomException($"Product with id {id} has been deleted");
            }

            return product;
        }

        public async Task<Product> CreateProduct(Product product)
        {
            product.CreatedAt = DateTime.UtcNow;
            product.UpdatedAt = DateTime.UtcNow;

            await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();

            return product;
        }

        public async Task UpdateProduct(int id, Product product)
        {
            var existingProduct = await _dbContext.Products.FindAsync(id);

            if (existingProduct == null)
            {
                throw new CustomException($"Product with id {id} not found");
            }

            if (existingProduct.Deleted)
            {
                throw new CustomException($"Product with id {id} has been deleted");
            }

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteProduct(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);

            if (product == null)
            {
                throw new CustomException($"Product with id {id} not found");
            }

            if (product.Deleted)
            {
                throw new CustomException($"Product with id {id} has already been deleted");
            }

            product.Deleted = true;
            product.DeletedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();
        }
    }

}

