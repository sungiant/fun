namespace Fun {

    using System;
    
    public struct Unit {
        public static Unit Value = new Unit ();
        public override Boolean Equals (Object obj) {
            var local = this;
            return If.Else (
                obj is Unit,
                () => local.Equals ((Unit) obj),
                () => false);
        }
        public static Boolean Equals (Unit v1, Unit v2) { return true; }
        public Boolean Equals (Unit that) { return true; }
        public static Boolean operator == (Unit v1, Unit v2) { return true; }
        public static Boolean operator != (Unit v1, Unit v2) { return false; }
        public override Int32 GetHashCode () { return 0; }
        public override String ToString () { return "()"; }
    }
}