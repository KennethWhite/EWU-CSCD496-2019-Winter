using System;
using System.Collections.Generic;

namespace SecretSanta.Domain.Tests
{
    public class TestableRandom : IRandom
    {
        public List<int> RandInts { get; set; }
        public int Index { get; set; }
        
        public TestableRandom(List<int> randInts)
        {    
            RandInts = randInts ?? throw new ArgumentNullException(nameof(randInts));
        }

        public int Next()
        {
            if(Index > RandInts.Count -1)
                throw new InvalidOperationException("Error calling Next() when there is no next int.");
            return RandInts[Index++];
        }

        public int Next(int max)
        {
            while (Index < RandInts.Count && RandInts[Index] > max)
            {
                Index++;
            }
            
           if(Index > RandInts.Count -1)
                throw new InvalidOperationException("Error calling Next(max) when there is no next int less than the maximum.");
            return RandInts[Index];
        }
    }
}