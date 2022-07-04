using graphql_auth.Models;
using MongoDB.Driver;

namespace graphql_auth.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly IMongoCollection<UserRole> _userRole;

        public UserRoleRepository(IMongoClient client)
        {
            var database = client.GetDatabase("NFTDB");
            var collection = database.GetCollection<UserRole>(nameof(UserRole));
            _userRole = collection;
        }

        public IList<UserRole> GetRoleById(Guid id)
        {
            return _userRole.Find(_ => _.UserId == id).ToList();
        }
    }
}
