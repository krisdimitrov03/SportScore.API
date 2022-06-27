using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using SportScore.API.DataTransferModels;
using SportScore.API.SportsDataOperators.Contracts;
using SportScore.Infrastructure.Data;
using SportScore.Infrastructure.Data.Repositories;

#pragma warning disable CS8602 // Dereference of a possibly null reference.

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

        public async Task<(AuthReturnDTO?, string)> LogUserIn(Stream body)
        {
            var user = await FormatInfo<LoginDTO>(body);

            if (user == null) return (null, "Data formatted incorrectly.");

            var result = await signInManager.PasswordSignInAsync(user.Email, user.Password, user.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var userFromDB = repo
                    .All<ApplicationUser>()
                    .Where(u => u.UserName == user.Email)
                    .Select(u => new { u.Id, u.AccessToken })
                    .FirstOrDefault();

                return (new AuthReturnDTO()
                {
                    Id = userFromDB.Id,
                    Username = user.Email,
                    AccessToken = userFromDB.AccessToken
                }, string.Empty);
            }
            else return (null, "Wrong username or password.");
        }

        public async Task<(AuthReturnDTO?, string)> RegisterUser(Stream body)
        {
            var data = await FormatInfo<RegisterDTO>(body);

            if (data == null) return (null, "Data formatted incorrectly.");

            var errors = new List<string>();

            var user = Activator.CreateInstance<ApplicationUser>();

            await userStore.SetUserNameAsync(user, data.Email, CancellationToken.None);
            await emailStore.SetEmailAsync(user, data.Email, CancellationToken.None);

            try
            {
                user.FirstName = data.FirstName != null
                    ? data.FirstName
                    : throw new InvalidOperationException("FirstName cannot be null!");

                user.LastName = data.LastName != null
                    ? data.LastName
                    : throw new InvalidOperationException("LastName cannot be null!");

                user.AccessToken = Guid.NewGuid().ToString();

                user.ProfileImage = data.ProfileImage == null
                    ? "https://www.business2community.com/wp-content/uploads/2017/08/blank-profile-picture-973460_640.png"
                    : data.ProfileImage;

                var result = await userManager.CreateAsync(user, data.Password);

                errors.AddRange(result.Errors.Select(e => e.Description).ToList());

                if (result.Succeeded) return (new AuthReturnDTO() { Id = user.Id, Username = user.UserName, AccessToken = user.AccessToken }, string.Empty);
                else return (null, string.Join(";", errors));
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        public async Task<UserDetailsDTO> GetUserDetails(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null) return null;

            return new UserDetailsDTO()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfileImage = user.ProfileImage,
                Email = user.Email,
                Username = user.UserName
            };
        }

        public async Task<UserInNavDTO?> GetUserById(string id)
        {
            return repo.All<ApplicationUser>()
                .Where(user => user.Id == id)
                .Select(user => new UserInNavDTO()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    ProfileImage = user.ProfileImage
                }).FirstOrDefault();
        }

        //-- Help Methods --//

        private async Task<T?> FormatInfo<T>(Stream body)
            where T : class
        {
            string bodyAsString = string.Empty;

            using (var reader = new StreamReader(body))
            {
                bodyAsString = await reader.ReadToEndAsync();
            }

            try
            {
                return JsonConvert.DeserializeObject<T>(bodyAsString);
            }
            catch (Exception)
            {
                return null;
            }
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

#pragma warning restore CS8602 // Dereference of a possibly null reference.