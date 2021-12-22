using System.Threading.Tasks;

namespace challenge.Repositories
{
    public interface IRepository
    {
        Task SaveAsync();
    }
}