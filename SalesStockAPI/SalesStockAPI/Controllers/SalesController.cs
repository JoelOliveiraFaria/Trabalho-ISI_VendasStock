using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesAPI.Data;
using SalesAPI.Models;

namespace SalesAPI.Controllers
{
    /// <summary>
    /// Controlador API para gestão de vendas.
    /// Fornece endpoints para consultar dados de vendas, estatísticas e análises por produto.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Inicializa uma nova instância do controlador SalesController.
        /// </summary>
        /// <param name="context">Contexto de base de dados injetado via Dependency Injection</param>
        public SalesController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém uma lista de vendas limitada a 100 registos.
        /// </summary>
        /// <returns>Lista de vendas com valores convertidos para decimal</returns>
        /// <response code="200">Retorna a lista de vendas</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<object>>> GetSales()
        {
            var sales = await _context.Sales
                .Take(100)
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

        /// <summary>
        /// Obtém estatísticas agregadas das vendas.
        /// </summary>
        /// <returns>Total de vendas, receita total e ticket médio</returns>
        /// <response code="200">Retorna as estatísticas calculadas</response>
        /// <response code="500">Erro interno do servidor</response>
        [HttpGet("stats")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Obtém os top 10 produtos ordenados por receita.
        /// </summary>
        /// <returns>Lista dos 10 produtos com maior receita, incluindo quantidade e número de vendas</returns>
        /// <response code="200">Retorna a lista dos top produtos</response>
        /// <response code="500">Erro interno do servidor com detalhes</response>
        [HttpGet("by-product")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetTopProductsByRevenue()
        {
            try
            {
                var allSales = await _context.Sales.ToListAsync();

                var topProducts = allSales
                    .GroupBy(s => s.ProductName)
                    .Select(g => new
                    {
                        product = g.Key ?? "Unknown",
                        revenue = g.Sum(s =>
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
                        }),
                        totalQuantity = g.Sum(s => s.Quantity),
                        totalSales = g.Count()
                    })
                    .Where(p => p.revenue > 0)
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

        /// <summary>
        /// Obtém uma venda específica por ID.
        /// </summary>
        /// <param name="id">ID único da venda</param>
        /// <returns>Dados completos da venda</returns>
        /// <response code="200">Retorna a venda encontrada</response>
        /// <response code="404">Venda não encontrada</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
