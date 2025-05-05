using AutoMapper;
using DomainLayer.Models.IdentityModule;
using Shared.DataTransferObject.IdentityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.MappingProfiles
{
    internal class IdentityProfile:Profile
    {
        public IdentityProfile()
        {
            CreateMap<Address,AddressDto>().ReverseMap();
        }
    }
}
