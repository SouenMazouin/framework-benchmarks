using System.Threading.Tasks;

namespace NetCoreBench.Models
{
    public interface IDb
    {
        Task<World> LoadSingleQueryRow();
        Task<World[]> LoadMultipleQueriesRows(int count);
    }
}