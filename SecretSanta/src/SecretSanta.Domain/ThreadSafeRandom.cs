using System;

namespace SecretSanta.Domain
{
    /**
     * When a new ThreadSafeRandom is created, A global instance of random is created. It serves as the seed for a local random object
     * Whenever this object is used, the local Rand is checked that it is created. if not the global random is locked and its Next() method
     * is called and used as the seed for a local random. Then the instance of ThreadSafeRandom can be used freely
     */
    public class ThreadSafeRandom : IRandom
    {
        private static readonly Random _globalRandom = new Random();
        [ThreadStatic] private static Random _localRandom;


        public ThreadSafeRandom()
        {
            EnsureRandIsValid();
        }

        private void EnsureRandIsValid()
        {
            if (_localRandom == null)
            {
                lock (_globalRandom)
                {
                    _localRandom = new Random(_globalRandom.Next());
                }
            }
        }

        public int Next()
        {
            EnsureRandIsValid();
            return _localRandom.Next();
        }

        public int Next(int max)
        {
            EnsureRandIsValid();
            return _localRandom.Next(max);
        }
    }
}