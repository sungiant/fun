namespace Fun.Tests {

    using System;
    using NUnit.Framework;

    [TestFixture]
    public class OptionTests {
        readonly Int32? a = null;
        readonly Int32? b = 42;
        readonly Int32  c = 99;
        readonly String d = "hello world";
        readonly String e = null;

        [Test]
        public void TestApply () {
            Assert.That (Option.Apply (a), Is.Not.EqualTo (null));
            Assert.That (Option.Apply (b), Is.Not.EqualTo (null));
            Assert.That (Option.Apply (c), Is.Not.EqualTo (null));
            Assert.That (Option.Apply (d), Is.Not.EqualTo (null));
            Assert.That (Option.Apply (e), Is.Not.EqualTo (null));

            Assert.That (Option.Apply (a).HasValue, Is.EqualTo (false));
            Assert.That (Option.Apply (b).HasValue, Is.EqualTo (true));
            Assert.That (Option.Apply (c).HasValue, Is.EqualTo (true));
            Assert.That (Option.Apply (d).HasValue, Is.EqualTo (true));
            Assert.That (Option.Apply (e).HasValue, Is.EqualTo (false));
        }

        [Test]
        public void TestNone () {
            Assert.That (Option.None <Double> (), Is.Not.EqualTo (null));
            Assert.That (Option.Apply (a), Is.EqualTo (Option.None <Int32>()));
            Assert.That (Option.Apply (e), Is.EqualTo (Option.None <String>()));
        }
        
        [Test]
        public void TestEquality () {
            Assert.That (Option.Apply (a), Is.EqualTo (Option.None <Int32> ()));
            Assert.That (Option.Apply (b), Is.EqualTo (Option.Apply (42)));
            Assert.That (Option.Apply (c), Is.EqualTo (Option.Apply (99)));
            Assert.That (Option.Apply (d), Is.EqualTo (Option.Apply ("hello world")));
            Assert.That (Option.Apply (e), Is.EqualTo (Option.None <String> ()));
            Assert.That (Option.None <Double> (), Is.EqualTo (Option.None <Double> ()));

            Assert.That (Option.Apply (a), Is.Not.EqualTo (Option.Apply (b)));
            Assert.That (Option.Apply (b), Is.Not.EqualTo (Option.Apply (c)));
            Assert.That (Option.Apply (c), Is.Not.EqualTo (Option.Apply (a)));
            Assert.That (Option.Apply (d), Is.Not.EqualTo (Option.Apply (e)));
            Assert.That (Option.Apply (e), Is.Not.EqualTo (Option.Apply (d)));


            Assert.That (Option.Apply (a) == Option.Apply (a));
            Assert.That (Option.Apply (b) == Option.Apply (b));
            Assert.That (Option.Apply (c) == Option.Apply (c));
            Assert.That (Option.Apply (d) == Option.Apply (d));
            Assert.That (Option.Apply (e) == Option.Apply (e));

            Assert.That (Option.Apply (a) != Option.Apply (b));
            Assert.That (Option.Apply (b) != Option.Apply (c));
            Assert.That (Option.Apply (c) != Option.Apply (a));
            Assert.That (Option.Apply (d) != Option.Apply (e));
            Assert.That (Option.Apply (e) != Option.Apply (d));
        }

        [Test]
        public void TestToString () {
            Assert.That (Option.Apply (a).ToString (), Is.EqualTo ("None"));
            Assert.That (Option.Apply (b).ToString (), Is.EqualTo ("Some (42)"));
            Assert.That (Option.Apply (c).ToString (), Is.EqualTo ("Some (99)"));
            Assert.That (Option.Apply (d).ToString (), Is.EqualTo ("Some (hello world)"));
            Assert.That (Option.Apply (e).ToString (), Is.EqualTo ("None"));
            Assert.That (Option.None <Double> ().ToString (), Is.EqualTo ("None"));
        }

        [Test]
        public void TestValueUnsafe () {
            Assert.Throws<Option.ValueNotDefinedException> (() => { var x = Option.Apply (a).ValueUnsafe; });
            Assert.That (Option.Apply (b).ValueUnsafe, Is.EqualTo (42));
            Assert.That (Option.Apply (c).ValueUnsafe, Is.EqualTo (99));
            Assert.That (Option.Apply (d).ValueUnsafe, Is.EqualTo ("hello world"));
            Assert.Throws<Option.ValueNotDefinedException> (() => { var x = Option.Apply (e).ValueUnsafe; });
            Assert.Throws<Option.ValueNotDefinedException> (() => { var x = Option.None <Double> ().ValueUnsafe; });
        }

        [Test]
        public void TestValueOrElse () {
            Assert.That (Option.Apply (a).ValueOrElse (72), Is.EqualTo (72));
            Assert.That (Option.Apply (b).ValueOrElse (72), Is.EqualTo (42));
            Assert.That (Option.Apply (c).ValueOrElse (72), Is.EqualTo (99));
            Assert.That (Option.Apply (d).ValueOrElse ("foobar"), Is.EqualTo ("hello world"));
            Assert.That (Option.Apply (e).ValueOrElse ("foobar"), Is.EqualTo ("foobar"));
            Assert.That (Option.None <Double> ().ValueOrElse (3.14), Is.EqualTo (3.14));

            Assert.That (Option.Apply (a).ValueOrElse (() => 72), Is.EqualTo (72));
            Assert.That (Option.Apply (b).ValueOrElse (() => 72), Is.EqualTo (42));
            Assert.That (Option.Apply (c).ValueOrElse (() => 72), Is.EqualTo (99));
            Assert.That (Option.Apply (d).ValueOrElse (() => "foobar"), Is.EqualTo ("hello world"));
            Assert.That (Option.Apply (e).ValueOrElse (() => "foobar"), Is.EqualTo ("foobar"));
            Assert.That (Option.None <Double> ().ValueOrElse (() => 3.14), Is.EqualTo (3.14));
        }

        [Test]
        public void TestMap () {
            Assert.That (Option.Apply (a).Map ((x) => x + 1), Is.EqualTo (Option.Apply (a)));
            Assert.That (Option.Apply (b).Map ((x) => x + 1), Is.EqualTo (Option.Apply (43)));
            Assert.That (Option.Apply (c).Map ((x) => x + 1), Is.EqualTo (Option.Apply (100)));
            Assert.That (Option.Apply (d).Map ((x) => x.ToUpper ()), Is.EqualTo (Option.Apply ("HELLO WORLD")));
            Assert.That (Option.Apply (e).Map ((x) => x.ToUpper ()), Is.EqualTo (Option.Apply (e)));
            Assert.That (Option.None <Double> ().Map ((x) => x * 2.0), Is.EqualTo (Option.None <Double> ()));
            Assert.That (Option.None <Double> ().Map ((x) => "foobar"), Is.EqualTo (Option.None <String> ()));
            Assert.That (Option.Apply <Double> (3.14).Map ((x) => "Pi: " + x), Is.EqualTo (Option.Apply <String> ("Pi: 3.14")));
        }

        [Test]
        public void TestFlatMap () {
            Assert.That (Option.Apply (a).FlatMap ((x) => Option.Apply (x + 1)), Is.EqualTo (Option.None<Int32> ()));
            Assert.That (Option.Apply (b).FlatMap ((x) => Option.Apply (x + 1)), Is.EqualTo (Option.Apply (43)));
            Assert.That (Option.Apply (c).FlatMap ((x) => Option.Apply (x + 1)), Is.EqualTo (Option.Apply (100)));
            Assert.That (Option.Apply (d).FlatMap ((x) => Option.Apply (x.ToUpper ())), Is.EqualTo (Option.Apply ("HELLO WORLD")));
            Assert.That (Option.Apply (e).FlatMap ((x) => Option.Apply (x.ToUpper ())), Is.EqualTo (Option.None <String> ()));
            Assert.That (Option.Apply (3.14).FlatMap ((x) => Option.None <Double> ()), Is.EqualTo (Option.None <Double> ()));
            Assert.That (Option.None <Double> ().FlatMap ((x) => Option.None <Double> ()), Is.EqualTo (Option.None <Double> ()));

            var oh = Option.Apply ("hello");
            var ow = Option.Apply ("world");

            Assert.That (oh.FlatMap ((h) => ow.Map ((w) => h + " " + w)), Is.EqualTo (Option.Apply (d)));
            Assert.That (oh.FlatMap ((h) => ow.Map ((w) => h.Length + w.Length)), Is.EqualTo (Option.Apply (10)));
        }

        [Test]
        public void TestMatch () {
            Assert.That (Option.Apply (a).Match ((x) => x + 1, () => 72), Is.EqualTo (72));
            Assert.That (Option.Apply (b).Match ((x) => x + 1, () => 72), Is.EqualTo (43));
            Assert.That (Option.Apply (c).Match ((x) => x + 1, () => 72), Is.EqualTo (100));
            Assert.That (Option.Apply (d).Match ((x) => x.ToUpper (), () => "foobar"), Is.EqualTo ("HELLO WORLD"));
            Assert.That (Option.Apply (e).Match ((x) => x.ToUpper (), () => "foobar"), Is.EqualTo ("foobar"));
            Assert.That (Option.Apply (3.14).Match ((x) => "hello", () => "world"), Is.EqualTo ("hello"));
            Assert.That (Option.None <Double> ().Match ((x) => "hello", () => "world"), Is.EqualTo ("world"));
        }

        [Test]
        public void TestGetHashCode () {
            Assert.That (Option.None <Double> ().GetHashCode (), Is.Not.EqualTo (Option.None <Int32> ().GetHashCode ()));
            Assert.That (Option.None <Double> ().GetHashCode (), Is.EqualTo (Option.None <Double> ().GetHashCode ()));
        }
    }
}
