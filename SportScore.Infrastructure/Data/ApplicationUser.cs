using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportScore.Infrastructure.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    [Required, StringLength(50)]
    public string FirstName { get; set; }

    [Required,StringLength(50)]
    public string LastName { get; set; }

    [Required,StringLength(100)]
    public string ProfileImage { get; set; }

    [Required]
    public string AccessToken { get; set; }
}

