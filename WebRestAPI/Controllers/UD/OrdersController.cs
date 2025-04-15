using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebRest.EF.Data;
using WebRest.EF.Models;

namespace WebRestAPI.Controllers.UD
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase, iController<Orders>
    {
        private readonly WebRestOracleContext _context;
        private readonly IMapper _mapper;

        public OrdersController(WebRestOracleContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var list = await _context.Orders.ToListAsync();
            return Ok(list);
        }

        [HttpGet("Get/{{ID}}")]
        public async Task<IActionResult> Get(string ID)
        {
            var item = await _context.Orders.FirstOrDefaultAsync(x => x.OrdersId == ID);
            return Ok(item);
        }

        [HttpDelete("Delete/{{ID}}")]
        public async Task<IActionResult> Delete(string ID)
        {
            var item = await _context.Orders.FirstOrDefaultAsync(x => x.OrdersId == ID);
            if (item != null)
            {
                _context.Orders.Remove(item);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Orders obj)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existing = await _context.Orders.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.OrdersId == obj.OrdersId);

                if (existing != null)
                {
                    var updated = _mapper.Map<Orders>(obj);
                    _context.Orders.Update(updated);
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
        public async Task<IActionResult> Post([FromBody] Orders obj)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                obj.OrdersId = Guid.NewGuid().ToString().ToUpper().Replace("-", "");
                _context.Orders.Add(obj);
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