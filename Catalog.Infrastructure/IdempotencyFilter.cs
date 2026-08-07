using Catalog.Infrastructure.Persistence;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure;

internal class IdempotencyFilter<TEntity> : IFilter<ConsumeContext<TEntity>> where TEntity : class
{
    private readonly CatalogDbContext _context;

    public IdempotencyFilter(CatalogDbContext context)
    {
        _context = context;
    }

    public void Probe(ProbeContext context) => context.CreateFilterScope("idempotency");

    public async Task Send(ConsumeContext<TEntity> context, IPipe<ConsumeContext<TEntity>> next)
    {
        var messageId = context.MessageId;
        if (messageId is null)
        {
            await next.Send(context);
            return;
        }
        try
        {
            _context.Set<ProcessMessageLog>().Add(new ProcessMessageLog
            {
                MessageId = messageId.Value,
                ProcessedAtUtc = DateTime.UtcNow
            });
            await next.Send(context);
        }
        catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex)) { return; }
    }
    private bool IsUniqueConstraintViolation(DbUpdateException ex)
    {
        return ex.InnerException?.Message.Contains("PRIMARY KEY") == true ||
               ex.InnerException?.Message.Contains("unique constraint") == true;
    }
}

public class ProcessMessageLog
{
    public Guid MessageId { get; set; }
    public DateTime ProcessedAtUtc { get; set; }
}
