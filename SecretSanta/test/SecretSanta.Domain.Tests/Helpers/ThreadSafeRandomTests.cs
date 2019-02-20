using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SecretSanta.Domain.Tests.Helpers
{
    /*NOTE: because we are testing random, these tests can fail. But generally next returns a unique integer*/
    [TestClass]
    public class ThreadSafeRandomTests
    {
        [TestMethod]
        public void Next_UniqueIntsReturned() //ensures the random number does not repeat more that 2 times or roughly 20%
        {
            var threadSafeRandom = new ThreadSafeRandom();
            int numOfTimesRepeated = 0, prevInt = 1;
            
            for (int i = 0; i < 10; i++)
            {
                var nextInt = threadSafeRandom.Next();
                Console.WriteLine(nextInt);
                
                if (prevInt == nextInt)
                    Assert.IsTrue(++numOfTimesRepeated < 2);    
                else
                    numOfTimesRepeated = 0;
                
                prevInt = nextInt;
            }
        }

        [DataTestMethod]
        [DataRow(10)]
        [DataRow(100)]
        [DataRow(1000)]
        public void Next_max_UniqueIntsReturned(int max) //ensures the random number does not repeat more that 2 times or roughly 20%
        {
            var threadSafeRandom = new ThreadSafeRandom();
            int numOfTimesRepeated = 0, prevInt = 1;
            
            for (int i = 0; i < 10; i++)
            {
                var nextInt = threadSafeRandom.Next(max);
                Console.WriteLine(nextInt);
                
                if (prevInt == nextInt)
                    Assert.IsTrue(++numOfTimesRepeated < 2);
                else
                    numOfTimesRepeated = 0;
                
                prevInt = nextInt;
            }
        }

        [DataTestMethod]
        [DataRow(20)]
        [DataRow(100)]
        [DataRow(1000)]
        public void Next_max_UniqueIntsReturnedUnderMax(int max)
        {
            var threadSafeRandom = new ThreadSafeRandom();
            for (int i = 0; i < 10; i++)
            {
                Assert.IsTrue(max >= threadSafeRandom.Next(max));
            }
        }
    }
}