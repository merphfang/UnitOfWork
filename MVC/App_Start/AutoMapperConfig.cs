using AutoMapper;
using MVC.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UnitOfWorkExample.Domain.Entities;

namespace MVC
{
    public static class AutoMapperConfig
    {
        public static void RegisterMappings() {
            Mapper.Initialize(config => {
                config.CreateMap<User, AccountViewModel>()
                .ForMember(dest => dest.Customer,
                           opts => opts.MapFrom(src => src.Customer.Name))
                .ForMember(dest => dest.CreatedDate,
                           opts => opts.MapFrom(src => src.CreatedDate.ToString()));
            });
        }
    }
}