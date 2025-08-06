using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Npgsql;
using System.Collections;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace day2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KeyValsController : ControllerBase
    {
        private readonly ILogger<KeyValsController> _logger;
        private KeyValContext context;
        //private static DbContextOptions<KeyValContext> options = DbContextOptionsBuilder<KeyValContext>.UseNpgsql("Host=127.0.0.1;Username=postgres;Password=abcd1234;Database=test")
        //private static PooledDbContextFactory<KeyValContext> dbCtxFactory = new(options: new DbContextOptions<KeyValContext>());
        public KeyValsController (ILogger<KeyValsController> logger) {
            this._logger = logger;
            this.context = new KeyValContext();
            //this.context = dbCtxFactory.CreateDbContext();
            if (context.ConnTest())
            {
                Console.WriteLine("Connected Successfully");
            } else {
                throw new Exception("Failed to Connect to DB");
            }
        }

        [HttpGet]
        public IAsyncEnumerable<KeyVal> Get()
        {
            var keyVals = context.KeyVals.AsAsyncEnumerable();
            return keyVals;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<KeyVal>> Get(int id)
        {
            var res = await context.KeyVals.FindAsync(id);
            if (res == null)
            {
                return NotFound();
                //throw new Exception("sadly it does not exist");
            }
            return res;
        }

        [HttpPost]
        public async Task<int> Post(KeyVal_ValOnly val)
        {
            KeyVal keyVal = new() { Val = val.Val };
            context.Add(keyVal);
            await context.SaveChangesAsync(); 
            return keyVal.Id; // TODO: return Created() status code too
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                KeyVal keyVal = new () { Id = id };
                var res = context.KeyVals.Remove(keyVal);
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, KeyVal_ValOnly val)
        {
            KeyVal keyVal = new KeyVal { Id = id, Val = val.Val };
            try
            {
                context.Update(keyVal);
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return BadRequest(); // TODO: use more appropriate status code
            }
            return Ok();
        }

        [HttpGet("test")]
        public IEnumerable<KeyVal> Test()
        {
            var x = from keyVal in this.context.KeyVals where keyVal.Val == "test" select keyVal;
            return x;
        }
    }
}
