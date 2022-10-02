using System.Runtime.CompilerServices;
using ParamValue = System.Double;

namespace Steinberg.Vst3
{
    unsafe static partial class Helpers
    {
        /// <summary>
        /// bound a value between a min and max
        /// </summary>
        /// <param name="minval"></param>
        /// <param name="maxval"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte BoundTo(byte minval, byte maxval, byte x)
            => x < minval ? minval : x > maxval ? maxval : x;
        /// <summary>
        /// bound a value between a min and max
        /// </summary>
        /// <param name="minval"></param>
        /// <param name="maxval"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float BoundTo(float minval, float maxval, float x)
            => x < minval ? minval : x > maxval ? maxval : x;
        /// <summary>
        /// bound a value between a min and max
        /// </summary>
        /// <param name="minval"></param>
        /// <param name="maxval"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double BoundTo(double minval, double maxval, double x)
            => x < minval ? minval : x > maxval ? maxval : x;

        /// <summary>
        /// Initialized a Event
        /// </summary>
        /// <param name="evnt"></param>
        /// <param name="type"></param>
        /// <param name="busIndex"></param>
        /// <param name="sampleOffset"></param>
        /// <param name="ppqPosition"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref Event Init(ref Event evnt, Event.EventTypes type, int busIndex = 0, int sampleOffset = 0, double ppqPosition = 0.0, Event.EventFlags flags = 0)
        {
            evnt.BusIndex = busIndex;
            evnt.SampleOffset = sampleOffset;
            evnt.PpqPosition = ppqPosition;
            evnt.Flags = flags;
            evnt.Type = type;
            return ref evnt;
        }

        /// <summary>
        /// Returns normalized value of a LegacyMIDICCOutEvent value [0, 127]
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ParamValue GetMIDINormValue(byte value)
            => value >= 127
                ? 1.0
                : (ParamValue)value / 127;

        /// <summary>
        /// Returns LegacyMIDICCOut value [0, 127] from a normalized value [0., 1.]
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte GetMIDICCOutValue(ParamValue value)
            => BoundTo(0, 127, (byte)(value * 127 + 0.5));

        /// <summary>
        /// Returns pitchbend value from a PitchBend LegacyMIDICCOut Event
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static short GetPitchBendValue(LegacyMIDICCOutEvent e)
            => (short)((e.Value & 0x7F) | ((e.Value2 & 0x7F) << 7));

        /// <summary>
        /// Set a normalized pitchbend value to a LegacyMIDICCOut Event
        /// </summary>
        /// <param name="e"></param>
        /// <param name="value"></param>
        public static void SetPitchBendValue(LegacyMIDICCOutEvent e, ParamValue value)
        {
            var tmp = (short)(value * 0x3FFF);
            e.Value = (sbyte)(tmp & 0x7F);
            e.Value2 = (sbyte)((tmp >> 7) & 0x7F);
        }

        /// <summary>
        /// Returns normalized pitchbend value from a PitchBend LegacyMIDICCOut Event
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static float GetNormPitchBendValue(LegacyMIDICCOutEvent e)
        {
            var val = (float)GetPitchBendValue(e) / (float)0x3FFF;
            return val < 0 ? 0
                : val > 1 ? 1
                : val;
        }

        /// <summary>
        /// Initialized a LegacyMIDICCOutEvent
        /// </summary>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <param name="controlNumber"></param>
        /// <param name="channel"></param>
        /// <param name="value"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
        public static LegacyMIDICCOutEvent InitLegacyMIDICCOutEvent(ref Event evnt, byte controlNumber, byte channel = 0, sbyte value = 0, sbyte value2 = 0)
        {
            Init(ref evnt, Event.EventTypes.LegacyMIDICCOutEvent);
            evnt.MidiCCOut.Channel = (sbyte)channel;
            evnt.MidiCCOut.ControlNumber = controlNumber;
            evnt.MidiCCOut.Value = value;
            evnt.MidiCCOut.Value2 = value2;
            return evnt.MidiCCOut;
        }
    }
}
