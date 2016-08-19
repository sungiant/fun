namespace Fun.Tests{
    
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class EitherTests {
        [Test]
        public void TestLeft () {
        	var t = Either.Left <Int32, String> (42);
            Assert.That (t.HasLeft);
            Assert.That (!t.HasRight);
            Assert.That (t.Left, Is.EqualTo (Option.Apply (42)));
            Assert.That (t.Right, Is.EqualTo (Option.None <String> ()));
        }

        [Test]
        public void TestRight () {
        	var t = Either.Right <Int32, String> ("hello world");
            Assert.That (t.HasRight);
            Assert.That (!t.HasLeft);
            Assert.That (t.Right, Is.EqualTo (Option.Apply ("hello world")));
            Assert.That (t.Left, Is.EqualTo (Option.None <Int32> ()));
        }
        
        [Test]
        public void TestEquality () {
            Assert.That (Either.Right <Int32, String> ("hello world"), Is.EqualTo (Either.Right <Int32, String> ("hello world")));
            Assert.That (Either.Right <Int32, String> ("hello world"), Is.Not.EqualTo (Either.Right <Int32, String> ("foobar")));
            Assert.That (Either.Left <Int32, String> (42), Is.EqualTo (Either.Left <Int32, String> (42)));
            Assert.That (Either.Right <Int32, String> ("hello world"), Is.Not.EqualTo (Either.Left <Int32, String> (42)));
            Assert.That (Either.Right <Double, Double> (3.14), Is.EqualTo (Either.Right <Double, Double> (3.14)));
            Assert.That (Either.Left <Double, Double> (3.14), Is.EqualTo (Either.Left <Double, Double> (3.14)));
            Assert.That (Either.Left <Double, Double> (3.14), Is.Not.EqualTo (Either.Right <Double, Double> (3.14)));

            Assert.That (Either.Right <Int32, String> ("hello world") == Either.Right <Int32, String> ("hello world"));
            Assert.That (Either.Right <Int32, String> ("hello world") != Either.Right <Int32, String> ("foobar"));
            Assert.That (Either.Left <Int32, String> (42) == Either.Left <Int32, String> (42));
            Assert.That (Either.Right <Int32, String> ("hello world") != Either.Left <Int32, String> (42));
            Assert.That (Either.Right <Double, Double> (3.14) == Either.Right <Double, Double> (3.14));
            Assert.That (Either.Left <Double, Double> (3.14) == Either.Left <Double, Double> (3.14));
            Assert.That (Either.Left <Double, Double> (3.14) != Either.Right <Double, Double> (3.14));

            Assert.That (Either.Right <Int32, String> ("hello world"), Is.Not.EqualTo (Either.Left <String, Int32> ("hello world")));
        }

        [Test]
        public void TestToString () {
            Assert.That (Either.Right <Int32, String> ("hello world").ToString (), Is.EqualTo ("Right (hello world)"));
            Assert.That (Either.Left <Int32, String> (42).ToString (), Is.EqualTo ("Left (42)"));
            Assert.That (Either.Right <Double, Double> (3.14).ToString (), Is.EqualTo ("Right (3.14)"));
            Assert.That (Either.Left <Double, Double> (3.14).ToString (), Is.EqualTo ("Left (3.14)"));
        }

        [Test]
        public void TestMapLeft () {
            Assert.That (Either.Right <Int32, String> ("hello world").MapLeft ((l) => l + 1), Is.EqualTo (Either.Right <Int32, String> ("hello world")));
            Assert.That (Either.Right <Int32, String> ("hello world").MapLeft ((l) => (Double) l), Is.EqualTo (Either.Right <Double, String> ("hello world")));

            Assert.That (Either.Left <Int32, String> (42).MapLeft ((l) => l + 1), Is.EqualTo (Either.Left <Int32, String> (43)));
            Assert.That (Either.Left <Int32, String> (42).MapLeft ((l) => (Double) l), Is.EqualTo (Either.Left <Double, String> (42.0)));
        }

        [Test]
        public void TestMapRight () {
            Assert.That (Either.Right <Int32, String> ("hello world").MapRight ((r) => r.ToUpper ()), Is.EqualTo (Either.Right <Int32, String> ("HELLO WORLD")));
            Assert.That (Either.Right <Int32, String> ("hello world").MapRight ((r) => (Double) r.Length), Is.EqualTo (Either.Right <Int32, Double> (11.0)));

            Assert.That (Either.Left <Int32, String> (42).MapRight ((r) => r.ToUpper ()), Is.EqualTo (Either.Left <Int32, String> (42)));
            Assert.That (Either.Left <Int32, String> (42).MapRight ((r) => (Double) r.Length), Is.EqualTo (Either.Left <Int32, Double> (42)));
        }

        [Test]
        public void TestMap () {
            Assert.That (Either.Right <Int32, String> ("hello world").Map ((l) => l + 1, (r) => r.ToUpper ()), Is.EqualTo (Either.Right <Int32, String> ("HELLO WORLD")));
            Assert.That (Either.Left <Int32, String> (42).Map ((l) => l + 1, (r) => r.ToUpper ()), Is.EqualTo (Either.Left <Int32, String> (43)));

            Assert.That (Either.Right <Int32, String> ("hello world").Map ((l) => (Double) l, (r) => r.Length), Is.EqualTo (Either.Right <Double, Int32> (11)));
            Assert.That (Either.Left <Int32, String> (42).Map ((l) => (Double) l, (r) => r.Length), Is.EqualTo (Either.Left <Double, Int32> (42.0)));
        }

        [Test]
        public void TestMatch () {
            Assert.That (Either.Right <Int32, String> ("hello world").Match ((l) => "#" + (l + 1), (r) => r.ToUpper ()), Is.EqualTo ("HELLO WORLD"));
            Assert.That (Either.Left <Int32, String> (42).Match ((l) => "#" + (l + 1), (r) => r.ToUpper ()), Is.EqualTo ("#43"));
        }

        [Test]
        public void TestGetHashCode () {
            Assert.That (Either.Right <Int32, String> ("hello world").GetHashCode (), Is.EqualTo (Either.Right <Int32, String> ("hello world").GetHashCode ()));
            Assert.That (Either.Left <Int32, String> (42).GetHashCode (), Is.EqualTo (Either.Left <Int32, String> (42).GetHashCode ()));
            Assert.That (Either.Right <Double, Double> (3.14).GetHashCode (), Is.EqualTo (Either.Right <Double, Double> (3.14).GetHashCode ()));
            Assert.That (Either.Left <Double, Double> (3.14).GetHashCode (), Is.EqualTo (Either.Left <Double, Double> (3.14).GetHashCode ()));

            Assert.That (Either.Right <Int32, String> ("hello world").GetHashCode (), Is.Not.EqualTo (Either.Right <Int32, String> ("foobar").GetHashCode ()));
            Assert.That (Either.Left <Int32, String> (42).GetHashCode (), Is.Not.EqualTo (Either.Right <Int32, String> ("hello world").GetHashCode ()));
            Assert.That (Either.Right <Double, Double> (3.14).GetHashCode (), Is.Not.EqualTo (Either.Left <Double, Double> (3.14).GetHashCode ()));
            Assert.That (Either.Left <Double, Double> (3.14).GetHashCode (), Is.Not.EqualTo (Either.Right <Double, Double> (3.14).GetHashCode ()));
        }
    }
}
