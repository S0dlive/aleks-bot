// Copyright (c) Alexis Chân Gridel. All Rights Reserved.
// Licensed under the GNU General Public License v3.0.
// See the LICENSE file in the project root for more information.

using System.Runtime.Serialization;

namespace RemoraDiscordBot.Core.Exceptions;

[Serializable]
public class BotTokenCannotBeNullException
    : Exception
{
    public BotTokenCannotBeNullException()
        : base("The bot token cannot be null, you probably missed to set it in the configuration file.")
    {
    }

    protected BotTokenCannotBeNullException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    public BotTokenCannotBeNullException(string? message)
        : base(message)
    {
    }

    public BotTokenCannotBeNullException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}