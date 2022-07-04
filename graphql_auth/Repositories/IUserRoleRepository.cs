using graphql_auth.Models;

namespace graphql_auth.Repositories
{
    public interface IUserRoleRepository
    {
        IList<UserRole> GetRoleById(Guid id);
    }
}
