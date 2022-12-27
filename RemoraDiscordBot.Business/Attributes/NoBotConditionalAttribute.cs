// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;

namespace RemoraDiscordBot.Business.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class NoBotConditionalAttribute
    : Attribute
{
    
}