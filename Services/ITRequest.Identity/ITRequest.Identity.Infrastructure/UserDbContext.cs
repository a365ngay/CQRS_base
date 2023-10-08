namespace ITRequest.Identity.Infrastructure
{
    using System.Reflection.Emit;
    using Fsel.Common.Constants;
    using Fsel.Core.Base;
    using ITRequest.Identity.Domain.Entities;
    using ITRequest.Identity.Infrastructure.Configs;
    using MediatR;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    public class UserDbContext : BaseIdentityDbContext<User, Role, string, IdentityUserClaim<string>, IdentityRoleClaim<string>, UserToken>
    {
        public UserDbContext(DbContextOptions<UserDbContext> options, IMediator mediator, AuthContext authContext) : base(options, mediator, authContext)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder);
            builder.ApplyConfiguration(new UserEnityTypeConfiguration());
            base.OnModelCreating(builder);
        }

        #region Db Set
        public override DbSet<User> Users { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
        public DbSet<Role> Roles { get; set; }

        #endregion
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            ArgumentNullException.ThrowIfNull(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile(Settings.SettingFileName)
                    .Build();
                optionsBuilder.UseSqlServer(
                    configuration.GetConnectionString(Settings.DefaultConnection),
                    options => options.MigrationsAssembly(GetType().Assembly.GetName().Name));
            }
        }
    }
}
