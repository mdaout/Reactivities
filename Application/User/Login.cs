using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Application.Errors;


namespace Application.User
{
    public class Login
    {

        public class Query : IRequest<User>
        {

            public string Email { get; set; }

            public string Password { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();

            }

        }
        public class Handler : IRequestHandler<Query, User>
        {
            private readonly SignInManager<AppUser> _signInManager;

            private readonly UserManager<AppUser> _userManager;

            public Handler(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
            {
                _userManager = userManager;
                _signInManager = signInManager;
            }

            public async Task<User> Handle(Query request, CancellationToken cancellationToken)
            {
                // Handler Logic  Goes Here 
                var user = await _userManager.FindByEmailAsync(request.Email);
                //   var activities = await _context.Activities.Include.  
                if (user == null)
                {
                    throw new RestException(HttpStatusCode.Unauthorized);
                }

                var result = await _signInManager
                .CheckPasswordSignInAsync(user, request.Password, false);
                if (result.Succeeded)
                {
                    // TODO: generate token
                    return new User{

                        DisplayName = user.DisplayName,
                        Token = "This will be a token",
                        UserName = null
                    };
                }
                  throw new RestException(HttpStatusCode.Unauthorized);
            }
        }
    }
}