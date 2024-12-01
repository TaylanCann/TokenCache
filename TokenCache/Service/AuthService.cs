using Microsoft.Extensions.Options;
using MongoDB.Driver;
using TokenCache.Interfaces;
using TokenCache.Models;

namespace TokenCache.Service
{
    public class AuthService : IAuthService
    {
        readonly ITokenService _tokenService;
        readonly MongoClient _client;
        readonly IMongoDatabase _database;
        private readonly IMongoCollection<User> _users;



        public AuthService(ITokenService tokenService, IOptions<MongoDBSettings>  mongoDBSettings)
        {
            _tokenService = tokenService;
            _client = new MongoClient(mongoDBSettings.Value.ConnectionString);
            _database = _client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _users = _database.GetCollection<User>(mongoDBSettings.Value.CollectionName);
        }

        public async Task<UserLoginResponse> LoginUserAsync(User request)
        {
            UserLoginResponse response = new ();

            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                throw new ArgumentNullException(nameof(request));
            }
            
            var generatedTokenInformation = await _tokenService.GenerateTokenAsync(new GenerateTokenRequest { Username = request.Username });

            response.AuthenticateResult = true;
            response.AuthToken = generatedTokenInformation.Token;
            response.AccessTokenExpireDate = generatedTokenInformation.TokenExpireDate;

            var saltedHash = request.Password;
            
            _users.InsertOne(request);

            return response;
        }
    }
}
