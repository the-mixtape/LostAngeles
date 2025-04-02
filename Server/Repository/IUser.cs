using System.Threading.Tasks;
using LostAngeles.Server.Domain;

namespace LostAngeles.Server.Repository
{
    public interface IUser
    {
        Task<User> GetOrCreate(string license);
    }
}