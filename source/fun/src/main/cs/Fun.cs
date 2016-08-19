namespace Fun {

    using System;

    public static class Fun {

        public static Unit Unit { get { return Unit.Value; } }
        
        public static Option <T> Some <T> (T value) { return Option.Apply <T> (value); }
        public static Option <T> Some <T> (T? value) where T: struct { return Option.Apply <T> (value); }
        public static Option <T> None <T> () { return Option.None <T> (); }

        public static Either <L, R> Left <L, R> (L left) { return Either.Left <L, R> (left); }
        public static Either <L, R> Right <L, R> (R right) { return Either.Right <L, R> (right); }

        public static Try <T> Try <T> (Func<T> f) { return Try.Apply <T> (f); }
        public static Try <T> Success <T> (T value) { return Try.Success <T> (value); }
        public static Try <T> Failure <T> (Exception e) { return Try.Failure <T> (e); }
    }
}