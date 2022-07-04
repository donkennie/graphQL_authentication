using graphql_auth.Models;

namespace graphql_auth.Types
{
    public class Subscription
    {

        [Topic]
        [Subscribe]
        public User SubscribeUser([EventMessage] User user)
        {
            return user;
        }
    }
}
