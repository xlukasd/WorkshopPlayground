﻿namespace LuDurCustomExtension
{
    public record Message
    {
        public required string Role { get; set; }
        public required string Content { get; set; }
    }
}
