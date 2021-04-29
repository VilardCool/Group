using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace WebApplication.Validators
{
    public interface IUserValidator<TUser> where TUser : class
    {
        Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user);
    }
}
