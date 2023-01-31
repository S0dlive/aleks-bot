// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;

namespace RemoraDiscordBot.Business.Extensions;

public static class AddOrUpdateExtension
{
    public static void AddOrUpdate(this DbContext ctx, object entity)
    {
        var entry = ctx.Entry(entity);
        switch (entry.State)
        {
            case EntityState.Detached:
                ctx.Add(entity);
                break;
            case EntityState.Modified:
                ctx.Update(entity);
                break;
            case EntityState.Added:
                ctx.Add(entity);
                break;
            case EntityState.Unchanged:
                break;

            case EntityState.Deleted:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}