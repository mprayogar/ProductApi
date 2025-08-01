using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductApi.dtos;
using ProductApi.models;
using ProductApi.services;
using ProductApi.services.ProductService;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductController : ControllerBase
{
    private readonly IProductService _service;
    private readonly ILogger<ProductController> _logger;

    public ProductController(IProductService service, ILogger<ProductController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var products = await _service.GetAll();
            return Ok(ApiResponse<List<Product>>.Success(products, "Data produk ditemukan"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAll");
            return StatusCode(500, ApiResponse<string>.Fail("Gagal mengambil data produk"));
        }
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var product = await _service.GetById(id);
            if (product == null)
                return NotFound(ApiResponse<string>.Fail("Produk tidak ditemukan"));

            return Ok(ApiResponse<Product>.Success(product, "Produk ditemukan"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetById");
            return StatusCode(500, ApiResponse<string>.Fail("Gagal mengambil data produk"));
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Product product)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return BadRequest(ApiResponse<List<string>>.Fail(errors, "Validasi gagal"));
        }
        try
        {
            var created = await _service.Create(product);
            return Ok(ApiResponse<Product>.Success(created, "Produk berhasil dibuat"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Gagal membuat produk");
            return StatusCode(500, ApiResponse<string>.Fail("Terjadi kesalahan saat membuat produk"));
        }
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Product product)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return BadRequest(ApiResponse<List<string>>.Fail(errors, "Validasi gagal"));
        }
        try
        {
            var updated = await _service.Update(id, product);
            if (updated == null)
                return NotFound(ApiResponse<string>.Fail("Produk tidak ditemukan"));

            return Ok(ApiResponse<Product>.Success(updated, "Produk berhasil diperbarui"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Gagal memperbarui produk");
            return StatusCode(500, ApiResponse<string>.Fail("Terjadi kesalahan saat memperbarui produk"));
        }
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var deleted = await _service.Delete(id);
            if (!deleted)
                return NotFound(ApiResponse<string>.Fail("Produk tidak ditemukan"));

            return Ok(ApiResponse<string>.Success("Produk berhasil dihapus"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Gagal menghapus produk");
            return StatusCode(500, ApiResponse<string>.Fail("Terjadi kesalahan saat menghapus produk"));
        }
    }


    [HttpGet("filter")]
    public async Task<IActionResult> SearchAndFilter(
        [FromQuery] string? keyword,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice)
    {
        try
        {
            // Validasi range harga
            if (minPrice.HasValue && maxPrice.HasValue && minPrice > maxPrice)
            {
                return BadRequest(ApiResponse<string>.Fail("minPrice tidak boleh lebih besar dari maxPrice"));
            }

            var results = await _service.SearchAndFilter(keyword, minPrice, maxPrice);
            return Ok(ApiResponse<List<Product>>.Success(results, "Produk ditemukan"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Gagal mencari/filter produk");
            return StatusCode(500, ApiResponse<string>.Fail("Terjadi kesalahan saat pencarian/filter produk"));
        }
    }


}
