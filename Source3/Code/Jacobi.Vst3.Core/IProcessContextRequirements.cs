using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
	/// <summary>
	/// Extended IAudioProcessor interface for a component: Vst::IProcessContextRequirements
	/// To get accurate process context information (Vst::ProcessContext), it is now required to implement this interface and
	/// return the desired bit mask of flags which your audio effect needs. If you do not implement this
	/// interface, you may not get any information at all of the process function.
	/// 
	/// The host asks for this information once between initialize and setActive. It cannot be changed afterwards.
	/// 
	/// This gives the host the opportunity to better optimize the audio process graph when it knows which
	/// plug-ins need which information.
	/// 
	/// Plug-Ins built with an earlier SDK version (< 3.7) will still get the old information, but the information
	/// may not be as accurate as when using this interface.
	/// </summary>
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
