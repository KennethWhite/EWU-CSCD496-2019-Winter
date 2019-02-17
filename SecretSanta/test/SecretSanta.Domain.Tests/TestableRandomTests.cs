using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SecretSanta.Domain.Tests
{
    [TestClass]
    public class TestableRandomTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestableRandom_ThrowsErrorIfListIsNull()
        {
            var testableRand = new TestableRandom(null);
        }
        
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestableRandom_ThrowsErrorIfNoNext()
        {
            var randInts = Enumerable.Range(1,2).ToList();
            var testableRand = new TestableRandom(randInts);
            testableRand.Next();
            testableRand.Next();
            testableRand.Next();     
        }

        [TestMethod]
        public void TestableRandom_IndexIncrementsOnNextCall()
        {
            var randInts = Enumerable.Range(1,5).ToList();
            var testableRand = new TestableRandom(randInts);
            Assert.AreEqual(0,testableRand.Index);
            testableRand.Next();            
            Assert.AreEqual(1,testableRand.Index);

        }

        [TestMethod]
        public void TestableRandom_GetsNextInt()
        {
            var randInts = Enumerable.Range(1,5).ToList();
            var testableRand = new TestableRandom(randInts);
            Assert.AreEqual(1,testableRand.Next());
            Assert.AreEqual(2,testableRand.Next());
            Assert.AreEqual(3,testableRand.Next());
        }

        
        [DataTestMethod]
        [DataRow(new []{5, 6, 7, 8, 9}, 4)]
        [DataRow(new []{6, 9, 6, 7, 8}, 1)]
        [DataRow(new []{6, 5, 4, 7, 8}, 3)]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestableRandom_GetsNextIntUnderMaxThrowsException(int[] randArray, int nextMax)
        {
            var testableRand = new TestableRandom(randArray.ToList());
            testableRand.Next(nextMax);            
        }

        [DataTestMethod]
        [DataRow(new []{1, 2, 3, 4, 5}, 4, 1)]
        [DataRow(new []{1, 2, 3, 4, 5}, 1, 1)]
        [DataRow(new []{5, 4, 3, 2, 1}, 4, 4)]
        [DataRow(new []{5, 4, 3, 2, 1}, 1, 1)]
        [DataRow(new []{7, 4, 6, 3, 2}, 2, 2)]
        [DataRow(new []{7, 4, 6, 3, 2}, 3, 3)]
        public void TestableRandom_GetsNextIntUnderMax(int[] randArray, int nextMax, int expectedNext)
        {
            var testableRand = new TestableRandom(randArray.ToList());
            Assert.AreEqual(expectedNext, testableRand.Next(nextMax));
        }
    }
}