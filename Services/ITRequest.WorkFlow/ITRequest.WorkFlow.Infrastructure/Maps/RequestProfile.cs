// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.WorkFlow.Infrastructure.Maps
{
    using AutoMapper;
    using Fsel.Core.Extensions;
    using ITRequest.WorkFlow.Domain.Entities;
    using ITRequest.WorkFlow.Domain.Models.Commands.Requests;
    using ITRequest.WorkFlow.Domain.Models.EntityModels;

    public class RequestProfile : Profile
    {
        public RequestProfile()
        {
            CreateMap<CreateRequestCommandModel, Request>().IgnoreAllNonExisting();
            CreateMap<Request, RequestModel>().IgnoreAllNonExisting();
        }
    }
}
