namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Component Flags used as classFlags in PClassInfo2
    /// </summary>
    public enum ComponentFlags
    {
        //None = 0,
        Distributable = 1 << 0,	        // Component can be run on remote computer
        SimpleModeSupported = 1 << 1	// Component supports simple IO mode (or works in simple mode anyway) see \ref vst3IoMode
    }
}
