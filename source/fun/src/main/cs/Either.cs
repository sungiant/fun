#pragma warning disable 108

namespace Fun {

    using System;
    using System.Linq;
    using System.Collections.Generic;

    public abstract class Either {
        public static Either <L, R> Left <L, R> (L left) { return LeftEither <L, R>.Apply (left); }
        public static Either <L, R> Right <L, R> (R right) { return RightEither <L, R>.Apply (right); }

        sealed class LeftEither <L, R>: Either <L, R> {
            public static LeftEither <L, R> Apply (L left) { return new LeftEither <L, R> (left); }
            readonly L value;
            LeftEither (L left) { value = left; }

            public override Option <L> Left { get { return Option.Apply <L> (value); } }
            public override Option <R> Right { get { return Option.None <R> (); } }

            public override Either <L2, R> MapLeft <L2> (Func <L, L2> f) { return new LeftEither<L2, R> (f (value)); }
            public override Either <L, R2> MapRight <R2> (Func <R, R2> f) { return new LeftEither<L, R2> (value); }

            public override T Match <T> (Func <L, T> lf, Func <R, T> rf) { return lf (value); }
        }

        sealed class RightEither <L, R>: Either <L, R> {
            public static RightEither <L, R> Apply (R right) { return new RightEither <L, R> (right); }
            readonly R value;
            RightEither (R right) { value = right; }

            public override Option <L> Left { get { return Option.None <L> (); } }
            public override Option <R> Right { get { return Option.Apply <R> (value); } }

            public override Either <L2, R> MapLeft <L2> (Func <L, L2> f) { return new RightEither<L2, R> (value); }
            public override Either <L, R2> MapRight <R2> (Func <R, R2> f) { return new RightEither<L, R2> (f (value)); }

            public override T Match <T> (Func <L, T> lf, Func <R, T> rf) { return rf (value); }
        }
    }

    public abstract class Either <L, R> : Either, IEquatable <Either <L, R>> {
        public Boolean HasLeft { get { return Left.HasValue; } }
        public Boolean HasRight { get { return Right.HasValue; } }

        public abstract Option <L> Left { get; }
        public abstract Option <R> Right { get; }

        public abstract Either <L2, R> MapLeft <L2> (Func <L, L2> f);
        public abstract Either <L, R2> MapRight <R2> (Func <R, R2> f);

        public Either <L2, R2> Map <L2, R2> (Func <L, L2> lf, Func <R, R2> rf) {
            return If.Else (
                HasLeft,
                () => MapLeft (lf).MapRight (rf),
                () => MapRight (rf).MapLeft (lf));
        }

        public abstract T Match <T> (Func <L, T> lf, Func <R, T> rf);

        public override Boolean Equals (Object obj) {
            return If.Else (
                obj is Either <L, R>,
                () => this.Equals ((Either <L, R>) obj),
                () => false);
        }

        public static Boolean Equals (Either <L, R> v1, Either <L, R> v2) {
            return If.Else (
                v1.HasLeft && v2.HasLeft, () => v1.Left == v2.Left,
                v1.HasRight && v2.HasRight, () => v1.Right == v2.Right,
                () => false);
        }

        public Boolean Equals (Either <L, R> that) { return Equals (this, that); }
        public static Boolean operator == (Either <L, R> v1, Either <L, R> v2) { return Equals (v1, v2); }
        public static Boolean operator != (Either <L, R> v1, Either <L, R> v2) { return !Equals (v1, v2); }
        public override Int32 GetHashCode () {
            return Match (
                (l) => l.GetHashCode (),
                (r) => r.GetHashCode ().ShiftAndWrap (2));
        }
        public override String ToString () {
            return Match (
                (l) => String.Format ("Left ({0})", l.ToString ()),
                (r) => String.Format ("Right ({0})", r.ToString ()));
        }
    }
}