using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebRest.EF.Data;
using WebRest.EF.Models;

namespace WebRestAPI.Controllers.UD
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenderController : ControllerBase, iController<Gender>
    {
        private readonly WebRestOracleContext _context;
        private readonly IMapper _mapper;

        public GenderController(WebRestOracleContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var list = await _context.Gender.ToListAsync();
            return Ok(list);
        }

        [HttpGet("Get/{{ID}}")]
        public async Task<IActionResult> Get(string ID)
        {
            var item = await _context.Gender.FirstOrDefaultAsync(x => x.GenderId == ID);
            return Ok(item);
        }

        [HttpDelete("Delete/{{ID}}")]
        public async Task<IActionResult> Delete(string ID)
        {
            var item = await _context.Gender.FirstOrDefaultAsync(x => x.GenderId == ID);
            if (item != null)
            {
                _context.Gender.Remove(item);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Gender obj)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                var existing = await _context.Gender.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.GenderId == obj.GenderId);

                if (existing != null)
                {
                    var updated = _mapper.Map<Gender>(obj);
                    _context.Gender.Update(updated);
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
        public async Task<IActionResult> Post([FromBody] Gender obj)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                obj.GenderId = Guid.NewGuid().ToString().ToUpper().Replace("-", "");
                _context.Gender.Add(obj);
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