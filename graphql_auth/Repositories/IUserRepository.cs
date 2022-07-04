using graphql_auth.Models;

namespace graphql_auth.Repositories
{
    public interface IUserRepository
    {
        IExecutable<User> GetUser();
        IExecutable<User> GetUserById([ID] Guid id);
        User CreateUser(CreateUserInput createUserInput);
        bool DeleteUser(Guid id);
        bool UpdateUser(Guid id, UpdateUserInput updateUserInput);
        UserTokenPayload Login(LoginInput loginInput);
        UserTokenPayload RenewAccessToken(RenewTokenInput renewTokenInput);
    }
}
