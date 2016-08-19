#pragma warning disable 108

namespace Fun {

    using System;
    using System.Linq;
    using System.Collections.Generic;

    public abstract class Try {
        public static Try <T> Apply <T> (Func<T> f) {
            try {
                var r = f ();
                return SuccessTry<T>.Create (r);
            } catch (Exception e) {
                return FailureTry<T>.Create (e);
            }
        }
        public static Try <T> Success <T> (T value) { return SuccessTry<T>.Create (value); }

        public static Try <T> Failure <T> (Exception e) { return FailureTry<T>.Create (e); }

        sealed class FailureTry <T>: Try <T> {
            public static FailureTry <T> Create (Exception value) { return new FailureTry <T> (value); }
            readonly Exception value;
            FailureTry (Exception value) { this.value = value; }

            public override Option <Exception> Failure { get { return Option.Apply <Exception> (value); } }
            public override Option <T> Success { get { return Option.None <T> (); } }

            public override T Flatten (Func <Exception, T> f) { return f (value); }

            public override T2 Match <T2> (Func <T, T2> sf, Func <Exception, T2> ff) { return ff (value); }
        }

        sealed class SuccessTry <T>: Try <T> {
            public static SuccessTry <T> Create (T value) { return new SuccessTry <T> (value); }
            readonly T value;
            SuccessTry (T value) { this.value = value; }

            public override Option <Exception> Failure { get { return Option.None <Exception> (); } }
            public override Option <T> Success { get { return Option.Apply <T> (value); } }

            public override T Flatten (Func <Exception, T> f) { return value; }

            public override T2 Match <T2> (Func <T, T2> sf, Func <Exception, T2> ff) { return sf (value); }
        }
    }

    public abstract class Try <T> : Try, IEquatable <Try <T>> {
        public Boolean HasFailure { get { return Failure.HasValue; } }
        public Boolean HasSuccess { get { return Success.HasValue; } }
        public Option <T> ToOption () { return Success; }

        public abstract Option <Exception> Failure { get; }
        public abstract Option <T> Success { get; }

        public abstract T Flatten (Func <Exception, T> f);

        public Try <T2> Map <T2> (Func <T, T2> f) {
            return this.Match (
                (success) => Try.Success <T2> (f (success)),
                (failure) => Try.Failure <T2> (failure)
            );
        }

        public Try <T2> FlatMap<T2> (Func <T, Try <T2>> f) {
            return this.Match (
                (success) => f (success),
                (failure) => Try.Failure <T2> (failure)
            );
        }

        public abstract T2 Match <T2> (Func <T, T2> sf, Func <Exception, T2> ff);

        public override Boolean Equals (Object obj) {
            return If.Else (
                obj is Try <T>,
                () => this.Equals ((Try <T>) obj),
                () => false);
        }

        public static Boolean Equals (Try <T> v1, Try <T> v2) {
            return If.Else (
                v1.HasFailure && v2.HasFailure, () => v1.Failure == v2.Failure,
                v1.HasSuccess && v2.HasSuccess, () => v1.Success == v2.Success,
                () => false);
        }

        public Boolean Equals (Try <T> that) { return Equals (this, that); }
        public static Boolean operator == (Try <T> v1, Try <T> v2) { return Equals (v1, v2); }
        public static Boolean operator != (Try <T> v1, Try <T> v2) { return !Equals (v1, v2); }
        public override Int32 GetHashCode () {
            return Match (
                (success) => success.GetHashCode ().ShiftAndWrap (2),
                (failure) => failure.GetHashCode ());
        }
        public override String ToString () {
            return Match (
                (success) => String.Format ("Success ({0})", success.ToString ()),
                (failure) => String.Format ("Failure ({0}: {1})", failure.GetType ().Name, failure.Message));
        }
    }
}