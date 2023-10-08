// Copyright (c) Atlantic. All rights reserved.

using Fsel.Core.Extensions;
using ITRequest.WorkFlow.Application.Service.SenderServices;
using ITRequest.WorkFlow.Application.Service.UserServices;
using ITRequest.WorkFlow.Domain.IRepositories;
using ITRequest.WorkFlow.Infrastructure;
using ITRequest.WorkFlow.Infrastructure.Repositories;
using ITRequest.WorkFlow.Infrastructure.ValueSetting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var appSetting = builder.AddAppSettings<AppSetting>();
builder.AddServices(appSetting);
builder.AddSwaggerGens(appSetting);
builder.AddAuthenticationJwtBearers(appSetting);
builder.AddDbContexts<WorkFlowDbContext>();

builder.Services.AddScoped<IRequestRepository, RequestRepository>();
builder.Services.AddScoped<IApprovalRepository, ApprovalRepository>();

builder.AddRefitClients(typeof(IUserService), appSetting?.Services?.UserApiUrl, appSetting?.Jwt?.SecretKey);
builder.AddRefitClients(typeof(ISenderService), appSetting?.Services?.SenderApiUrl, appSetting?.Jwt?.SecretKey);

var app = builder.Build();
app.UseServices();
app.Run();
