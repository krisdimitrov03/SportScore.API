using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SportScore.Infrastructure.Data;

namespace SportScore.Infrastructure.Seeders
{
    public class Seeder
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<SportScoreContext>();

                context.Database.EnsureCreated();

                if (!context.Users.Any())
                {
                    var users = new List<ApplicationUser>();

                    using (var reader = new StreamReader("../SportScore.Infrastructure/Seeders/Data/users.json"))
                    {
                        users = JsonConvert.DeserializeObject<List<ApplicationUser>>(reader.ReadToEnd());
                    }

                    context.Users.AddRange(users);
                    context.SaveChanges();
                }
            }
        }
    }
}
