// Copyright (c) Atlantic. All rights reserved.

using Fsel.Core.Extensions;
using ITRequest.Identity.Domain.Entities;
using ITRequest.Identity.Domain.IRepositories;
using ITRequest.Identity.Infrastructure.Repositories;
using ITRequest.Identity.Infrastructure;
using Microsoft.AspNetCore.Identity;
using ITRequest.Identity.Infrastructure.ValueSetting;

var builder = WebApplication.CreateBuilder(args);
var appSetting = builder.AddAppSettings<AppSetting>();
builder.AddServices(appSetting);
builder.AddSwaggerGens(appSetting);
builder.AddAuthenticationIdentity(appSetting);
builder.AddDbContexts<UserDbContext>();

builder.Services.AddIdentity<User, Role>()
        .AddEntityFrameworkStores<UserDbContext>()
        .AddDefaultTokenProviders();

builder.Services.AddScoped<IUserTokenRepository, UserTokenRepository>();
builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();


var app = builder.Build();
app.UseServices(appSetting);
app.Run();
