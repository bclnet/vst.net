using Jacobi.Vst3.Core;

namespace Jacobi.Vst3.Utility
{
    public class ProcessContextRequirements
    {
        IProcessContextRequirements.Flags flags;

        public ProcessContextRequirements(uint inFlags = 0) => flags = (IProcessContextRequirements.Flags)inFlags;

        public bool WantsNone => flags == 0;
        public bool WantsSystemTime => (flags & IProcessContextRequirements.Flags.NeedSystemTime) != 0;
        public bool WantsContinousTimeSamples => (flags & IProcessContextRequirements.Flags.NeedContinousTimeSamples) != 0;
        public bool WantsProjectTimeMusic => (flags & IProcessContextRequirements.Flags.NeedProjectTimeMusic) != 0;
        public bool WantsBarPositionMusic => (flags & IProcessContextRequirements.Flags.NeedBarPositionMusic) != 0;
        public bool WantsCycleMusic => (flags & IProcessContextRequirements.Flags.NeedCycleMusic) != 0;
        public bool WantsSamplesToNextClock => (flags & IProcessContextRequirements.Flags.NeedSamplesToNextClock) != 0;
        public bool WantsTempo => (flags & IProcessContextRequirements.Flags.NeedTempo) != 0;
        public bool WantsTimeSignature => (flags & IProcessContextRequirements.Flags.NeedTimeSignature) != 0;
        public bool WantsChord => (flags & IProcessContextRequirements.Flags.NeedChord) != 0;
        public bool WantsFrameRate => (flags & IProcessContextRequirements.Flags.NeedFrameRate) != 0;
        public bool WantsTransportState => (flags & IProcessContextRequirements.Flags.NeedTransportState) != 0;

        // set SystemTime as requested
        public ProcessContextRequirements NeedSystemTime() { flags |= IProcessContextRequirements.Flags.NeedSystemTime; return this; }
        // set ContinousTimeSamples as requested
        public ProcessContextRequirements NeedContinousTimeSamples() { flags |= IProcessContextRequirements.Flags.NeedContinousTimeSamples; return this; }
        // set ProjectTimeMusic as requested
        public ProcessContextRequirements NeedProjectTimeMusic() { flags |= IProcessContextRequirements.Flags.NeedProjectTimeMusic; return this; }
        // set BarPositionMusic as needed
        public ProcessContextRequirements NeedBarPositionMusic() { flags |= IProcessContextRequirements.Flags.NeedBarPositionMusic; return this; }
        // set CycleMusic as needed
        public ProcessContextRequirements NeedCycleMusic() { flags |= IProcessContextRequirements.Flags.NeedCycleMusic; return this; }
        // set SamplesToNextClock as needed
        public ProcessContextRequirements NeedSamplesToNextClock() { flags |= IProcessContextRequirements.Flags.NeedSamplesToNextClock; return this; }
        // set Tempo as needed
        public ProcessContextRequirements NeedTempo() { flags |= IProcessContextRequirements.Flags.NeedTempo; return this; }
        // set TimeSignature as needed
        public ProcessContextRequirements NeedTimeSignature() { flags |= IProcessContextRequirements.Flags.NeedTimeSignature; return this; }
        // set Chord as needed
        public ProcessContextRequirements NeedChord() { flags |= IProcessContextRequirements.Flags.NeedChord; return this; }
        // set FrameRate as needed
        public ProcessContextRequirements NeedFrameRate() { flags |= IProcessContextRequirements.Flags.NeedFrameRate; return this; }
        // set TransportState as needed
        public ProcessContextRequirements NeedTransportState() { flags |= IProcessContextRequirements.Flags.NeedTransportState; return this; }
    }
}
