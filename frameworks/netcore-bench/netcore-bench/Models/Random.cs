using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace NetCoreBench.Models
{
    public class DefaultRandom : IRandom
    {
        private static int _nextSeed;

        // Random isn't thread safe
        [ThreadStatic] private static Random _random;

        private static Random Random => _random ?? CreateRandom();

        public int Next(int minValue, int maxValue)
        {
            return Random.Next(minValue, maxValue);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Random CreateRandom()
        {
            _random = new Random(Interlocked.Increment(ref _nextSeed));
            return _random;
        }
    }
}