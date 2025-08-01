using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProductApi.data;
using ProductApi.models;
using ProductApi.services.ProductService;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAll()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product?> GetById(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<Product> Create(Product product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<Product> Update(int id, Product updated)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return null;

        product.Name = updated.Name;
        product.Description = updated.Description;
        product.Price = updated.Price;

        await _context.SaveChangesAsync();
        return product;
    }

    public async Task<bool> Delete(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null) return false;

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Product>> SearchAndFilter(string? keyword, decimal? minPrice, decimal? maxPrice)
    {
        var query = _context.Products.AsQueryable();

        if (!string.IsNullOrEmpty(keyword))
            query = query.Where(p =>
            p.Name.ToLower().Contains(keyword.ToLower()) ||
            p.Description.ToLower().Contains(keyword.ToLower()));

        if (minPrice.HasValue)
            query = query.Where(p => p.Price >= minPrice.Value);

        if (maxPrice.HasValue)
            query = query.Where(p => p.Price <= maxPrice.Value);

        return await query.ToListAsync();
    }

}
