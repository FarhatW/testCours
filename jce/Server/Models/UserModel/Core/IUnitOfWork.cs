using System.Threading.Tasks;

namespace jce.Server.UserModel.Core
{
    
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}