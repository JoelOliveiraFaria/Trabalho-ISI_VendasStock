using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesAPI.Data;
using SalesAPI.Models;

namespace SalesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SalesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/sales
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetSales()
        {
            var sales = await _context.Sales
                .Take(100)  // Limita a 100 registos
                .ToListAsync();

            return Ok(sales.Select(s => new {
                s.SaleId,
                s.SaleDate,
                s.CustomerId,
                s.CustomerEmail,
                s.ProductName,
                s.Quantity,
                UnitPrice = s.UnitPriceDecimal,
                TotalAmount = s.TotalAmountDecimal,
                s.Status
            }));
        }

        // GET: api/sales/stats
        [HttpGet("stats")]
        public async Task<ActionResult> GetStats()
        {
            var sales = await _context.Sales.ToListAsync();

            var totalSales = sales.Count;
            var totalRevenue = sales.Sum(s => s.TotalAmountDecimal);
            var avgTicket = totalSales > 0 ? sales.Average(s => s.TotalAmountDecimal) : 0;

            return Ok(new
            {
                totalSales,
                totalRevenue,
                avgTicket
            });
        }

        // GET: api/sales/by-product
        [HttpGet("by-product")]
        public async Task<ActionResult> GetSalesByProduct()
        {
            var sales = await _context.Sales.ToListAsync();

            var data = sales
                .GroupBy(s => s.ProductName)
                .Select(g => new {
                    product = g.Key,
                    revenue = g.Sum(s => s.TotalAmountDecimal)
                })
                .OrderByDescending(x => x.revenue)
                .Take(10)
                .ToList();

            return Ok(data);
        }

        // GET: api/sales/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult> GetSale(string id)
        {
            var sale = await _context.Sales.FindAsync(id);

            if (sale == null)
                return NotFound();

            return Ok(new
            {
                sale.SaleId,
                sale.SaleDate,
                sale.CustomerId,
                sale.CustomerEmail,
                sale.ProductName,
                sale.Quantity,
                UnitPrice = sale.UnitPriceDecimal,
                TotalAmount = sale.TotalAmountDecimal,
                sale.Status
            });
        }
    }
}
