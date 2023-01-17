// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using MediatR;
using Remora.Commands.Attributes;
using Remora.Commands.Groups;
using Remora.Rest.Core;
using Remora.Results;

namespace RemoraDiscordBot.Plugins.AutoRoles.CommandGroups;

[Group("autoroles")]
[Description("Commands for managing autoroles.")]
public class AutoRoleCommandGroup
     : CommandGroup
{
     private readonly IMediator _mediator;

     public AutoRoleCommandGroup(IMediator mediator)
     {
          _mediator = mediator;
     }
     
     [Command("create")]
     [Description("Creates a new autorole message.")]
     // create <message> [<channelId>]
     public async Task<Result> CreateAutoRoleAsync(string message, Snowflake? channelId = null)
     {
     }
     
     [Command("remove")]
     [Description("Removes an autorole message.")]
     // remove <messageId>
     public async Task<Result> RemoveAutoRoleAsync(Snowflake messageId)
     {
     }
}