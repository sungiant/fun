namespace Fun.Tests {

    using System;
    using NUnit.Framework;

    [TestFixture]
    public class TryTests {
        readonly Exception exception = new ArgumentOutOfRangeException ();

        Double Sqrt (Double x) {
            if (x < 0.0) { throw exception; }
            return Math.Sqrt (x);
        }

        [Test]
        public void TestApply () {
            var ta = Try.Apply (() => Sqrt (9.0));
            var tb = Try.Apply (() => Sqrt (-9.0));

            Assert.That (ta, Is.EqualTo (Try.Success (3.0)));
            Assert.That (tb, Is.EqualTo (Try.Failure <Double>(exception)));
        }

        [Test]
        public void TestToOption () {
            Assert.That (Try.Apply (() => Sqrt (9.0)).ToOption, Is.EqualTo (Option.Apply (3.0)));
            Assert.That (Try.Apply (() => Sqrt (-9.0)).ToOption, Is.EqualTo (Option.None<Double> ()));
        }

        [Test]
        public void TestSuccess () {
            var t = Try.Apply (() => Sqrt (9.0));

            Assert.That (t.HasSuccess);
            Assert.That (!t.HasFailure);

            t.Success.Match<Unit> (
                (x) => { Assert.That (x, Is.EqualTo (3.0)); return Unit.Value; },
                ()  => { Assert.Fail (); return Unit.Value; });

            t.Failure.Match<Unit> (
                (e) => { Assert.Fail (); return Unit.Value; },
                ()  => { Assert.Pass (); return Unit.Value; });
        }
        
        [Test]
        public void TestFailure () {
            var t = Try.Apply (() => Sqrt (-9.0));

            Assert.That (t.HasFailure);
            Assert.That (!t.HasSuccess);

            t.Success.Match<Unit> (
                (x) => { Assert.Fail (); return Unit.Value; },
                ()  => { Assert.Pass (); return Unit.Value; });

            t.Failure.Match<Unit> (
                (e) => { Assert.That (e, Is.EqualTo (exception)); return Unit.Value; },
                ()  => { Assert.Fail (); return Unit.Value; });
        }
        
        [Test]
        public void TestEquality () {
            Assert.That (Try.Apply (() => Sqrt (9.0)), Is.EqualTo (Try.Apply (() => Sqrt (9.0))));
            Assert.That (Try.Apply (() => Sqrt (-9.0)), Is.EqualTo (Try.Apply (() => Sqrt (-9.0))));
            Assert.That (Try.Apply (() => Sqrt (9.0)), Is.Not.EqualTo (Try.Apply (() => Sqrt (-9.0))));
            Assert.That (Try.Apply (() => Sqrt (9.0)), Is.Not.EqualTo (Try.Apply (() => Sqrt (16.0))));
            Assert.That (Try.Apply (() => Sqrt (-9.0)), Is.Not.EqualTo (Try.Apply (() => Sqrt (9.0))));
            Assert.That (Try.Apply (() => Sqrt (9.0)) == Try.Apply (() => Sqrt (9.0)));
            Assert.That (Try.Apply (() => Sqrt (-9.0)) == Try.Apply (() => Sqrt (-9.0)));
            Assert.That (Try.Apply (() => Sqrt (9.0)) != Try.Apply (() => Sqrt (-9.0)));
            Assert.That (Try.Apply (() => Sqrt (9.0)) != Try.Apply (() => Sqrt (16.0)));
            Assert.That (Try.Apply (() => Sqrt (-9.0)) != Try.Apply (() => Sqrt (9.0)));
        }

        [Test]
        public void TestToString () {
            Assert.That (Try.Apply (() => Sqrt (9.0)).ToString (), Is.EqualTo ("Success (3)"));
            Assert.That (Try.Apply (() => Sqrt (-9.0)).ToString (), Is.EqualTo ("Failure (ArgumentOutOfRangeException: Specified argument was out of the range of valid values.)"));
        }

        [Test]
        public void TestFlatten () {
            Assert.That (Try.Apply (() => Sqrt (9.0)).Flatten ((e) => 3.14), Is.EqualTo (3.0));
            Assert.That (Try.Apply (() => Sqrt (-9.0)).Flatten ((e) => 3.14), Is.EqualTo (3.14));
        }

        [Test]
        public void TestMap () {
            Assert.That (Try.Apply (() => Sqrt (9.0)).Map ((x) => x * 2.0), Is.EqualTo (Try.Success (6.0)));
            Assert.That (Try.Apply (() => Sqrt (-9.0)).Map ((x) => x * 2.0), Is.EqualTo (Try.Failure <Double>(exception)));

            Assert.That (Try.Apply (() => Sqrt (9.0)).Map ((x) => "foobar"), Is.EqualTo (Try.Success ("foobar")));
            Assert.That (Try.Apply (() => Sqrt (-9.0)).Map ((x) => "foobar"), Is.EqualTo (Try.Failure <String>(exception)));
        }

        [Test]
        public void TestFlatMap () {
            var ta = Try.Apply (() => Sqrt (9.0));
            var tb = Try.Apply (() => Sqrt (16.0));
            var tc = Try.Apply (() => Sqrt (-9.0));

            Assert.That (ta.FlatMap ((a) => tb.Map ((b) => a + b)), Is.EqualTo (Try.Success (7.0)));
            Assert.That (tc.FlatMap ((c) => tb.Map ((b) => c + b)), Is.EqualTo (Try.Failure <Double>(exception)));
            Assert.That (ta.FlatMap ((a) => tc.Map ((c) => a + c)), Is.EqualTo (Try.Failure <Double>(exception)));
            Assert.That (tc.FlatMap ((c1) => tc.Map ((c2) => c1 + c2)), Is.EqualTo (Try.Failure <Double>(exception)));

            Assert.That (ta.FlatMap ((a) => tb.Map ((b) => a.ToString () + " " + b.ToString ())), Is.EqualTo (Try.Success <String>("3 4")));
            Assert.That (tc.FlatMap ((c1) => tc.Map ((c2) => c1.ToString () + " " + c2.ToString ())), Is.EqualTo (Try.Failure <String>(exception)));
        }

        [Test]
        public void TestMatch () {
            Assert.That (Try.Apply (() => Sqrt (9.0)).Match ((s) => s + 0.14, (f) => 99.0), Is.EqualTo (3.14));
            Assert.That (Try.Apply (() => Sqrt (-9.0)).Match ((s) => s + 0.14, (f) => 99.0), Is.EqualTo (99.0));
        }

        [Test]
        public void TestGetHashCode () {
            Assert.That (Try.Apply (() => Sqrt (9.0)).GetHashCode (), Is.Not.EqualTo (Try.Apply (() => Sqrt (-9.0)).GetHashCode ()));
            Assert.That (Try.Apply (() => Sqrt (9.0)).GetHashCode (), Is.EqualTo (Try.Apply (() => Sqrt (9.0)).GetHashCode ()));
            Assert.That (Try.Apply (() => Sqrt (-9.0)).GetHashCode (), Is.EqualTo (Try.Apply (() => Sqrt (-9.0)).GetHashCode ()));
        }
    }
}
