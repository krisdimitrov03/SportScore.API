using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using SportScore.API.DataTransferModels;
using SportScore.API.SportsDataOperators.Contracts;
using SportScore.Infrastructure.Data;
using SportScore.Infrastructure.Data.Repositories;

namespace SportScore.API.SportsDataOperators.Services
{
    public class UserService : IUserService
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserStore<ApplicationUser> userStore;
        private readonly IUserEmailStore<ApplicationUser> emailStore;
        private readonly IApplicationDbRepository repo;

        public UserService(
            SignInManager<ApplicationUser> _signInManager,
            UserManager<ApplicationUser> _userManager,
            IUserStore<ApplicationUser> _userStore,
            IApplicationDbRepository _repo)
        {
            signInManager = _signInManager;
            userManager = _userManager;
            userStore = _userStore;
            emailStore = GetEmailStore();
            repo = _repo;
        }

        public async Task<AuthReturnDTO?> LogUserIn(Stream body)
        {
            var user = await FormatInfo<LoginDTO>(body);

            var result = await signInManager.PasswordSignInAsync(user.Email, user.Password, user.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return new AuthReturnDTO()
                {
                    Id = repo
                    .All<ApplicationUser>()
                    .FirstOrDefault(u => u.UserName == user.Email).Id,

                    Username = user.Email
                };
            }
            else return null;
        }

        public async Task<AuthReturnDTO?> RegisterUser(Stream body)
        {
            var data = await FormatInfo<RegisterDTO>(body);

            var user = Activator.CreateInstance<ApplicationUser>();

            await userStore.SetUserNameAsync(user, data.Email, CancellationToken.None);
            await emailStore.SetEmailAsync(user, data.Email, CancellationToken.None);

            var result = await userManager.CreateAsync(user, data.Password);

            if (result.Succeeded) return new AuthReturnDTO() { Id = user.Id, Username = user.UserName };
            else return null;
        }

        public async Task<UserDetailsDTO> GetUserDetails(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if(user == null) return null;

            return new UserDetailsDTO()
            {
                Id = user.Id,
                Email = user.Email,
                Username = user.UserName,
                ProfilePicture = "default-image-url.jpg"
            };
        }

        //-- Help Methods --//

        private async Task<T> FormatInfo<T>(Stream body)
        {
            string bodyAsString = string.Empty;

            using (var reader = new StreamReader(body))
            {
                bodyAsString = await reader.ReadToEndAsync();
            }

            return JsonConvert.DeserializeObject<T>(bodyAsString);
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)userStore;
        }
    }
}
