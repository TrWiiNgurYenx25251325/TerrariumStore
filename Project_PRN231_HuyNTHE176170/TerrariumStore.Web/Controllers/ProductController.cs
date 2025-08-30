using Microsoft.AspNetCore.Mvc;
using TerrariumStore.Web.Services;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace TerrariumStore.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index(int? categoryId, decimal? minPrice, decimal? maxPrice, string searchTerm, string sortBy, string sortOrder)
        {
            Console.WriteLine($"Received parameters: categoryId={categoryId}, minPrice={minPrice}, maxPrice={maxPrice}, searchTerm={searchTerm}");
            
            // Lấy danh sách sản phẩm từ API
            var products = await _productService.GetProductsAsync();
            
            // Log số lượng sản phẩm ban đầu
            Console.WriteLine($"Total products before filtering: {products.Count}");
            
            // Kiểm tra danh sách các danh mục có sẵn từ sản phẩm
            var categories = products
                .Where(p => p.Category != null)
                .Select(p => new { p.Category.Id, p.Category.Name })
                .Distinct()
                .ToList();
            
            Console.WriteLine($"Available categories: {string.Join(", ", categories.Select(c => $"{c.Id}:{c.Name}"))}");
            
            // Kiểm tra xem categoryId có giá trị hợp lệ không
            string categoryName = "Tất cả sản phẩm";
            
            // Lọc theo danh mục nếu có
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                Console.WriteLine($"Filtering by category ID: {categoryId.Value}");
                
                // Lọc theo CategoryId thay vì Category.Id
                var filteredProducts = products.Where(p => p.CategoryId == categoryId.Value).ToList();
                
                Console.WriteLine($"Found {filteredProducts.Count} products in category {categoryId.Value}");
                products = filteredProducts;
                
                // Lấy tên danh mục
                var category = categories.FirstOrDefault(c => c.Id == categoryId.Value);
                if (category != null)
                {
                    categoryName = category.Name;
                }
            }
            
            // Lọc theo giá nếu có
            if (minPrice.HasValue)
            {
                products = products.Where(p => p.Price >= minPrice.Value).ToList();
            }
            
            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.Price <= maxPrice.Value).ToList();
            }
            
            // Lọc theo từ khóa tìm kiếm nếu có
            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                products = products.Where(p => 
                    p.Name.ToLower().Contains(searchTerm) || 
                    (p.Description != null && p.Description.ToLower().Contains(searchTerm)) ||
                    (p.Category != null && p.Category.Name.ToLower().Contains(searchTerm))
                ).ToList();
            }
            
            // Sắp xếp
            if (!string.IsNullOrEmpty(sortBy))
            {
                bool isAscending = sortOrder?.ToLower() != "desc";
                
                switch (sortBy.ToLower())
                {
                    case "name":
                        products = isAscending 
                            ? products.OrderBy(p => p.Name).ToList()
                            : products.OrderByDescending(p => p.Name).ToList();
                        break;
                    case "price":
                        products = isAscending
                            ? products.OrderBy(p => p.Price).ToList()
                            : products.OrderByDescending(p => p.Price).ToList();
                        break;
                    default:
                        // Mặc định sắp xếp theo ID
                        products = isAscending
                            ? products.OrderBy(p => p.Id).ToList()
                            : products.OrderByDescending(p => p.Id).ToList();
                        break;
                }
            }
            
            // Lưu các tham số lọc vào ViewBag để sử dụng trong View
            ViewBag.CategoryId = categoryId;
            ViewBag.CategoryName = categoryName;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.SearchTerm = searchTerm;
            ViewBag.SortBy = sortBy;
            ViewBag.SortOrder = sortOrder;
            
            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                return RedirectToAction("Index");
            }
            
            // Truyền tham số id vào View để sử dụng trong JavaScript
            ViewBag.ProductId = id;
            
            // Chỉ gọi API từ View bằng AJAX, không cần truyền model vào view
            return View();
        }
    }
}
