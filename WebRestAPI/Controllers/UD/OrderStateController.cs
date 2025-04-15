using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebRest.EF.Data;
using WebRest.EF.Models;

namespace WebRestAPI.Controllers.UD
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderStateController : ControllerBase, iController<OrderState>
    {
        private readonly WebRestOracleContext _context;
        private readonly IMapper _mapper;

        public OrderStateController(WebRestOracleContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var list = await _context.OrderState.ToListAsync();
            return Ok(list);
        }

        [HttpGet("Get/{{ID}}")]
        public async Task<IActionResult> Get(string ID)
        {
            var item = await _context.OrderState.FirstOrDefaultAsync(x => x.OrderStateId == ID);
            return Ok(item);
        }

        [HttpDelete("Delete/{{ID}}")]
        public async Task<IActionResult> Delete(string ID)
        {
            var item = await _context.OrderState.FirstOrDefaultAsync(x => x.OrderStateId == ID);
            if (item != null)
            {
                _context.OrderState.Remove(item);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] OrderState obj)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existing = await _context.OrderState.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.OrderStateId == obj.OrderStateId);

                if (existing != null)
                {
                    var updated = _mapper.Map<OrderState>(obj);
                    _context.OrderState.Update(updated);
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
        public async Task<IActionResult> Post([FromBody] OrderState obj)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                obj.OrderStateId = Guid.NewGuid().ToString().ToUpper().Replace("-", "");
                _context.OrderState.Add(obj);
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