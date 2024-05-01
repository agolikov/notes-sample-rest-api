using notes.data.Entities;
using notes.data.Interfaces;

namespace notes.data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext appContext) : base(appContext)
        {
        }
    }
}