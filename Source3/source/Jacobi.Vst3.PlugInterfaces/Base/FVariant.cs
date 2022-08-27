using System.Runtime.InteropServices;

namespace Jacobi.Vst3
{
    [StructLayout(LayoutKind.Explicit, CharSet = Platform.CharacterSet)]
    public struct FVariant
    {
        public static readonly int Size = Marshal.SizeOf<FVariant>();

        [FieldOffset(FieldOffset_Type)] public VariantType Type;
        [FieldOffset(FieldOffset_Union), MarshalAs(UnmanagedType.I8)] public long IntValue;
        [FieldOffset(FieldOffset_Union), MarshalAs(UnmanagedType.R8)] public double floatValue;
        [FieldOffset(FieldOffset_Union), MarshalAs(UnmanagedType.LPStr)] public string string8;
        [FieldOffset(FieldOffset_Union), MarshalAs(UnmanagedType.LPWStr)] public string string16;
        [FieldOffset(FieldOffset_Union), MarshalAs(UnmanagedType.IUnknown)] public object Obj;

        public enum VariantType
        {
            Empty = 0,
            Integer = 1 << 0,
            Float = 1 << 1,
            String8 = 1 << 2,
            Object = 1 << 3,
            Owner = 1 << 4,
            String16 = 1 << 5
        }

#if X86
        const int FieldOffset_Type = 0;
        const int FieldOffset_Union = 4;
#endif
#if X64
        const int FieldOffset_Type = 0;
        const int FieldOffset_Union = 8;
#endif

    }
}
