using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebRest.EF.Data;
using WebRest.EF.Models;

namespace WebRestAPI.Controllers.UD
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderStatusController : ControllerBase, iController<OrderStatus>
    {
        private readonly WebRestOracleContext _context;
        private readonly IMapper _mapper;

        public OrderStatusController(WebRestOracleContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var list = await _context.OrderStatus.ToListAsync();
            return Ok(list);
        }

        [HttpGet("Get/{{ID}}")]
        public async Task<IActionResult> Get(string ID)
        {
            var item = await _context.OrderStatus.FirstOrDefaultAsync(x => x.OrderStatusId == ID);
            return Ok(item);
        }

        [HttpDelete("Delete/{{ID}}")]
        public async Task<IActionResult> Delete(string ID)
        {
            var item = await _context.OrderStatus.FirstOrDefaultAsync(x => x.OrderStatusId == ID);
            if (item != null)
            {
                _context.OrderStatus.Remove(item);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] OrderStatus obj)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existing = await _context.OrderStatus.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.OrderStatusId == obj.OrderStatusId);

                if (existing != null)
                {
                    var updated = _mapper.Map<OrderStatus>(obj);
                    _context.OrderStatus.Update(updated);
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
        public async Task<IActionResult> Post([FromBody] OrderStatus obj)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                obj.OrderStatusId = Guid.NewGuid().ToString().ToUpper().Replace("-", "");
                _context.OrderStatus.Add(obj);
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