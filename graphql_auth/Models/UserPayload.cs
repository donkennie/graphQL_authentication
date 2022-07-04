namespace graphql_auth.Models
{
  
        public record CreateUserPayload(User user);
        public record DeleteUserPayload(bool isSuccessfull);
        public record UpdateUserPayload(bool isSuccessfull);
        public record UserTokenPayload(string Message, string AccessToken, string RefreshToken);
   
}
