namespace Fun.Tests {

    using System;
    using NUnit.Framework;

    [TestFixture]
    public class UnitTests {
        [Test]
        public void TestEquality () {   
            Assert.That (new Unit (), Is.EqualTo(new Unit ()));
            Assert.That (Unit.Value, Is.EqualTo(new Unit ()));
            Assert.That (new Unit (), Is.EqualTo(Unit.Value));

            Assert.That (new Unit () == new Unit ());
            Assert.That (!(new Unit () != new Unit ()));
        }

        [Test]
        public void TestGetHashCode () {
            Assert.That (new Unit ().GetHashCode (), Is.EqualTo (new Unit ().GetHashCode ()));
        }

        [Test]
        public void TestToString () {
            Assert.That (Unit.Value.ToString (), Is.EqualTo ("()"));
        }
    }
}
