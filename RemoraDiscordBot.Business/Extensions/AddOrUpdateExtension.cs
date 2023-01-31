// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace RemoraDiscordBot.Business.Extensions;

public static class AddOrUpdateExtension
{
    public static async Task AddOrUpdateAsync<T>(
        this DbSet<T> dbSet,
        T entity,
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default)
        where T : class
    {
        var existing = await dbSet.FirstOrDefaultAsync(predicate, cancellationToken);

        if (existing == null)
        {
            await dbSet.AddAsync(entity, cancellationToken);
        }
        else
        {
            dbSet.Update(entity);
        }
    }
}