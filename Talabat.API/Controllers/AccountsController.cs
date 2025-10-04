using AutoMapper;
using Talabat.API.Errors;
using Talabat.API.Extensions;
using Talabat.Core.Dtos;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Talabat.API.Controllers
{
    public class AccountsController : ApiBaseController
    {
        private readonly ITokenService tokenService;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly IMapper mapper;

        public AccountsController(ITokenService tokenService,UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,IMapper mapper)
        {
            this.tokenService = tokenService;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.mapper = mapper;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto login)
        {
            var user= await userManager.FindByEmailAsync(login.Email);

            if (user is null) return Unauthorized(new ApiResponse(401));

            var result = await signInManager.CheckPasswordSignInAsync(user, login.Password,false);

            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

            return Ok(new UserDto()
            {
                DisplayName = login.Email.Split("@")[0],
                Email = login.Email,
                Token= await tokenService.GenerateToken(user, userManager)
            });
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto register)
        {
            if (CheckEmailExists(register.Email).Result.Value)
                return BadRequest(new ApiValidationErrorResponse(){ Errors = new List<string>() { "this email is already found" } }); 

            // UserName is required
            var user = new AppUser()
            {
                Email = register.Email,
                UserName= register.Email.Split("@")[0],
                PhoneNumber= register.PhoneNumber,
                DisplayName= register.DisplayName
            };

            var result= await userManager.CreateAsync(user,register.Password);

            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            return Ok(new UserDto()
            {
                DisplayName= user.DisplayName,
                Email= user.Email,
                Token= await tokenService.GenerateToken(user,userManager)
            });
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var user= await userManager.FindByEmailAsync(email);

            return Ok(new UserDto()
            {
                DisplayName= user.DisplayName,
                Email= user.Email,
                Token= await tokenService.GenerateToken(user,userManager)
            });
        }

        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await userManager.FindUserAddressAsync(User);

            var address = mapper.Map<AddressDto>(user.Address);
            
            return Ok(address);
        }

        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto updatedAddress)
        {
            var user = await userManager.FindUserAddressAsync(User);

            if(user!.Address is not null)
            updatedAddress.Id = user.Address.Id;
            user.Address = mapper.Map<Address>(updatedAddress);

            var result= await userManager.UpdateAsync(user);

            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            return Ok(updatedAddress);
        }


        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
        {
            return await userManager.FindByEmailAsync(email) is not null;
        }


    }
}
