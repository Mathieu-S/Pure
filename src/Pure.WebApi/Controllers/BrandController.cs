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
    /// Controller API to manage brands.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class BrandController : ControllerBase
    {
        private readonly ILogger<BrandController> _logger;
        private readonly IBrandHandler _brandHandler;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="brandHandler"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public BrandController(ILogger<BrandController> logger, IBrandHandler brandHandler)
        {
            _logger = Guard.Against.Null(logger, nameof(logger));
            _brandHandler = Guard.Against.Null(brandHandler, nameof(brandHandler));
        }
        
        /// <summary>
        /// Get all brands.
        /// </summary>
        /// <returns>A list of <see cref="BrandDto"/></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BrandDto>))]
        public async Task<IActionResult> Get()
        {
            return Ok(await _brandHandler.GetAllBrandsAsync());
        }

        /// <summary>
        /// Get a brand by ID.
        /// </summary>
        /// <param name="id">The Guid of brand.</param>
        /// <returns>The specified brand.</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BrandDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            BrandDto brand;

            try
            {
                brand = await _brandHandler.GetBrandAsync(id);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (EntityNotFoundException e)
            {
                _logger.LogTrace(e.Message);
                return NotFound();
            }

            return Ok(brand);
        }

        /// <summary>
        /// Create a brand.
        /// </summary>
        /// <param name="brandDto">The brand to add.</param>
        /// <returns>The route and the Guid of created brand.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] BrandDto brandDto)
        {
            Guid idBrand;

            try
            {
                idBrand = await _brandHandler.CreateBrandAsync(brandDto);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            catch (ValidationException e)
            {
                return BadRequest(e.Errors);
            }

            _logger.LogInformation($"The brand '{brandDto.Name}' has been created with ID:{idBrand.ToString()}.");
            return CreatedAtAction("Get", idBrand.ToString());
        }

        /// <summary>
        /// Update a brand.
        /// </summary>
        /// <param name="id">The Guid of brand.</param>
        /// <param name="brandDto">The brand to update.</param>
        /// <returns>The brand with data updated.</returns>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] BrandDto brandDto)
        {
            BrandDto updatedBrand;

            try
            {
                updatedBrand = await _brandHandler.UpdateBrandAsync(id, brandDto);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }

            _logger.LogInformation($"The brand with ID:'{updatedBrand.Id}' has been updated.");
            return Ok(updatedBrand);
        }

        /// <summary>
        /// Delete a brand.
        /// </summary>
        /// <param name="id">The Guid of brand.</param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                await _brandHandler.DeleteBrandAsync(id);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }
            
            _logger.LogInformation($"The brand '{id}' has been removed.");
            return NoContent();
        }
    }
}