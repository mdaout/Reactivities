using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain;
using FluentValidation;
using MediatR;
using Persistence;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Application.Errors;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace Application.User
{
    public class RegisterUser2  : IRegisterUser
    {
            private readonly DataContext _context;
            private readonly UserManager<AppUser> _userManager;
            private readonly IJwtGenerator _jwtGenerator;

            public  RegisterUser2 (DataContext context, UserManager<AppUser> userManager,
            IJwtGenerator jwtGenerator)
            {
                _context = context;
                _userManager = userManager;
                _jwtGenerator = jwtGenerator;
            }

            public async Task<User> AddIt (AddUser request)
            {
                if (await _context.Users.Where(x => x.Email == request.Email).AnyAsync())
                {
                    throw new RestException(HttpStatusCode.BadRequest, new { Email = " Email already exists" });
                }
                if (await _context.Users.Where(x => x.UserName == request.UserName).AnyAsync())
                {
                    throw new RestException(HttpStatusCode.BadRequest, new { UserName = " User Name already exists" });
                };
                var user = new AppUser
                {
                    DisplayName = request.DisplayName,
                    UserName = request.UserName,
                    Email = request.Email
                };
                    var result = await _userManager.CreateAsync(user, request.Password);
                     if (result.Succeeded)
                {
                    // TODO: generate token
                    return new User
                    {
                        DisplayName = user.DisplayName,
                        Token = _jwtGenerator.CreateToken(user),
                        UserName = user.UserName,
                        Image = null
                    };
                }

                throw new Exception("Problem adding record");
            }

        // public Task<User> AddIt(RegisterUser.AddUser request)
        // {
        //     throw new NotImplementedException();
        // }

        // public class AddUser 
        // {
        //     public string DisplayName { get; set; }
        //     public string UserName { get; set; }
        //     public string Email { get; set; }
        //     public string Password { get; set; }

        //     // Properites go here 
        // }

        // public class QueryValidator : AbstractValidator<AddUser>
        // {
        //     public QueryValidator()
        //     {
        //         RuleFor(x => x.DisplayName).NotEmpty();
        //         RuleFor(x => x.UserName).NotEmpty();
        //         RuleFor(x => x.Email).NotEmpty();
        //         RuleFor(x => x.Password).NotEmpty();
        //     }
        // }
       
        }
}