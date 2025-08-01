using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ProductApi.models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nama produk wajib diisi")]
        [StringLength(100, ErrorMessage = "Nama produk maksimal 100 karakter")]
        public string Name { get; set; }

        [StringLength(250, ErrorMessage = "Deskripsi maksimal 250 karakter")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Harga produk wajib diisi")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Harga harus lebih dari 0")]
        public decimal Price { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}