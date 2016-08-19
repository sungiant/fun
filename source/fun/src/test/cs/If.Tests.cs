namespace Fun.Tests {
    
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    public class IfTests {
        [Test]
        public void TestIfElse1 () {
            Enumerable.Range (1, 2).ToList ().ForEach ( i => {
                Assert.That(
                    If.Else (
                        i == 1, () => 100,
                        () => 200),
                    Is.EqualTo (i * 100));
                }
            );
        }

        [Test]
        public void TestIfElse2 () {
            Enumerable.Range (1, 3).ToList ().ForEach ( i => {
                Assert.That(
                    If.Else (
                        i == 1, () => 100,
                        i == 2, () => 200,
                        () => 300),
                    Is.EqualTo (i * 100));
                }
            );
        }

        [Test]
        public void TestIfElse3 () {
            Enumerable.Range (1, 4).ToList ().ForEach ( i => {
                Assert.That(
                    If.Else (
                        i == 1, () => 100,
                        i == 2, () => 200,
                        i == 3, () => 300,
                        () => 400),
                    Is.EqualTo (i * 100));
                }
            );
        }

        [Test]
        public void TestIfElse4 () {
            Enumerable.Range (1, 5).ToList ().ForEach ( i => {
                Assert.That(
                    If.Else (
                        i == 1, () => 100,
                        i == 2, () => 200,
                        i == 3, () => 300,
                        i == 4, () => 400,
                        () => 500),
                    Is.EqualTo (i * 100));
                }
            );
        }

        [Test]
        public void TestIfElse5 () {
            Enumerable.Range (1, 6).ToList ().ForEach ( i => {
                Assert.That(
                    If.Else (
                        i == 1, () => 100,
                        i == 2, () => 200,
                        i == 3, () => 300,
                        i == 4, () => 400,
                        i == 5, () => 500,
                        () => 600),
                    Is.EqualTo (i * 100));
                }
            );
        }
    }
}
