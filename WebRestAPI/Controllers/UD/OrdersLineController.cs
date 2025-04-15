using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebRest.EF.Data;
using WebRest.EF.Models;

namespace WebRestAPI.Controllers.UD
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersLineController : ControllerBase, iController<OrdersLine>
    {
        private readonly WebRestOracleContext _context;
        private readonly IMapper _mapper;

        public OrdersLineController(WebRestOracleContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var list = await _context.OrdersLine.ToListAsync();
            return Ok(list);
        }

        [HttpGet("Get/{{ID}}")]
        public async Task<IActionResult> Get(string ID)
        {
            var item = await _context.OrdersLine.FirstOrDefaultAsync(x => x.OrdersLineId == ID);
            return Ok(item);
        }

        [HttpDelete("Delete/{{ID}}")]
        public async Task<IActionResult> Delete(string ID)
        {
            var item = await _context.OrdersLine.FirstOrDefaultAsync(x => x.OrdersLineId == ID);
            if (item != null)
            {
                _context.OrdersLine.Remove(item);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] OrdersLine obj)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existing = await _context.OrdersLine.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.OrdersLineId == obj.OrdersLineId);

                if (existing != null)
                {
                    var updated = _mapper.Map<OrdersLine>(obj);
                    _context.OrdersLine.Update(updated);
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
        public async Task<IActionResult> Post([FromBody] OrdersLine obj)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                obj.OrdersLineId = Guid.NewGuid().ToString().ToUpper().Replace("-", "");
                _context.OrdersLine.Add(obj);
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