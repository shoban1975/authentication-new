using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using pulsenet.api.models;
using pulsenet.api.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pulsenet.api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/comparisons")]
    public class ComparisonController : ControllerBase
    {
        private readonly ILogger<ComparisonController> _logger;
        private readonly IComparisonService _comparisonService;

        public ComparisonController(ILogger<ComparisonController> logger, IComparisonService comparisonService)
        {
            _logger = logger;
            _comparisonService = comparisonService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<List<Comparison>>> GetAllComparisons()
        {
            try
            {
                List<Comparison> comparisons = await _comparisonService.GetAllComparisonsAsync();
                return Ok(comparisons);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting comparisons: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Comparison>> GetComparisonById(int id)
        {
            try
            {
                Comparison? comparison = await _comparisonService.GetComparisonByIdAsync(id);

                if (comparison == null)
                {
                    return NotFound();
                }

                return Ok(comparison);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting comparison with id {id}: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<Comparison>> InsertComparison([FromBody] ComparisonDTO comparisonDTO)
        {
            try
            {
                bool result = await _comparisonService.InsertComparisonsAsync(comparisonDTO);

                if (!result)
                {
                    return BadRequest();
                }

                Comparison? comparison = await _comparisonService.GetComparisonByNameAsync(comparisonDTO.name);

                if (comparison == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

                return CreatedAtAction(nameof(GetComparisonById), new { id = comparison.Id }, comparison);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error inserting comparison: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> UpdateComparison(int id, [FromBody] ComparisonDTO comparisonDTO)
        {
            try
            {
                bool result = await _comparisonService.UpdateComparisonsAsync(id, comparisonDTO);

                if (!result)
                {
                    return BadRequest();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating comparison with id {id}: {ex.Message}");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

public async Task<ActionResult> DeleteComparison(int id)
{
    try
    {
        bool success = await _comparisonService.DeleteComparisonsAsync(id);

        if (!success)
        {
            return NotFound($"Comparison with id {id} not found.");
        }

        return NoContent();
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, $"Error deleting comparison with id {id}");
        return StatusCode(500, "An error occurred while deleting the comparison.");
    }
}

           
