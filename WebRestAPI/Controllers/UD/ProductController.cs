using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebRest.EF.Data;
using WebRest.EF.Models;

namespace WebRestAPI.Controllers.UD
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase, iController<Product>
    {
        private readonly WebRestOracleContext _context;
        private readonly IMapper _mapper;

        public ProductController(WebRestOracleContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var list = await _context.Product.ToListAsync();
            return Ok(list);
        }

        [HttpGet("Get/{{ID}}")]
        public async Task<IActionResult> Get(string ID)
        {
            var item = await _context.Product.FirstOrDefaultAsync(x => x.ProductId == ID);
            return Ok(item);
        }

        [HttpDelete("Delete/{{ID}}")]
        public async Task<IActionResult> Delete(string ID)
        {
            var item = await _context.Product.FirstOrDefaultAsync(x => x.ProductId == ID);
            if (item != null)
            {
                _context.Product.Remove(item);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Product obj)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existing = await _context.Product.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.ProductId == obj.ProductId);

                if (existing != null)
                {
                    var updated = _mapper.Map<Product>(obj);
                    _context.Product.Update(updated);
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
        public async Task<IActionResult> Post([FromBody] Product obj)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                obj.ProductId = Guid.NewGuid().ToString().ToUpper().Replace("-", "");
                _context.Product.Add(obj);
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