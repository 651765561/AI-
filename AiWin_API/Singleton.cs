using System;

namespace AiWin_API
{
    public class Singleton<T> where T : new()
    {
        private static readonly Lazy<T> Lazy =
            new Lazy<T>(() => new T());

        protected Singleton()
        {
        }

        public static T Instance => Lazy.Value;
    }
}