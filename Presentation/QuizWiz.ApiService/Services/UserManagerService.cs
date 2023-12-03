using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using QuizWiz.Domain.Entities;

namespace QuizWiz.ApiService.Services
{
    public class UserManagerService : UserManager<User>
    {
        public UserManagerService(IUserStore<User> store, IOptions<IdentityOptions> optionsAccessor,
        IPasswordHasher<User> passwordHasher, IEnumerable<IUserValidator<User>> userValidators,
        IEnumerable<IPasswordValidator<User>> passwordValidators, ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<User>> logger)
        : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }

        public override async Task<IdentityResult> CreateAsync(User identityUser, string password)
        {
            var result = await base.CreateAsync(identityUser, password);

            return result;
        }
    }
}
