using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    [ComImport, Guid(Interfaces.IProcessContextRequirements), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IProcessContextRequirements
    {
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        UInt32 GetProcessContextRequirements();

		public enum Flags
		{
			NeedSystemTime = 1 << 0, // kSystemTimeValid
			NeedContinousTimeSamples = 1 << 1, // kContTimeValid
			NeedProjectTimeMusic = 1 << 2, // kProjectTimeMusicValid
			NeedBarPositionMusic = 1 << 3, // kBarPositionValid
			NeedCycleMusic = 1 << 4, // kCycleValid
			NeedSamplesToNextClock = 1 << 5, // kClockValid
			NeedTempo = 1 << 6, // kTempoValid
			NeedTimeSignature = 1 << 7, // kTimeSigValid
			NeedChord = 1 << 8, // kChordValid
			NeedFrameRate = 1 << 9, // kSmpteValid
			NeedTransportState = 1 << 10, // kPlaying, kCycleActive, kRecording
		}
	}

	static partial class Interfaces
	{
        public const string IProcessContextRequirements = "2A654303-EF76-4E3D-95B5-FE83730EF6D0";
	}
}
