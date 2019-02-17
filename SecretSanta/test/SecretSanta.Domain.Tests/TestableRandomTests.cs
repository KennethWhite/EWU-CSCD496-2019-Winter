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
        [ExpectedException(typeof(InvalidOperationException),
            "Error calling Next() when there is no next int.")]
        public void TestableRandom_ThrowsErrorIfNext()
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
    }
}