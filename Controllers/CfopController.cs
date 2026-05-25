using GrupoTecnofix_Api.BLL.Interfaces;
using GrupoTecnofix_Api.Dtos.Cfop;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrupoTecnofix_Api.Controllers
{
    [ApiController]
    [Route("api/cfops")]
    [Authorize]
    public class CfopController : ControllerBase
    {
        private readonly ICfopService _service;

        public CfopController(ICfopService service)
        {
            _service = service;
        }

        [Authorize(Policy = "cfop.read")]
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            return Ok(await _service.GetAllAsync(ct));
        }

        [Authorize(Policy = "cfop.read")]
        [HttpGet("lookup")]
        public async Task<IActionResult> GetLookup([FromQuery] string? search = null, CancellationToken ct = default)
        {
            return Ok(await _service.GetListAsync(search, ct));
        }

        [Authorize(Policy = "cfop.read")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            var result = await _service.GetByIdAsync(id, ct);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [Authorize(Policy = "cfop.create")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CfopCreateUpdateDto dto, CancellationToken ct)
        {
            try
            {
                var result = await _service.CreateAsync(dto, ct);
                return CreatedAtAction(nameof(GetById), new { id = result.IdCfop }, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [Authorize(Policy = "cfop.update")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CfopCreateUpdateDto dto, CancellationToken ct)
        {
            try
            {
                var result = await _service.UpdateAsync(id, dto, ct);

                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [Authorize(Policy = "cfop.delete")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var deleted = await _service.DeleteAsync(id, ct);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
