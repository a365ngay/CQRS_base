// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.Identity.Infrastructure.Maps
{
    using AutoMapper;
    using Fsel.Core.Extensions;
    using ITRequest.Identity.Domain.Entities;
    using ITRequest.Identity.Domain.Models.EntityModels;

    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserModel>().IgnoreAllNonExisting();
        }
    }
}
