using MongoDB.Driver;

namespace graphql_auth.Models
{
    public class UserNodeResolver
    {
        public Task<User> ResolveAsync([Service] IMongoCollection<User> collection, Guid id)
        {
            return collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}
