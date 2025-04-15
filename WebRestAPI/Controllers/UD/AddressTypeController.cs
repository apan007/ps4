using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebRest.EF.Data;
using WebRest.EF.Models;

namespace WebRestAPI.Controllers.UD
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressTypeController : ControllerBase, iController<AddressType>
    {
        private readonly WebRestOracleContext _context;
        private readonly IMapper _mapper;

        public AddressTypeController(WebRestOracleContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var list = await _context.AddressType.ToListAsync();
            return Ok(list);
        }

        [HttpGet("Get/{{ID}}")]
        public async Task<IActionResult> Get(string ID)
        {
            var item = await _context.AddressType.FirstOrDefaultAsync(x => x.AddressTypeId == ID);
            return Ok(item);
        }

        [HttpDelete("Delete/{{ID}}")]
        public async Task<IActionResult> Delete(string ID)
        {
            var item = await _context.AddressType.FirstOrDefaultAsync(x => x.AddressTypeId == ID);
            if (item != null)
            {
                _context.AddressType.Remove(item);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] AddressType obj)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existing = await _context.AddressType.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.AddressTypeId == obj.AddressTypeId);

                if (existing != null)
                {
                    var updated = _mapper.Map<AddressType>(obj);
                    _context.AddressType.Update(updated);
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
        public async Task<IActionResult> Post([FromBody] AddressType obj)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                obj.AddressTypeId = Guid.NewGuid().ToString().ToUpper().Replace("-", "");
                _context.AddressType.Add(obj);
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