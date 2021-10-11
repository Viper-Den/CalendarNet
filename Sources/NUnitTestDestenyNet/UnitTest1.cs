using NUnit.Framework;
using Destiny.Core;
using System;

namespace NUnitTestDestinyNet
{
    [TestFixture]
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestIndexDayByDayOfWeek()
        {
            Assert.IsTrue(DateHelper.GetIndexDay(DayOfWeek.Monday) == 0);
            Assert.IsTrue(DateHelper.GetIndexDay(DayOfWeek.Tuesday) == 1);
            Assert.IsTrue(DateHelper.GetIndexDay(DayOfWeek.Wednesday) == 2);
            Assert.IsTrue(DateHelper.GetIndexDay(DayOfWeek.Thursday) == 3);
            Assert.IsTrue(DateHelper.GetIndexDay(DayOfWeek.Friday) == 4);
            Assert.IsTrue(DateHelper.GetIndexDay(DayOfWeek.Saturday) == 5);
            Assert.IsTrue(DateHelper.GetIndexDay(DayOfWeek.Sunday) == 6);
            Assert.Pass();
        }
        [Test]
        public void TestIndexDayByDate()
        {
            var d = new DateTime(2021, 9, 6);
            Assert.IsTrue(DateHelper.GetIndexDay(d) == 0);
            Assert.IsTrue(DateHelper.GetIndexDay(d.AddDays(1)) == 1);
            Assert.IsTrue(DateHelper.GetIndexDay(d.AddDays(2)) == 2);
            Assert.IsTrue(DateHelper.GetIndexDay(d.AddDays(3)) == 3);
            Assert.IsTrue(DateHelper.GetIndexDay(d.AddDays(4)) == 4);
            Assert.IsTrue(DateHelper.GetIndexDay(d.AddDays(5)) == 5);
            Assert.IsTrue(DateHelper.GetIndexDay(d.AddDays(6)) == 6);
            Assert.Pass();
        }
    }
}