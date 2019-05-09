using System.Data.Common;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Options;
using NetCoreBench.Properties;

namespace NetCoreBench.Models
{
    public class DapperDb : IDb
    {
        private readonly string _connectionString;
        private readonly DbProviderFactory _dbProviderFactory;
        private readonly IRandom _random;

        public DapperDb(IRandom random, DbProviderFactory dbProviderFactory, IOptions<AppSettings> appSettings)
        {
            _random = random;
            _dbProviderFactory = dbProviderFactory;
            _connectionString = appSettings.Value.ConnectionString;
        }

        public async Task<World> LoadSingleQueryRow()
        {
            using (var db = _dbProviderFactory.CreateConnection())
            {
                db.ConnectionString = _connectionString;
                return await ReadSingleRow(db);
            }
        }

        public async Task<World[]> LoadMultipleQueriesRows(int count)
        {
            var results = new World[count];
            using (var db = _dbProviderFactory.CreateConnection())
            {
                db.ConnectionString = _connectionString;
                await db.OpenAsync();
                for (var i = 0; i < count; i++) results[i] = await ReadSingleRow(db);
            }

            return results;
        }

        private Task<World> ReadSingleRow(DbConnection db)
        {
            return db.QueryFirstOrDefaultAsync<World>(
                "SELECT id, randomnumber FROM world WHERE id = @Id",
                new {Id = _random.Next(1, 10001)});
        }
    }
}