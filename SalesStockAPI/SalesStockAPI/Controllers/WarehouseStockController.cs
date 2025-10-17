using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesAPI.Data;
using SalesAPI.Models;

namespace SalesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseStockController : ControllerBase
    {
        private readonly AppDbContext _context;

        public WarehouseStockController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/warehousestock
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetAllStock()
        {
            var stock = await _context.WarehouseStock.ToListAsync();

            return Ok(stock.Select(s => new {
                s.WarehouseId,
                s.Sku,
                s.ProductName,
                s.QuantityAvailable,
                s.MinimumLevel,
                s.LastUpdated,
                s.StockStatus,
                s.IsLowStock
            }));
        }

        // GET: api/warehousestock/{warehouseId}
        [HttpGet("{warehouseId}")]
        public async Task<ActionResult> GetWarehouseStock(string warehouseId)
        {
            var stock = await _context.WarehouseStock
                .Where(s => s.WarehouseId == warehouseId)
                .ToListAsync();

            if (!stock.Any())
                return NotFound($"Warehouse {warehouseId} not found");

            return Ok(stock.Select(s => new {
                s.WarehouseId,
                s.Sku,
                s.ProductName,
                s.QuantityAvailable,
                s.MinimumLevel,
                s.LastUpdated,
                s.StockStatus
            }));
        }

        // GET: api/warehousestock/low-stock
        [HttpGet("low-stock")]
        public async Task<ActionResult> GetLowStock()
        {
            var stock = await _context.WarehouseStock.ToListAsync();

            var lowStock = stock.Where(s => s.IsLowStock)
                .Select(s => new {
                    s.WarehouseId,
                    s.Sku,
                    s.ProductName,
                    s.QuantityAvailable,
                    s.MinimumLevel,
                    s.StockStatus,
                    ShortageAmount = s.MinimumLevel - s.QuantityAvailable
                })
                .OrderByDescending(s => s.ShortageAmount)
                .ToList();

            return Ok(lowStock);
        }

        // GET: api/warehousestock/out-of-stock
        [HttpGet("out-of-stock")]
        public async Task<ActionResult> GetOutOfStock()
        {
            var outOfStock = await _context.WarehouseStock
                .Where(s => s.QuantityAvailable == 0)
                .Select(s => new {
                    s.WarehouseId,
                    s.Sku,
                    s.ProductName,
                    s.MinimumLevel,
                    s.LastUpdated
                })
                .ToListAsync();

            return Ok(outOfStock);
        }

        // GET: api/warehousestock/by-product/{sku}
        [HttpGet("by-product/{sku}")]
        public async Task<ActionResult> GetStockByProduct(string sku)
        {
            var stock = await _context.WarehouseStock
                .Where(s => s.Sku == sku)
                .Select(s => new {
                    s.WarehouseId,
                    s.QuantityAvailable,
                    s.MinimumLevel,
                    s.StockStatus
                })
                .ToListAsync();

            if (!stock.Any())
                return NotFound($"Product {sku} not found in any warehouse");

            var totalQuantity = stock.Sum(s => s.QuantityAvailable);

            return Ok(new
            {
                sku,
                totalQuantity,
                warehouses = stock
            });
        }

        // GET: api/warehousestock/stats
        [HttpGet("stats")]
        public async Task<ActionResult> GetStockStats()
        {
            var stock = await _context.WarehouseStock.ToListAsync();

            var totalProducts = stock.Count;
            var totalQuantity = stock.Sum(s => s.QuantityAvailable);
            var lowStockCount = stock.Count(s => s.IsLowStock);
            var outOfStockCount = stock.Count(s => s.QuantityAvailable == 0);
            var warehouses = stock.Select(s => s.WarehouseId).Distinct().Count();

            return Ok(new
            {
                totalProducts,
                totalQuantity,
                lowStockCount,
                outOfStockCount,
                totalWarehouses = warehouses,
                avgQuantityPerProduct = totalProducts > 0 ? totalQuantity / totalProducts : 0
            });
        }
    }
}
