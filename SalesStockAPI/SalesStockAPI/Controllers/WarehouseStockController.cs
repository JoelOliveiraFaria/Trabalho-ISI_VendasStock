using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesAPI.Data;
using SalesAPI.Models;

namespace SalesAPI.Controllers
{
    /// <summary>
    /// Controlador API para gestão de inventário de armazém.
    /// Fornece endpoints para consultar stock, identificar produtos com baixo inventário e obter estatísticas.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseStockController : ControllerBase
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Inicializa uma nova instância do controlador WarehouseStockController.
        /// </summary>
        /// <param name="context">Contexto de base de dados injetado via Dependency Injection</param>
        public WarehouseStockController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém a lista completa de stock de todos os armazéns.
        /// </summary>
        /// <returns>Lista de todos os produtos em stock com informações de status</returns>
        /// <response code="200">Retorna a lista completa de stock</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
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

        /// <summary>
        /// Obtém o stock de um armazém específico.
        /// </summary>
        /// <param name="warehouseId">ID único do armazém</param>
        /// <returns>Lista de produtos no armazém especificado</returns>
        /// <response code="200">Retorna o stock do armazém</response>
        /// <response code="404">Armazém não encontrado</response>
        [HttpGet("{warehouseId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Obtém lista de produtos com stock abaixo do nível mínimo.
        /// </summary>
        /// <returns>Lista de produtos com baixo stock, ordenados por deficit de quantidade</returns>
        /// <response code="200">Retorna produtos com stock baixo</response>
        [HttpGet("low-stock")]
        [ProducesResponseType(StatusCodes.Status200OK)]
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

        /// <summary>
        /// Obtém lista de produtos esgotados (quantidade zero).
        /// </summary>
        /// <returns>Lista de produtos sem stock disponível</returns>
        /// <response code="200">Retorna produtos esgotados</response>
        [HttpGet("out-of-stock")]
        [ProducesResponseType(StatusCodes.Status200OK)]
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

        /// <summary>
        /// Obtém o stock de um produto específico em todos os armazéns.
        /// </summary>
        /// <param name="sku">SKU (código) do produto</param>
        /// <returns>Quantidade total e distribuição do produto por armazém</returns>
        /// <response code="200">Retorna informações de stock do produto</response>
        /// <response code="404">Produto não encontrado em nenhum armazém</response>
        [HttpGet("by-product/{sku}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Obtém estatísticas gerais do inventário de todos os armazéns.
        /// </summary>
        /// <returns>Métricas agregadas incluindo total de produtos, quantidades e contadores de alertas</returns>
        /// <response code="200">Retorna estatísticas do inventário</response>
        [HttpGet("stats")]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
