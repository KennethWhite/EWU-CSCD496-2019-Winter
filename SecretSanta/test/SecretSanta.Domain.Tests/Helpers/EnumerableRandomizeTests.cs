using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SecretSanta.Domain.Tests.Helpers
{
    [TestClass]
    public class EnumerableRandomizeTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EnumerableRandomize_ThrowsExceptionIfIRandomNull()
        {
            var list = Enumerable.Range(1, 5).ToList();
            var randList = list.Randomize(null).ToList();
        }

        [DataTestMethod]
        [DataRow(new []{5, 6, 7, 8, 9})]
        [DataRow(new []{6, 9, 6, 7, 8})]
        [DataRow(new []{6, 5, 4, 7, 8})]
        public void EnumerableRandomize_ListIsRandomized(int[] initialArray)
        {
            var random = new ThreadSafeRandom();
            var listBefore = new List<int>(initialArray);
            var listAfter = listBefore.Randomize(random).ToList();
            Assert.IsFalse(listBefore.SequenceEqual(listAfter));
        }
    }
}