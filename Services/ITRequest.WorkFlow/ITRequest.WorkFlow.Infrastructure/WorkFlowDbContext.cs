namespace ITRequest.WorkFlow.Infrastructure
{
    using Fsel.Common.Constants;
    using Fsel.Core.Base;
    using ITRequest.WorkFlow.Domain.Entities;
    using ITRequest.WorkFlow.Infrastructure.Configs;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    public class WorkFlowDbContext : BaseDbContext
    {
        public WorkFlowDbContext(DbContextOptions<WorkFlowDbContext> options, IMediator mediator, AuthContext authContext) : base(options, mediator, authContext)
        {
        }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Approval> Approvals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ArgumentNullException.ThrowIfNull(modelBuilder);
            modelBuilder.ApplyConfiguration(new RequestEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ApprovalEntityTypeConfiguration());
            base.OnModelCreating(modelBuilder);
        }
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
