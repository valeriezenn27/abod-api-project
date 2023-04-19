using abod_api_project.Exceptions;
using abod_api_project.Models;
using abod_api_project.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace abod_api_project.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService productService, ILogger<ProductsController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> Get()
        {
            try
            {
                var products = await _productService.GetAllProducts(true);
                return Ok(products);
            }
            catch (CustomException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all products.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while getting all products.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> Get(int id)
        {
            try
            {
                var product = await _productService.GetProductById(id, true);
                return Ok(product);
            }
            catch (CustomException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting product with id {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting product with id {id}.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Product>> Create(Product product)
        {
            try
            {
                var createdProduct = await _productService.CreateProduct(product);
                return CreatedAtAction(nameof(Get), new { id = createdProduct.Id }, createdProduct);
            }
            catch (CustomException ex)
            {
                _logger.LogError(ex, "Error creating product");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception creating product");
                return StatusCode(500);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Product product)
        {
            try
            {
                var existingProduct = await _productService.GetProductById(id, true);
                if (existingProduct == null)
                {
                    return NotFound();
                }
                product.Id = id;
                await _productService.UpdateProduct(id, product);
                return NoContent();
            }
            catch (CustomException ex)
            {
                _logger.LogError(ex, "Error updating product");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception updating product");
                return StatusCode(500);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _productService.DeleteProduct(id);
                return NoContent();
            }
            catch (CustomException ex)
            {
                _logger.LogError(ex, "Error deleting product");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception deleting product");
                return StatusCode(500);
            }
        }
    }
}
