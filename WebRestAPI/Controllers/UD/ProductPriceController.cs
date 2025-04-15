using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebRest.EF.Data;
using WebRest.EF.Models;

namespace WebRestAPI.Controllers.UD
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductPriceController : ControllerBase, iController<ProductPrice>
    {
        private readonly WebRestOracleContext _context;
        private readonly IMapper _mapper;

        public ProductPriceController(WebRestOracleContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var list = await _context.ProductPrice.ToListAsync();
            return Ok(list);
        }

        [HttpGet("Get/{{ID}}")]
        public async Task<IActionResult> Get(string ID)
        {
            var item = await _context.ProductPrice.FirstOrDefaultAsync(x => x.ProductPriceId == ID);
            return Ok(item);
        }

        [HttpDelete("Delete/{{ID}}")]
        public async Task<IActionResult> Delete(string ID)
        {
            var item = await _context.ProductPrice.FirstOrDefaultAsync(x => x.ProductPriceId == ID);
            if (item != null)
            {
                _context.ProductPrice.Remove(item);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] ProductPrice obj)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existing = await _context.ProductPrice.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ProductPriceId == obj.ProductPriceId);

                if (existing != null)
                {
                    var updated = _mapper.Map<ProductPrice>(obj);
                    _context.ProductPrice.Update(updated);
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
        public async Task<IActionResult> Post([FromBody] ProductPrice obj)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                obj.ProductPriceId = Guid.NewGuid().ToString().ToUpper().Replace("-", "");
                _context.ProductPrice.Add(obj);
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