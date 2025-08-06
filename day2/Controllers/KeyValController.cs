using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Npgsql;
using System.Collections;
using System.Net.Mime;
using System.Threading.Tasks;

namespace day2.Controllers
{
    [ApiController]
    [Route("[controller]s")]
    public class KeyValController : ControllerBase
    {
        private readonly ILogger<KeyValController> _logger;
        private KeyValContext context;
        //private static DbContextOptions<KeyValContext> options = DbContextOptionsBuilder<KeyValContext>.UseNpgsql("Host=127.0.0.1;Username=postgres;Password=abcd1234;Database=test")
        //private static PooledDbContextFactory<KeyValContext> dbCtxFactory = new(options: new DbContextOptions<KeyValContext>());
        public KeyValController(ILogger<KeyValController> logger) {
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

        [HttpGet("[controller]s")]
        public IAsyncEnumerable<KeyVal> Get()
        {
            var keyVals = context.KeyVals.AsAsyncEnumerable();
            return keyVals;
        }

        [HttpGet("[controller]")]
        public async Task<KeyVal> Get(int id)
        {
            var res = await context.KeyVals.FindAsync(id);
            if (res == null)
            {
                throw new Exception("sadly it does not exist");
            }
            return res;
        }

        [HttpPost("[controller]")]
        public async Task<int> Post(string val)
        {
            KeyVal keyVal = new() { Val = val };
            context.Add(keyVal);
            await context.SaveChangesAsync(); 
            return keyVal.Id;
        }

        [HttpDelete("[controller]")]
        public async Task<bool> Delete(int id)
        {
            try
            {
                KeyVal keyVal = new () { Id = id };
                var res = context.KeyVals.Remove(keyVal);
                await context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        [HttpPut("[controller]")]
        public async Task<bool> Put(KeyVal keyVal)
        {
            try
            {
                context.Update(keyVal);
                await context.SaveChangesAsync();
            } catch (Exception) {
                return false;
            }
            return true;
        }

        [HttpGet("test")]
        public IEnumerable<KeyVal> Test()
        {
            var x = from keyVal in this.context.KeyVals where keyVal.Val == "test" select keyVal;
            return x;
        }
    }
}
