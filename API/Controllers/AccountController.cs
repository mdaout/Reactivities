
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
 
using System.Threading.Tasks;
using Application.User;
using Domain;
using System;
using Persistence;
using Application.Interfaces;

namespace API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
 
    public class AccountController : ControllerBase
    {
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly  IUserAccessor _userAccessor;

         private readonly DataContext _context;

        public AccountController(DataContext context, UserManager<AppUser> userMgr,
         SignInManager<AppUser> signinMgr, IJwtGenerator jwtGenerator,
         IUserAccessor userAccessor)
        {
            try {

            _userManager = userMgr;
            _signInManager = signinMgr;
            _jwtGenerator = jwtGenerator;
            _context =  context;
            _userAccessor = userAccessor;
        }
        catch(Exception ex ){

            string xx = ex.Message;
        }
        }
  
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login(LoginTo login)
       //     public async Task<ActionResult<AppUser>> Login(LoginTo login)
        {
            AppUser appUser = await _userManager.FindByEmailAsync(login.Email);
            if (appUser != null)
            {
                var result = await _signInManager
           .CheckPasswordSignInAsync(appUser, login.Password, false);
                if (result.Succeeded)
                {
                 //   return appUser;
                    // TODO: generate token
                    return new User
                    {
                        DisplayName = appUser.DisplayName,
                        Token = _jwtGenerator.CreateToken(appUser),
                        UserName = appUser.UserName
                    };
                }
            }
            return NotFound();
        }
          [HttpPost("register")]
        public async Task<ActionResult<User>> Register(RegisterUser.AddUser  command)
        {
        //     var sender = new RegisterUser.insertUser(_context, _userManager, _jwtGenerator );
        //    return await sender.AddIt(command); 

           var sender = new RegisterUser2(_context, _userManager, _jwtGenerator );
           return await sender.AddIt(command); 
        }

         [HttpGet]
        public async Task<ActionResult<User>> CurrentUser()
        {
            var sender = new  CurrentLoggedinUser( _userManager, _jwtGenerator, _userAccessor );
            return await sender.GetUser();
        
        }
    }
}