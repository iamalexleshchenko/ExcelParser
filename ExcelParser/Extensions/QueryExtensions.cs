using ExcelParser.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExcelParser.Extensions;

public static class QueryExtensions
{
    public static async Task<List<T>> ToListAsync<T>(this IQueryable<T> queryable, int? limit, int? offset, bool withTracking = false) where T : WithId
    {
        if (withTracking)
        {
            return await queryable
                .OrderBy(person => person.Id)
                .Skip(offset ?? 0)
                .Take(limit ?? 20)
                .ToListAsync();
        }
        
        return await queryable
            .AsNoTracking()
            .OrderBy(person => person.Id)
            .Skip(offset ?? 0)
            .Take(limit ?? 20)
            .ToListAsync();
    }
}