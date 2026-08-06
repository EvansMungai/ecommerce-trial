using Microsoft.EntityFrameworkCore;
using Order.Application.Interfaces;
using Order.Domain.Entities;
using System.Reflection;

namespace Order.Infrastructure.Persistence;

public class OrderDbContext : DbContext, IUnitOfWork
{
    #region
    public DbSet<OrderDomain> Orders => Set<OrderDomain>();
    public DbSet<OrderItem> OrderItem => Set<OrderItem>();
    #endregion

    #region Constructor
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }
    #endregion

    #region Utilities
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    #endregion

    #region Methods
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => base.SaveChangesAsync(cancellationToken);
    #endregion
}
