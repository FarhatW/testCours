using System.Threading.Tasks;
using jce.Server.Persistence;

namespace jce.Server.UserModel.Core
{
     public class UnitOfWork : IUnitOfWork
    {
        private readonly JceDbContext _context;
        public UnitOfWork(JceDbContext context)
        {
            this._context = context;

        }
        public async Task CompleteAsync()
        {
           await _context.SaveChangesAsync();
        }
    }
}