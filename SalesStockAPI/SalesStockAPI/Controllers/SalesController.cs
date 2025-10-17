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
            try
            {
                var allSales = await _context.Sales.ToListAsync();

                var totalSales = allSales.Count;

                var totalRevenue = allSales.Sum(s =>
                {
                    try
                    {
                        var priceStr = (s.UnitPrice ?? "0")
                            .Replace("$", "")
                            .Replace("€", "")
                            .Replace(",", "")
                            .Trim();

                        var price = decimal.TryParse(priceStr, out var p) ? p : 0m;

                        return price * s.Quantity;
                    }
                    catch
                    {
                        return 0m;
                    }
                });

                var avgTicket = totalSales > 0 ? totalRevenue / totalSales : 0;

                return Ok(new
                {
                    totalSales,
                    totalRevenue,
                    avgTicket
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // GET: api/sales/by-product
        [HttpGet("by-product")]
        public async Task<ActionResult> GetTopProductsByRevenue()
        {
            try
            {
                // Pega TODOS os dados
                var allSales = await _context.Sales.ToListAsync();

                // Agrupa e calcula receita EM MEMÓRIA
                var topProducts = allSales
                    .GroupBy(s => s.ProductName)
                    .Select(g => new
                    {
                        product = g.Key ?? "Unknown",
                        revenue = g.Sum(s =>
                        {
                            try
                            {
                                // Só precisa converter o UnitPrice (Quantity já é int!)
                                var priceStr = (s.UnitPrice ?? "0")
                                    .Replace("$", "")
                                    .Replace("€", "")
                                    .Replace(",", "")
                                    .Trim();

                                var price = decimal.TryParse(priceStr, out var p) ? p : 0m;

                                return price * s.Quantity;  // Quantity já é int!
                            }
                            catch
                            {
                                return 0m;
                            }
                        }),
                        totalQuantity = g.Sum(s => s.Quantity),  // Direto!
                        totalSales = g.Count()
                    })
                    .Where(p => p.revenue > 0)  // Só produtos com receita > 0
                    .OrderByDescending(p => p.revenue)
                    .Take(10)
                    .ToList();

                return Ok(topProducts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = ex.Message,
                    stackTrace = ex.StackTrace,
                    innerException = ex.InnerException?.Message
                });
            }
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
