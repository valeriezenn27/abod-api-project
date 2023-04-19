using System;
using abod_api_project.Models;

namespace abod_api_project.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProducts(bool includeDeleted);
        Task<Product> GetProductById(int id, bool includeDeleted);
        Task<Product> CreateProduct(Product product);
        Task UpdateProduct(int id, Product product);
        Task DeleteProduct(int id);
    }
}

