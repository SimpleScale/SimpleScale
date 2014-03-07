using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

namespace SimpleScale.Common.UnitTests
{
    [TestFixture]
    public class ResultUnitTests
    {
        [Test]
        public void IfResultHasExceptionHasErrorReturnsTrue()
        {
            var results = new Result<int>(0, 0, Guid.NewGuid(), new Exception());
            Assert.That(results.HasError, Is.True);
        }

        [Test]
        public void IfResultsHasNullExceptionHasErrorReturnFalse()
        {
            var results = new Result<int>(0, 0, Guid.NewGuid(), null);
            Assert.That(results.HasError, Is.False);
        }

        [Test]
        public void IfResultsHasNoExceptionInConstructorHasErrorReturnsFalse()
        {
            var results = new Result<int>(0, 0, Guid.NewGuid());
            Assert.That(results.HasError, Is.False);
        }
    }
}
