using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductApi.models;

namespace ProductApi.services.ProductService
{
    public interface IProductService
    {
        Task<List<Product>> GetAll();
        Task<Product?> GetById(int id);
        Task<Product> Create(Product product);
        Task<Product?> Update(int id, Product updated);
        Task<bool> Delete(int id);
        Task<List<Product>> SearchAndFilter(string? keyword, decimal? minPrice, decimal? maxPrice);

        
    }
}