using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace WebApplication.Models
{
    public class CustomUserValidator<TUser> : IUserValidator<User>
    {
        public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user)
        {
            List<IdentityError> errors = new List<IdentityError>();

            if (!user.Email.ToLower().EndsWith("@gmail.com") && !user.Email.ToLower().EndsWith("@knu.ua"))
            {
                errors.Add(new IdentityError
                {
                    Description = "Електронна адреса доступна тільки @gmail.com або @knu.ua"
                });
            }
            return Task.FromResult(errors.Count == 0 ?
                IdentityResult.Success : IdentityResult.Failed(errors.ToArray()));
        }
    }
}