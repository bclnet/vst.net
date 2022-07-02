using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Frame Rate 
    /// A frame rate describes the number of image(frame) displayed per second.
    /// Some examples:
	/// - 23.976 fps     is framesPerSecond: 24 and flags: kPullDownRate
	/// - 24 fps         is framesPerSecond: 24 and flags: 0
	/// - 25 fps         is framesPerSecond: 25 and flags: 0
	/// - 29.97 drop fps is framesPerSecond: 30 and flags: kDropRate|kPullDownRate
	/// - 29.97 fps      is framesPerSecond: 30 and flags: kPullDownRate
	/// - 30 fps         is framesPerSecond: 30 and flags: 0
	/// - 30 drop fps    is framesPerSecond: 30 and flags: kDropRate
	/// - 50 fps         is framesPerSecond: 50 and flags: 0
	/// - 59.94 fps	     is framesPerSecond: 60 and flags: kPullDownRate
	/// - 60 fps         is framesPerSecond: 60 and flags: 0
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = Platform.CharacterSet, Pack = Platform.StructurePack)]
    public struct FrameRate
    {
        [MarshalAs(UnmanagedType.U4)] public UInt32 framesPerSecond;	// frame rate
        [MarshalAs(UnmanagedType.U4)] public FrameRateFlags flags;    	// flags #FrameRateFlags

        public enum FrameRateFlags
        {
            PullDownRate = 1 << 0,      // for ex. HDTV: 23.976 fps with 24 as frame rate
            DropRate = 1 << 1	        // for ex. 29.97 fps drop with 30 as frame rate
        }
    }
}
