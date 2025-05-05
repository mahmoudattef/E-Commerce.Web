using Shared.DataTransferObject.IdentityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbstraction
{
    public  interface IAuthenticationService
    {
        Task<UserDto> LoginAsync (LoginDto loginDto);
        Task<UserDto> RegisterAsync(RegisterDto registerDto);
        //Check Email
        //Take Email Then Return boolean To Client  
        Task<bool> CheckEmailAsync(string email);

        //Get Current User Address 
        //Take Email Then Return Address [AddressDto]
        Task<AddressDto> GetCurrentUserAddressAsync(string Email);

        //Update Current User Address 
        ///Take Updated Address[AddressDto] and Email Then Return Address [AddressDto] after Update To 
        Task<AddressDto>UpdateCurrentUserAddressAsync(string Email ,AddressDto addressDto);

        //Current User Endpoint
        //Take Email Then Return [UserDto] Token , Email and Display Name To Client  
        Task<UserDto> GetCurrentUserAsync (string Email);


    }
}
