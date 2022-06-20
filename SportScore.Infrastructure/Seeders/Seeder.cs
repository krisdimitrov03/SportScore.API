using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SportScore.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportScore.Infrastructure.Seeders
{
    public class Seeder
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using(var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<SportScoreContext>();

                context.Database.EnsureCreated();

                if (!context.Set<ApplicationUser>().Any())
                {
                    //TODO
                }
            }
        }
    }
}
