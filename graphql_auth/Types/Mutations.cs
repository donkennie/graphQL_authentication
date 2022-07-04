using graphql_auth.Models;
using graphql_auth.Repositories;
using HotChocolate.AspNetCore.Authorization;
using HotChocolate.Subscriptions;

namespace graphql_auth.Types
{
    public class Mutations
    {
        [Authorize]
        public async Task<CreateUserPayload> CreateUser([Service] IUserRepository userRepository, [Service] ITopicEventSender eventSender, CreateUserInput createUserInput)
        {
            try
            {
                var item = userRepository.CreateUser(createUserInput);
                await eventSender.SendAsync(nameof(Subscription.SubscribeUser), item);

                return new CreateUserPayload(item);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DeleteUserPayload DeleteUser([Service] IUserRepository userRepository, [ID] Guid id)
        {
            var item = userRepository.DeleteUser(id);
            return new DeleteUserPayload(item);
        }
        public UpdateUserPayload UpdateUser([Service] IUserRepository userRepository, [ID] Guid id, UpdateUserInput updateUserInput)
        {
            var item = userRepository.UpdateUser(id, updateUserInput);
            return new UpdateUserPayload(item);
        }

        public UserTokenPayload Login([Service] IUserRepository userRepository, LoginInput loginInput)
        {
            return userRepository.Login(loginInput);
        }
        public UserTokenPayload RenewAccessToken([Service] IUserRepository userRepository, RenewTokenInput renewTokenInput)
        {
            return userRepository.RenewAccessToken(renewTokenInput);
        }
    }
}
