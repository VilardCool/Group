using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace WebApplication.Validators
{
    public interface IUserValidator<TUser> where TUser : class
    {
        Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user);
    }
}
