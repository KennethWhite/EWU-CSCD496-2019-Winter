using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SecretSanta.Domain.Tests.Helpers
{
    /*NOTE: because we are testing random, these tests can fail. But generally next returns a unique integer*/
    [TestClass]
    public class ThreadSafeRandomTests
    {
        [TestMethod]
        public void Next_UniqueIntsReturned()
        {
            var threadSafeRandom = new ThreadSafeRandom();
            int prevInt = 0;
            for (int i = 0; i < 10; i++)
            {
                var nextInt = threadSafeRandom.Next();
                Assert.AreNotEqual(prevInt, nextInt);
                prevInt = nextInt;
            }
        }

        [DataTestMethod]
        [DataRow(5)]
        [DataRow(100)]
        [DataRow(1000)]
        public void Next_max_UniqueIntsReturned(int max)
        {
            var threadSafeRandom = new ThreadSafeRandom();
            int prevInt = 0;
            for (int i = 0; i < 10; i++)
            {
                var nextInt = threadSafeRandom.Next(max);
                Assert.AreNotEqual(prevInt, nextInt);
                prevInt = nextInt;
            }
        }
        
        [DataTestMethod]
        [DataRow(5)]
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