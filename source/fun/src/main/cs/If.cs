namespace Fun {

    using System;
    
    public static class If {
        public static T Else<T> (
            Boolean condition, Func <T> onConditionTrue, Func<T> onElse) {
            if (condition) { return onConditionTrue (); }
            else { return onElse (); }
        }

        public static T Else<T> (Boolean c1, Func <T> onC1True, Boolean c2, Func <T> onC2True,Func<T> onElse) {
            if (c1) { return onC1True (); }
            else if (c2) { return onC2True (); }
            else { return onElse (); }
        }

        public static T Else<T> (Boolean c1, Func <T> onC1True, Boolean c2, Func <T> onC2True,
            Boolean c3, Func <T> onC3True, Func<T> onElse) {
            if (c1) { return onC1True (); }
            else if (c2) { return onC2True (); }
            else if (c3) { return onC3True (); }
            else { return onElse (); }
        }

        public static T Else<T> (Boolean c1, Func <T> onC1True, Boolean c2, Func <T> onC2True,
            Boolean c3, Func <T> onC3True, Boolean c4, Func <T> onC4True, Func<T> onElse) {
            if (c1) { return onC1True (); }
            else if (c2) { return onC2True (); }
            else if (c3) { return onC3True (); }
            else if (c4) { return onC4True (); }
            else { return onElse (); }
        }

        public static T Else<T> (Boolean c1, Func <T> onC1True, Boolean c2, Func <T> onC2True,
            Boolean c3, Func <T> onC3True, Boolean c4, Func <T> onC4True,
            Boolean c5, Func <T> onC5True, Func<T> onElse) {
            if (c1) { return onC1True (); }
            else if (c2) { return onC2True (); }
            else if (c3) { return onC3True (); }
            else if (c4) { return onC4True (); }
            else if (c5) { return onC5True (); }
            else { return onElse (); }
        }
    }
}