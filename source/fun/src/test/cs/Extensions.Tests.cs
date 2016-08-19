namespace Fun.Tests {
    
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class ExtensionsTests {

        [Test]
        public void TestApply () {
            Assert.That (100.Apply ((Int32 x) => x * 2), Is.EqualTo(200));
            Assert.That (100.0.Apply ((Double x) => x * 2.0), Is.EqualTo(200.0));
            Assert.That ("foo".Apply ((String x) => x + "bar"), Is.EqualTo("foobar"));
        }
    }
}
