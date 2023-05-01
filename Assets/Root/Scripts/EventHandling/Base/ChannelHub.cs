using System;
using System.Collections.Generic;
using YagizAyer.Root.Scripts.EventHandling.BasicPassableData;

namespace YagizAyer.Root.Scripts.EventHandling.Base
{
    public static class ChannelHub
    {
        private static readonly Dictionary<string, Channel> Channels = new();

        /// <summary>
        /// WARNING: This method only used for Event Handling System. Do not use it for other purposes.
        /// Subscribe to a channel.
        /// </summary>
        /// <param name="channelName">EventChannel to subscribe</param>
        /// <param name="listener">Listener to subscribe</param>
        internal static void Subscribe(this Enum channelName, Listener listener) =>
            GetChannel(channelName).Subscribe(listener);

        /// <summary>
        /// WARNING: This method only used for Event Handling System. Do not use it for other purposes.
        /// Unsubscribe from a channel.
        /// </summary>
        /// <param name="channelName">EventChannel to unsubscribe</param>
        /// <param name="listener">Listener to unsubscribe</param>
        internal static void Unsubscribe(this Enum channelName, Listener listener) =>
            GetChannel(channelName).Unsubscribe(listener);

        /// <summary>
        /// WARNING: This method only used for Event Handling System. Do not use it for other purposes.
        /// Raise an event on a channel.
        /// </summary>
        /// <param name="channel">EventChannel to raise</param>
        /// <param name="data">Data to pass to listeners</param>
        public static void Raise(this Enum channel, IPassableData data) => channel.GetChannel().Raise(data);

        private static Channel GetChannel(this Enum channelName)
        {
            if (Channels.TryGetValue(channelName.ToHash(), out var channel)) return channel;

            channel = new Channel();
            Channels.Add(channelName.ToHash(), channel);
            return channel;
        }

        private static string ToHash(this Enum me) => me.ToString() + (int)(object)me;
    }
}