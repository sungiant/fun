#pragma warning disable 108

namespace Fun {

    using System;
    using System.Linq;
    using System.Collections.Generic;

    public abstract class Option {
        public static Option <T> Apply <T> (T value) {
            return If.Else<Option <T>> (
                !typeof(T).IsValueType && value == null,
                () => OptionNone<T>.None,
                () => OptionSome <T>.Create (value));
        }

        public static Option <T> Apply <T> (T? value) where T: struct {
            return If.Else<Option <T>> (
                value.HasValue,
                () => OptionSome <T>.Create (value.Value),
                () => OptionNone<T>.None);
        }

        public static Option <T> None <T> () { return OptionNone <T>.None; }

        public sealed class ValueNotDefinedException: Exception {}

        sealed class OptionSome <T>: Option <T> {
            public static OptionSome <T> Create (T some) { return new OptionSome <T> (some); }
            readonly T value;
            OptionSome (T some) { value = some; }
            public override Boolean HasValue { get { return true; } }
            public override T ValueOrElse (Func<T> fallback) { return value; }
            public override Option<T2> Map <T2> (Func <T, T2> f) { return Option.Apply <T2> (f (value)); }
            public override T2 Match <T2> (Func <T, T2> onSome, Func <T2> onNone) { return onSome (value); }
        }

        sealed class OptionNone <T>: Option <T> {
            public static readonly OptionNone <T> None = new OptionNone <T> ();
            OptionNone () {}
            public override Boolean HasValue { get { return false; } }
            public override T ValueOrElse (Func<T> fallback) { return fallback (); }
            public override Option<T2> Map <T2> (Func <T, T2> f) { return Option.None <T2> (); }
            public override T2 Match <T2> (Func <T, T2> onSome, Func <T2> onNone) { return onNone (); }
        }
    }

    public abstract class Option <T>: Option, IEquatable <Option <T>> {
        public abstract Boolean HasValue { get; }

        public T ValueOrElse (T fallback) { return ValueOrElse (() => fallback); }
        public abstract T ValueOrElse (Func<T> fallback);
        public abstract Option<T2> Map <T2> (Func <T, T2> f);
        public abstract T2 Match <T2> (Func <T, T2> onSome, Func <T2> onNone);

        static T ValueNotDefined () { throw new ValueNotDefinedException (); }

        public T ValueUnsafe { get { return ValueOrElse (ValueNotDefined); } }

        public Option<T2> FlatMap <T2> (Func <T, Option<T2>> f) {
            Option<Option<T2>> x = this.Map (f);
            return If.Else (
                x.HasValue && x.ValueOrElse (Option.None <T2> ()).HasValue,
                () => Option.Apply <T2> (x.ValueOrElse (Option.None <T2> ()).ValueUnsafe),
                () => Option.None <T2> ());
        }

        public override Boolean Equals (Object obj) {
            return If.Else (
                obj is Option <T>,
                () => this.Equals ((Option <T>) obj),
                () => false);
        }
        public static Boolean Equals (Option <T> v1, Option <T> v2) {
            return If.Else (
                !v1.HasValue && !v2.HasValue, () => true,
                v1.HasValue && v2.HasValue, () => v1.ValueUnsafe.Equals (v2.ValueUnsafe),
                () => false);
        }

        public Boolean Equals (Option<T> that) { return Equals (this, that); }
        public static Boolean operator == (Option <T> v1, Option <T> v2) { return Equals (v1, v2); }
        public static Boolean operator != (Option <T> v1, Option <T> v2) { return !Equals (v1, v2); }
        public override Int32 GetHashCode () {
            return Match (
                (v) => v.GetHashCode (),
                ()  => typeof (T).GetHashCode ());
        }
        public override String ToString () {
            return Match (
                (v) => String.Format ("Some ({0})", v.ToString ()),
                ()  => "None");
        }
    }
}