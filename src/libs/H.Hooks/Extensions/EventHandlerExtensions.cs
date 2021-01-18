using System;
using System.Threading;

namespace H.Hooks.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class EventHandlerExtensions
    {
        /// <summary>
        /// Invokes <paramref name="handler"/> in <see cref="ThreadPool"/>
        /// if <paramref name="pushToThreadPool"/> is <see langword="true"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        /// <param name="sender"></param>
        /// <param name="value"></param>
        /// <param name="pushToThreadPool"></param>
        public static void Invoke<T>(
            this EventHandler<T>? handler, 
            object sender, 
            T value, 
            bool pushToThreadPool)
        {
            if (pushToThreadPool)
            {
                ThreadPool.QueueUserWorkItem(_ =>
                {
                    handler?.Invoke(sender, value);
                });
                return;
            }

            handler?.Invoke(sender, value);
        }
    }
}
