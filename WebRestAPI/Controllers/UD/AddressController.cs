using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebRest.EF.Data;
using WebRest.EF.Models;

namespace WebRestAPI.Controllers.UD
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase, iController<Address>
    {
        private readonly WebRestOracleContext _context;
        private readonly IMapper _mapper;

        public AddressController(WebRestOracleContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var list = await _context.Address.ToListAsync();
            return Ok(list);
        }

        [HttpGet("Get/{{ID}}")]
        public async Task<IActionResult> Get(string ID)
        {
            var item = await _context.Address.FirstOrDefaultAsync(x => x.AddressId == ID);
            return Ok(item);
        }

        [HttpDelete("Delete/{{ID}}")]
        public async Task<IActionResult> Delete(string ID)
        {
            var item = await _context.Address.FirstOrDefaultAsync(x => x.AddressId == ID);
            if (item != null)
            {
                _context.Address.Remove(item);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Address obj)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existing = await _context.Address.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.AddressId == obj.AddressId);

                if (existing != null)
                {
                    var updated = _mapper.Map<Address>(obj);
                    _context.Address.Update(updated);
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
        public async Task<IActionResult> Post([FromBody] Address obj)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                obj.AddressId = Guid.NewGuid().ToString().ToUpper().Replace("-", "");
                _context.Address.Add(obj);
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