// Copyright (c) Atlantic. All rights reserved.

namespace ITRequest.WorkFlow.Infrastructure.Repositories
{
    using Fsel.Core.Base;
    using ITRequest.WorkFlow.Domain.Entities;
    using ITRequest.WorkFlow.Domain.IRepositories;

    public class RequestRepository : BaseRepository<Request>, IRequestRepository
    {
        public RequestRepository(WorkFlowDbContext dbContext, AuthContext authContext) : base(dbContext, authContext)
        {
        }
    }
}
