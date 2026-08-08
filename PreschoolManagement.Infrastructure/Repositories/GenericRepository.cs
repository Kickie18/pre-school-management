using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PreschoolManagement.Application.Common;
using PreschoolManagement.Application.Interfaces;
using PreschoolManagement.Domain.Common;
using PreschoolManagement.Infrastructure.Persistence;

namespace PreschoolManagement.Infrastructure.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
{
    private readonly PreschoolDbContext _dbContext;
    private readonly DbSet<TEntity> _dbSet;

    public GenericRepository(PreschoolDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = dbContext.Set<TEntity>();
    }

    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<PagedResult<TEntity>> GetPagedAsync(QueryParameters query, CancellationToken cancellationToken = default)
    {
        var loadedItems = await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
        var entityQuery = loadedItems.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim().ToLowerInvariant();
            entityQuery = entityQuery.Where(x =>
                x.Id.ToString().Contains(search, StringComparison.OrdinalIgnoreCase) ||
                x.GetType().GetProperties()
                    .Where(p => p.PropertyType == typeof(string))
                    .Select(p => p.GetValue(x)?.ToString())
                    .Any(value => !string.IsNullOrWhiteSpace(value) && value.Contains(search, StringComparison.OrdinalIgnoreCase)));
        }

        if (!string.IsNullOrWhiteSpace(query.FilterBy) && !string.IsNullOrWhiteSpace(query.FilterValue))
        {
            entityQuery = entityQuery.Where(x =>
            {
                var prop = x.GetType().GetProperty(query.FilterBy!);
                var value = prop?.GetValue(x)?.ToString();
                return !string.IsNullOrWhiteSpace(value) && value.Contains(query.FilterValue!, StringComparison.OrdinalIgnoreCase);
            });
        }

        var sortBy = string.IsNullOrWhiteSpace(query.SortBy) ? nameof(BaseEntity.CreatedDate) : query.SortBy;
        entityQuery = query.SortDescending
            ? entityQuery.OrderByDescending(x => x.GetType().GetProperty(sortBy!)?.GetValue(x))
            : entityQuery.OrderBy(x => x.GetType().GetProperty(sortBy!)?.GetValue(x));

        var totalCount = entityQuery.Count();
        var skip = (query.PageNumber - 1) * query.PageSize;
        var items = entityQuery.Skip(skip).Take(query.PageSize).ToList();

        return new PagedResult<TEntity>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize
        };
    }

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking().Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
    }

    public void Update(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    public void SoftDelete(TEntity entity)
    {
        entity.IsDeleted = true;
        _dbSet.Update(entity);
    }
}
