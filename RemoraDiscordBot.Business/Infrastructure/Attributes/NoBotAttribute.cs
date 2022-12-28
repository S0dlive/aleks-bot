// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using Remora.Commands.Conditions;

namespace RemoraDiscordBot.Business.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.Parameter)]
public class NoBotAttribute
    : ConditionAttribute
{
}