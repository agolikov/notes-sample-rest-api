using notes.data.Entities;
using notes.data.Interfaces;

namespace notes.data.Repositories
{
    public class Repository : Repository<User>, IUserRepository
    {
        public Repository(AppDbContext appContext) : base(appContext)
        {
        }
    }
}