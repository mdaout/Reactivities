using System.Threading.Tasks;
using Application.Interfaces;
using Domain;
using Microsoft.AspNetCore.Identity;
using Persistence;

namespace Application.User
{
    public class CurrentLoggedinUser
    {
        
            private readonly IUserAccessor _userAccessor; 
            private readonly UserManager<AppUser> _userManager;
            private readonly IJwtGenerator _jwtGenerator; 

            public CurrentLoggedinUser ( UserManager<AppUser> userManager,
            IJwtGenerator jwtGenerator, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _userManager = userManager;
                _jwtGenerator = jwtGenerator;
            }

             public async Task<User> GetUser()
            {
                var user = await _userManager.FindByNameAsync(_userAccessor.GetCurrentUsername());

                return new User
                {
                    DisplayName = user.DisplayName,
                    UserName = user.UserName,
                    Token = _jwtGenerator.CreateToken(user),
                    Image = null
                };
            }
    }
}