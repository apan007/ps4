using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebRest.EF.Data;
using WebRest.EF.Models;

namespace WebRestAPI.Controllers.UD
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductStatusController : ControllerBase, iController<ProductStatus>
    {
        private readonly WebRestOracleContext _context;
        private readonly IMapper _mapper;

        public ProductStatusController(WebRestOracleContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var list = await _context.ProductStatus.ToListAsync();
            return Ok(list);
        }

        [HttpGet("Get/{{ID}}")]
        public async Task<IActionResult> Get(string ID)
        {
            var item = await _context.ProductStatus.FirstOrDefaultAsync(x => x.ProductStatusId == ID);
            return Ok(item);
        }

        [HttpDelete("Delete/{{ID}}")]
        public async Task<IActionResult> Delete(string ID)
        {
            var item = await _context.ProductStatus.FirstOrDefaultAsync(x => x.ProductStatusId == ID);
            if (item != null)
            {
                _context.ProductStatus.Remove(item);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ProductStatus obj)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existing = await _context.ProductStatus.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ProductStatusId == obj.ProductStatusId);

                if (existing != null)
                {
                    var updated = _mapper.Map<ProductStatus>(obj);
                    _context.ProductStatus.Update(updated);
                    await _context.SaveChangesAsync();
                    trans.Commit();
                }
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductStatus obj)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                obj.ProductStatusId = Guid.NewGuid().ToString().ToUpper().Replace("-", "");
                _context.ProductStatus.Add(obj);
                await _context.SaveChangesAsync();
                trans.Commit();
            }
            catch (Exception ex)
            {
                trans.Rollback();
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return Ok();
        }
    }
}