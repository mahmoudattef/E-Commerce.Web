using AutoMapper;
using DomainLayer.Exceptions;
using DomainLayer.Models.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServiceAbstraction;
using Shared.DataTransferObject.IdentityDtos;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class AuthenticationService(UserManager<ApplicationUser> _userManager ,IConfiguration _configuration ,IMapper _mapper) : IAuthenticationService
    {
        public async Task<bool> CheckEmailAsync(string email)
        {
            var User =await _userManager.FindByEmailAsync(email);
            return User is not null;
        }
        public async Task<UserDto> GetCurrentUserAsync(string Email)
        {
           var User= await _userManager.FindByEmailAsync(Email) ?? throw new UserNotFoundException(Email);
            return new UserDto() { DisplayName = User.DisplayName, Email = Email ,Token =await CreateTokenAsync(User)};
        }


        public async Task<AddressDto> GetCurrentUserAddressAsync(string Email)
        {
            var User = await _userManager.Users.Include(U => U.Address)
                                              .FirstOrDefaultAsync(U => U.Email == Email) ?? throw new UserNotFoundException(Email);
            if (User.Address is not null) return _mapper.Map<Address, AddressDto>(User.Address);
            else throw new AddressNotFoundException(User.UserName);

        }

        public async Task<AddressDto> UpdateCurrentUserAddressAsync(string Email, AddressDto addressDto)
        {
            var User = await _userManager.Users.Include(U => U.Address)
                                              .FirstOrDefaultAsync(U => U.Email == Email) ?? throw new UserNotFoundException(Email);
            if(User.Address is not null) //Update
            {
                User.Address.FirstName = addressDto.FirstName;
                User.Address.LastName = addressDto.LastName;
                User.Address.City = addressDto.City;
                User.Address.Country = addressDto.Country;
                User.Address.Streat = addressDto.Street;


            }
            else //Add new Address
            {
                User.Address = _mapper.Map<AddressDto, Address>(addressDto);
            }
            await _userManager.UpdateAsync(User);
            return _mapper.Map<AddressDto>(User.Address);
        }

        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {
            //Check If Email Exist
            var User =await _userManager.FindByEmailAsync(loginDto.Email) ?? throw new UserNotFoundException(loginDto.Email); ;

            //Check Password
            var IsPasswordValid = await _userManager.CheckPasswordAsync(User, loginDto.Password);
            if (IsPasswordValid)
            {
                //Return UserDto
                return new UserDto()
                {
                    DisplayName = User.DisplayName,
                    Email = User.Email,
                    Token =await CreateTokenAsync(User)

                };

            }
            else
                throw new UnAuthorizedException();
        }


        public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
        {
            //Mapp RegisterDto => ApplicationUser
            var User = new ApplicationUser()
            {
                Email = registerDto.Email,
                DisplayName = registerDto.DisplayName,
                UserName = registerDto.UserName,
                PhoneNumber = registerDto.PhoneNumber
                
            };

            //Create User [Application User] 
            var Result =await _userManager.CreateAsync(User ,registerDto.Password);
            if (Result.Succeeded)
                return new UserDto() { DisplayName = User.DisplayName, Email = User.Email, Token =await CreateTokenAsync(User) }; 
                //Return UserDto 
                //Throw Exc If UserDto Not
                else
            {
                var Errors =Result.Errors.Select(E=>E.Description).ToList();
                throw new BadRequestException(Errors);
            }
        }


        private  async Task<string> CreateTokenAsync(ApplicationUser user)
        {
            //Payload
            var Claim = new List<Claim>()
            {
                new  (ClaimTypes.Email,user.Email!),
                new (ClaimTypes.Name ,user.UserName!),
                new (ClaimTypes.NameIdentifier ,user.Id!)
            };

            var Roles =await _userManager.GetRolesAsync(user);

            foreach (var role in Roles) {
                Claim.Add(new Claim(ClaimTypes.Role, role));
                }
            //Key
            var SecretKey = _configuration.GetSection("JWTOptions")["SecretKey"];
            var Key =new SymmetricSecurityKey (Encoding.UTF8.GetBytes(SecretKey));

            //Credentials 
            var Creds =new SigningCredentials(Key,SecurityAlgorithms.HmacSha256);

            //Create Token
            var Token = new JwtSecurityToken(
                issuer: _configuration["JWTOptions:Issuer"], 
                 audience: _configuration["JWTOptions:Audience"],
                 expires: DateTime.Now.AddDays(1),
                signingCredentials: Creds
                );

            return new JwtSecurityTokenHandler().WriteToken(Token); 
        }
    }
}
