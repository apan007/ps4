using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebRest.EF.Data;
using WebRest.EF.Models;

namespace WebRestAPI.Controllers.UD
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerAddressController : ControllerBase, iController<CustomerAddress>
    {
        private readonly WebRestOracleContext _context;
        private readonly IMapper _mapper;

        public CustomerAddressController(WebRestOracleContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var list = await _context.CustomerAddress.ToListAsync();
            return Ok(list);
        }

        [HttpGet("Get/{{ID}}")]
        public async Task<IActionResult> Get(string ID)
        {
            var item = await _context.CustomerAddress.FirstOrDefaultAsync(x => x.CustomerAddressId == ID);
            return Ok(item);
        }

        [HttpDelete("Delete/{{ID}}")]
        public async Task<IActionResult> Delete(string ID)
        {
            var item = await _context.CustomerAddress.FirstOrDefaultAsync(x => x.CustomerAddressId == ID);
            if (item != null)
            {
                _context.CustomerAddress.Remove(item);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] CustomerAddress obj)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existing = await _context.CustomerAddress.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.CustomerAddressId == obj.CustomerAddressId);

                if (existing != null)
                {
                    var updated = _mapper.Map<CustomerAddress>(obj);
                    _context.CustomerAddress.Update(updated);
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
        public async Task<IActionResult> Post([FromBody] CustomerAddress obj)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                obj.CustomerAddressId = Guid.NewGuid().ToString().ToUpper().Replace("-", "");
                _context.CustomerAddress.Add(obj);
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