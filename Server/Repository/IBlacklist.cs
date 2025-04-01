using System.Threading.Tasks;
using LostAngeles.Server.Domain;

namespace LostAngeles.Server.Repository
{
    public interface IBlacklist
    {
        Task<Blacklist> GetByLicenseAsync(string license);
        Task AddAsync(Blacklist blacklist);
        Task UpdateAsync(Blacklist blacklist);
        Task DeleteAsync(string license);
    }
}