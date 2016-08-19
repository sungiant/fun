namespace Fun {

    using System;

    public static class Extensions {
        public static U Apply <T, U> (this T value, Func <T, U> f) { return f (value); }
    }

    internal static class InternalExtensions {
        // http://msdn.microsoft.com/en-us/library/system.object.gethashcode(v=vs.110).aspx
        internal static Int32 ShiftAndWrap (this Int32 value, Int32 positions = 2) {
            positions = positions & 0x1F;
            // Save the existing bit pattern, but interpret it as an unsigned integer.
            UInt32 number = BitConverter.ToUInt32 (BitConverter.GetBytes (value), 0);
            // Preserve the bits to be discarded.
            UInt32 wrapped = number >> (32 - positions);
            // Shift and wrap the discarded bits.
            return BitConverter.ToInt32 (BitConverter.GetBytes ((number << positions) | wrapped), 0);
        }
    }
}