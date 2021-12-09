using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pure.Application.Commons;
using Pure.Application.Dto;
using Pure.Application.Exceptions;

namespace Pure.WebApi.Controllers
{
    /// <summary>
    /// Controller API to manage products.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductHandler _productHandler;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="productHandler"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ProductController(ILogger<ProductController> logger, IProductHandler productHandler)
        {
            _logger = Guard.Against.Null(logger, nameof(logger));
            _productHandler = Guard.Against.Null(productHandler, nameof(productHandler));
        }

        /// <summary>
        /// Get all products.
        /// </summary>
        /// <returns>A list of <see cref="ProductDto"/></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductDto>))]
        public async Task<IActionResult> Get()
        {
            return Ok(await _productHandler.GetAllProductsAsync());
        }

        /// <summary>
        /// Get a product by ID.
        /// </summary>
        /// <param name="id">The Guid of product.</param>
        /// <returns>The specified product.</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            ProductDto product;

            try
            {
                product = await _productHandler.GetProductAsync(id);
            }
            catch (EntityNotFoundException e)
            {
                _logger.LogTrace(e.Message);
                return NotFound();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }

            return Ok(product);
        }

        /// <summary>
        /// Create a product.
        /// </summary>
        /// <param name="productDto">The product to add.</param>
        /// <returns>The route and the Guid of created product.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] ProductDto productDto)
        {
            Guid idProduct;

            try
            {
                idProduct = await _productHandler.CreateProductAsync(productDto);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Errors);
            }

            _logger.LogWarning($"The product '{productDto.Name}' has been created with ID:{idProduct.ToString()}.");
            return CreatedAtAction("Get", idProduct.ToString());
        }

        /// <summary>
        /// Update a product.
        /// </summary>
        /// <param name="id">The Guid of product.</param>
        /// <param name="productDto">The product to update.</param>
        /// <returns>The product with data updated.</returns>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] ProductDto productDto)
        {
            ProductDto updatedProduct;

            try
            {
                updatedProduct = await _productHandler.UpdateProductAsync(id, productDto);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }

            _logger.LogWarning($"The product with ID:'{productDto.Id}' has been updated.");
            return Ok(updatedProduct);
        }

        /// <summary>
        /// Delete a product.
        /// </summary>
        /// <param name="id">The Guid of product.</param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                await _productHandler.DeleteProductAsync(id);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }

            _logger.LogWarning($"The product '{id}' has been removed.");
            return NoContent();
        }
    }
}