namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// OS-independent enumeration of virtual modifier-codes.
    /// </summary>
    public enum KeyModifier
    {
        ShiftKey = 1 << 0,          // same on both PC and Mac
        AlternateKey = 1 << 1,      // same on both PC and Mac
        CommandKey = 1 << 2,        // windows ctrl key; mac cmd key (apple button)
        ControlKey = 1 << 3         // windows: not assigned, mac: ctrl key
    }
}
