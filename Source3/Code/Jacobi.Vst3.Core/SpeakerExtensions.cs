using System.Runtime.CompilerServices;
using static Jacobi.Vst3.Core.SpeakerArrangement;
using static Jacobi.Vst3.Core.Speaker;
using static Jacobi.Vst3.Core.SpeakerArrangementString;

namespace Jacobi.Vst3.Core
{
    public static class SpeakerArrangementExtensions
    {
        /// <summary>
        /// Returns number of channels used in speaker arrangement. ingroup speakerArrangements
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetChannelCount(this SpeakerArrangement source)
        {
            var arr = (ulong)source;
            var count = 0;
            while (arr != 0)
            {
                if ((arr & 1) != 0) ++count;
                arr >>= 1;
            }
            return count;
        }

        /// <summary>
        /// Returns the index of a given speaker in a speaker arrangement (-1 if speaker is not part of the arrangement)
        /// </summary>
        /// <param name="speaker"></param>
        /// <param name="arrangement"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetSpeakerIndex(this Speaker source, SpeakerArrangement arrangement)
        {
            var speaker = (ulong)source;
            // check if speaker is present in arrangement
            if (((ulong)arrangement & speaker) == 0) return -1;

            var result = 0;
            var i = 1UL;
            while (i < speaker)
            {
                if (((ulong)arrangement & i) != 0) result++;
                i <<= 1;
            }

            return result;
        }

        /// <summary>
        /// Returns the speaker for a given index in a speaker arrangement (return 0 when out of range).
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Speaker GetSpeaker(this SpeakerArrangement arr, int index)
        {
            var arrTmp = (int)arr;
            var index2 = -1;
            var pos = -1;
            while (arrTmp != 0)
            {
                if ((arrTmp & 0x1) != 0) index2++;
                pos++;
                if (index2 == index) return (Speaker)(1 << pos);
                arrTmp >>= 1;
            }
            return 0;
        }

        /// <summary>
        /// Returns true if arrSubSet is a subset speaker of arr (means each speaker of arrSubSet is included in arr).
        /// </summary>
        /// <param name="arrSubSet"></param>
        /// <param name="arr"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsSubsetOf(this SpeakerArrangement arrSubSet, SpeakerArrangement arr)
            => arrSubSet == (arrSubSet & arr);

        /// <summary>
        /// Returns true if arrangement is a Auro configuration.
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAuro(this SpeakerArrangement arr)
            => arr == k90 || arr == k91 || arr == k100 || arr == k101 || arr == k110 || arr == k111 || arr == k130 || arr == k131;

        /// <summary>
        /// Returns true if arrangement contains top (upper layer) speakers
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasTopSpeakers(this SpeakerArrangement source)
        {
            var arr = (Speaker)source;
            return (arr & kSpeakerTc) != 0 || (arr & kSpeakerTfl) != 0 || (arr & kSpeakerTfc) != 0 || (arr & kSpeakerTfr) != 0 ||
                (arr & kSpeakerTrl) != 0 || (arr & kSpeakerTrc) != 0 || (arr & kSpeakerTrr) != 0 || (arr & kSpeakerTsl) != 0 ||
                (arr & kSpeakerTsr) != 0;
        }

        /// <summary>
        /// Returns true if arrangement contains bottom (lower layer) speakers */
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasBottomSpeakers(this SpeakerArrangement source)
        {
            var arr = (Speaker)source;
            return (arr & kSpeakerBfl) != 0 || (arr & kSpeakerBfc) != 0 || (arr & kSpeakerBfl) != 0 || (arr & kSpeakerBfc) != 0 ||
                (arr & kSpeakerBfr) != 0;
        }

        /// <summary>
        /// Returns true if arrangement contains middle layer (at ears level) speakers */
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasMiddleSpeakers(this SpeakerArrangement source)
        {
            var arr = (Speaker)source;
            return (arr & kSpeakerL) != 0 || (arr & kSpeakerR) != 0 || (arr & kSpeakerC) != 0 || (arr & kSpeakerLs) != 0 ||
                (arr & kSpeakerRs) != 0 || (arr & kSpeakerLc) != 0 || (arr & kSpeakerRc) != 0 || (arr & kSpeakerCs) != 0 ||
                (arr & kSpeakerSl) != 0 || (arr & kSpeakerSr) != 0 || (arr & kSpeakerM) != 0 || (arr & kSpeakerPl) != 0 ||
                (arr & kSpeakerPr) != 0 || (arr & kSpeakerLcs) != 0 || (arr & kSpeakerRcs) != 0;
        }

        /// <summary>
        /// Returns true if arrangement contains LFE speakers
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool HasLfe(this SpeakerArrangement source)
        {
            var arr = (Speaker)source;
            return (arr & kSpeakerLfe) != 0 || (arr & kSpeakerLfe2) != 0;
        }

        /// <summary>
        /// Returns true if arrangement is a 3D configuration ((top or bottom) and middle)
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Is3D(this SpeakerArrangement arr)
        {
            bool top = HasTopSpeakers(arr), bottom = HasBottomSpeakers(arr), middle = HasMiddleSpeakers(arr);
            return ((top || bottom) && middle) || (top && bottom);
        }

        /// <summary>
        /// Returns true if arrangement is a Auro configuration.
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAmbisonics(this SpeakerArrangement arr)
            => arr == kAmbi1stOrderACN || arr == kAmbi2cdOrderACN || arr == kAmbi3rdOrderACN;

        /// <summary>
        /// Returns the speaker arrangement associated to a string representation.
        /// </summary>
        /// <param name="arrStr"></param>
        /// <returns>kEmpty if no associated arrangement is known.</returns>
        public static SpeakerArrangement GetSpeakerArrangementFromString(string arrStr)
        {
            if (string.Equals(arrStr, kStringMono)) return kMono;
            if (string.Equals(arrStr, kStringStereo)) return kStereo;
            if (string.Equals(arrStr, kStringStereoR)) return kStereoSurround;
            if (string.Equals(arrStr, kStringStereoC)) return kStereoCenter;
            if (string.Equals(arrStr, kStringStereoSide)) return kStereoSide;
            if (string.Equals(arrStr, kStringStereoCLfe)) return kStereoCLfe;
            if (string.Equals(arrStr, kStringStereoTF)) return kStereoTF;
            if (string.Equals(arrStr, kStringStereoTS)) return kStereoTS;
            if (string.Equals(arrStr, kStringStereoTR)) return kStereoTR;
            if (string.Equals(arrStr, kStringStereoBF)) return kStereoBF;
            if (string.Equals(arrStr, kStringCineFront)) return kCineFront;
            if (string.Equals(arrStr, kString30Cine)) return k30Cine;
            if (string.Equals(arrStr, kString30Music)) return k30Music;
            if (string.Equals(arrStr, kString31Cine)) return k31Cine;
            if (string.Equals(arrStr, kString31Music)) return k31Music;
            if (string.Equals(arrStr, kString40Cine)) return k40Cine;
            if (string.Equals(arrStr, kString40Music)) return k40Music;
            if (string.Equals(arrStr, kString41Cine)) return k41Cine;
            if (string.Equals(arrStr, kString41Music)) return k41Music;
            if (string.Equals(arrStr, kString50)) return k50;
            if (string.Equals(arrStr, kString51)) return k51;
            if (string.Equals(arrStr, kString60Cine)) return k60Cine;
            if (string.Equals(arrStr, kString60Music)) return k60Music;
            if (string.Equals(arrStr, kString61Cine)) return k61Cine;
            if (string.Equals(arrStr, kString61Music)) return k61Music;
            if (string.Equals(arrStr, kString70Cine) || string.Equals(arrStr, kString70CineOld)) return k70Cine;
            if (string.Equals(arrStr, kString70Music) || string.Equals(arrStr, kString70MusicOld)) return k70Music;
            if (string.Equals(arrStr, kString71Cine) || string.Equals(arrStr, kString71CineOld)) return k71Cine;
            if (string.Equals(arrStr, kString71Music) || string.Equals(arrStr, kString71MusicOld)) return k71Music;
            if (string.Equals(arrStr, kString71Proximity)) return k71Proximity;
            if (string.Equals(arrStr, kString80Cine)) return k80Cine;
            if (string.Equals(arrStr, kString80Music)) return k80Music;
            if (string.Equals(arrStr, kString81Cine)) return k81Cine;
            if (string.Equals(arrStr, kString81Music)) return k81Music;
            if (string.Equals(arrStr, kString102)) return k102;
            if (string.Equals(arrStr, kString122)) return k122;
            if (string.Equals(arrStr, kString80Cube)) return k80Cube;
            if (string.Equals(arrStr, kString71CineTopCenter)) return k71CineTopCenter;
            if (string.Equals(arrStr, kString71CineCenterHigh)) return k71CineCenterHigh;
            if (string.Equals(arrStr, kString71CineFrontHigh)) return k71CineFrontHigh;
            if (string.Equals(arrStr, kString70CineFrontHigh)) return k70CineFrontHigh;
            if (string.Equals(arrStr, kString71CineSideHigh)) return k71CineSideHigh;
            if (string.Equals(arrStr, kString71CineFullRear)) return k71CineFullRear;
            if (string.Equals(arrStr, kString90Cine)) return k90Cine;
            if (string.Equals(arrStr, kString91Cine)) return k91Cine;
            if (string.Equals(arrStr, kString100Cine)) return k100Cine;
            if (string.Equals(arrStr, kString101Cine)) return k101Cine;
            if (string.Equals(arrStr, kString50_4)) return k50_4;
            if (string.Equals(arrStr, kString51_4)) return k51_4;
            if (string.Equals(arrStr, kString50_4_1)) return k50_4_1;
            if (string.Equals(arrStr, kString51_4_1)) return k51_4_1;
            if (string.Equals(arrStr, kString81MPEG)) return k81MPEG3D;
            if (string.Equals(arrStr, kString70_2)) return k70_2;
            if (string.Equals(arrStr, kString71_2)) return k71_2;
            if (string.Equals(arrStr, kString70_3)) return k70_3;
            if (string.Equals(arrStr, kString72_3)) return k72_3;
            if (string.Equals(arrStr, kString70_4)) return k70_4;
            if (string.Equals(arrStr, kString71_4)) return k71_4;
            if (string.Equals(arrStr, kString70_6)) return k70_6;
            if (string.Equals(arrStr, kString71_6)) return k71_6;
            if (string.Equals(arrStr, kString90_4)) return k90_4;
            if (string.Equals(arrStr, kString91_4)) return k91_4;
            if (string.Equals(arrStr, kString90_6)) return k90_6;
            if (string.Equals(arrStr, kString91_6)) return k91_6;
            if (string.Equals(arrStr, kString100)) return k100;
            if (string.Equals(arrStr, kString101)) return k101;
            if (string.Equals(arrStr, kString110)) return k110;
            if (string.Equals(arrStr, kString111)) return k111;
            if (string.Equals(arrStr, kString130)) return k130;
            if (string.Equals(arrStr, kString131)) return k131;
            if (string.Equals(arrStr, kString140)) return k140;
            if (string.Equals(arrStr, kString222)) return k222;
            if (string.Equals(arrStr, kString220)) return k220;
            if (string.Equals(arrStr, kString50_5_3)) return k50_5_3;
            if (string.Equals(arrStr, kString51_5_3)) return k51_5_3;
            if (string.Equals(arrStr, kStringAmbi1stOrder)) return kAmbi1stOrderACN;
            if (string.Equals(arrStr, kStringAmbi2cdOrder)) return kAmbi2cdOrderACN;
            if (string.Equals(arrStr, kStringAmbi3rdOrder)) return kAmbi3rdOrderACN;
            return kEmpty;
        }

        /// <summary>
        /// Returns the string representation of a given speaker arrangement.
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="withSpeakersName"></param>
        /// <returns>kStringEmpty if arr is unknown.</returns>
        public static string GetSpeakerArrangementString(SpeakerArrangement arr, bool withSpeakersName)
        {
            switch (arr)
            {
                case kMono: return withSpeakersName ? kStringMonoS : kStringMono;
                case kStereo: return withSpeakersName ? kStringStereoS : kStringStereo;
                case kStereoSurround: return withSpeakersName ? kStringStereoRS : kStringStereoR;
                case kStereoCenter: return withSpeakersName ? kStringStereoCS : kStringStereoC;
                case kStereoSide: return withSpeakersName ? kStringStereoSS : kStringStereoSide;
                case kStereoCLfe: return withSpeakersName ? kStringStereoCLfeS : kStringStereoCLfe;
                case kStereoTF: return withSpeakersName ? kStringStereoTFS : kStringStereoTF;
                case kStereoTS: return withSpeakersName ? kStringStereoTSS : kStringStereoTS;
                case kStereoTR: return withSpeakersName ? kStringStereoTRS : kStringStereoTR;
                case kStereoBF: return withSpeakersName ? kStringStereoBFS : kStringStereoBF;
                case kCineFront: return withSpeakersName ? kStringCineFrontS : kStringCineFront;
                case k30Cine: return withSpeakersName ? kString30CineS : kString30Cine;
                case k30Music: return withSpeakersName ? kString30MusicS : kString30Music;
                case k31Cine: return withSpeakersName ? kString31CineS : kString31Cine;
                case k31Music: return withSpeakersName ? kString31MusicS : kString31Music;
                case k40Cine: return withSpeakersName ? kString40CineS : kString40Cine;
                case k40Music: return withSpeakersName ? kString40MusicS : kString40Music;
                case k41Cine: return withSpeakersName ? kString41CineS : kString41Cine;
                case k41Music: return withSpeakersName ? kString41MusicS : kString41Music;
                case k50: return withSpeakersName ? kString50S : kString50;
                case k51: return withSpeakersName ? kString51S : kString51;
                case k60Cine: return withSpeakersName ? kString60CineS : kString60Cine;
                case k60Music: return withSpeakersName ? kString60MusicS : kString60Music;
                case k61Cine: return withSpeakersName ? kString61CineS : kString61Cine;
                case k61Music: return withSpeakersName ? kString61MusicS : kString61Music;
                case k70Cine: return withSpeakersName ? kString70CineS : kString70Cine;
                case k70Music: return withSpeakersName ? kString70MusicS : kString70Music;
                case k71Cine: return withSpeakersName ? kString71CineS : kString71Cine;
                case k71Music: return withSpeakersName ? kString71MusicS : kString71Music;
                case k71Proximity: return withSpeakersName ? kString71ProximityS : kString71Proximity;
                case k80Cine: return withSpeakersName ? kString80CineS : kString80Cine;
                case k80Music: return withSpeakersName ? kString80MusicS : kString80Music;
                case k81Cine: return withSpeakersName ? kString81CineS : kString81Cine;
                case k81Music: return withSpeakersName ? kString81MusicS : kString81Music;
                case k81MPEG3D: return withSpeakersName ? kString81MPEGS : kString81MPEG;
                case k102: return withSpeakersName ? kString102S : kString102;
                case k122: return withSpeakersName ? kString122S : kString122;
                case k80Cube: return withSpeakersName ? kString80CubeS : kString80Cube;
                case k71CineTopCenter: return withSpeakersName ? kString71CineTopCenterS : kString71CineTopCenter;
                case k71CineCenterHigh: return withSpeakersName ? kString71CineCenterHighS : kString71CineCenterHigh;
                case k71CineFrontHigh: return withSpeakersName ? kString71CineFrontHighS : kString71CineFrontHigh;
                case k70CineFrontHigh: return withSpeakersName ? kString70CineFrontHighS : kString70CineFrontHigh;
                case k71CineSideHigh: return withSpeakersName ? kString71CineSideHighS : kString71CineSideHigh;
                case k71CineFullRear: return withSpeakersName ? kString71CineFullRearS : kString71CineFullRear;
                case k90Cine: return withSpeakersName ? kString90CineS : kString90Cine;
                case k91Cine: return withSpeakersName ? kString91CineS : kString91Cine;
                case k100Cine: return withSpeakersName ? kString100CineS : kString100Cine;
                case k101Cine: return withSpeakersName ? kString101CineS : kString101Cine;
                case k100: return withSpeakersName ? kString100S : kString100;
                case k101: return withSpeakersName ? kString101S : kString101;
                case k110: return withSpeakersName ? kString110S : kString110;
                case k111: return withSpeakersName ? kString111S : kString111;

                case k50_4: return withSpeakersName ? kString50_4S : kString50_4;
                case k51_4: return withSpeakersName ? kString51_4S : kString51_4;
                case k50_4_1: return withSpeakersName ? kString50_4_1S : kString50_4_1;
                case k51_4_1: return withSpeakersName ? kString51_4_1S : kString51_4_1;
                case k70_2: return withSpeakersName ? kString70_2S : kString70_2;
                case k71_2: return withSpeakersName ? kString71_2S : kString71_2;
                case k70_3: return withSpeakersName ? kString70_3S : kString70_3;
                case k72_3: return withSpeakersName ? kString72_3S : kString72_3;
                case k70_4: return withSpeakersName ? kString70_4S : kString70_4;
                case k71_4: return withSpeakersName ? kString71_4S : kString71_4;
                case k70_6: return withSpeakersName ? kString70_6S : kString70_6;
                case k71_6: return withSpeakersName ? kString71_6S : kString71_6;
                case k90_4: return withSpeakersName ? kString90_4S : kString90_4;
                case k91_4: return withSpeakersName ? kString91_4S : kString91_4;
                case k90_6: return withSpeakersName ? kString90_6S : kString90_6;
                case k91_6: return withSpeakersName ? kString91_6S : kString91_6;
                case k130: return withSpeakersName ? kString130S : kString130;
                case k131: return withSpeakersName ? kString131S : kString131;
                case k140: return withSpeakersName ? kString140S : kString140;
                case k222: return withSpeakersName ? kString222S : kString222;
                case k220: return withSpeakersName ? kString220S : kString220;
                case k50_5_3: return withSpeakersName ? kString50_5_3S : kString50_5_3;
                case k51_5_3: return withSpeakersName ? kString51_5_3S : kString51_5_3;
            }

            if (arr == kAmbi1stOrderACN) return withSpeakersName ? kStringAmbi1stOrderS : kStringAmbi1stOrder;
            if (arr == kAmbi2cdOrderACN) return withSpeakersName ? kStringAmbi2cdOrderS : kStringAmbi2cdOrder;
            if (arr == kAmbi3rdOrderACN) return withSpeakersName ? kStringAmbi3rdOrderS : kStringAmbi3rdOrder;

            return kStringEmpty;
        }

        /// <summary>
        /// Returns a CString representation of a given speaker in a given arrangement
        /// </summary>
        /// <param name="arr"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string GetSpeakerShortName(this SpeakerArrangement arr, int index)
        {
            var arrTmp = (ulong)arr;

            var found = false;
            var index2 = -1;
            var pos = -1;
            while (arrTmp != 0)
            {
                if ((arrTmp & 0x1) != 0) index2++;
                pos++;
                if (index2 == index) { found = true; break; }
                arrTmp >>= 1;
            }

            if (!found) return string.Empty;

            var speaker = (Speaker)(1 << pos);
            if (speaker == kSpeakerL) return "L";
            if (speaker == kSpeakerR) return "R";
            if (speaker == kSpeakerC) return "C";
            if (speaker == kSpeakerLfe) return "LFE";
            if (speaker == kSpeakerLs) return "Ls";
            if (speaker == kSpeakerRs) return "Rs";
            if (speaker == kSpeakerLc) return "Lc";
            if (speaker == kSpeakerRc) return "Rc";
            if (speaker == kSpeakerCs) return "S";
            if (speaker == kSpeakerSl) return "Sl";
            if (speaker == kSpeakerSr) return "Sr";
            if (speaker == kSpeakerTc) return "Tc";
            if (speaker == kSpeakerTfl) return "Tfl";
            if (speaker == kSpeakerTfc) return "Tfc";
            if (speaker == kSpeakerTfr) return "Tfr";
            if (speaker == kSpeakerTrl) return "Trl";
            if (speaker == kSpeakerTrc) return "Trc";
            if (speaker == kSpeakerTrr) return "Trr";
            if (speaker == kSpeakerLfe2) return "LFE2";
            if (speaker == kSpeakerM) return "M";

            if (speaker == kSpeakerACN0) return "0";
            if (speaker == kSpeakerACN1) return "1";
            if (speaker == kSpeakerACN2) return "2";
            if (speaker == kSpeakerACN3) return "3";
            if (speaker == kSpeakerACN4) return "4";
            if (speaker == kSpeakerACN5) return "5";
            if (speaker == kSpeakerACN6) return "6";
            if (speaker == kSpeakerACN7) return "7";
            if (speaker == kSpeakerACN8) return "8";
            if (speaker == kSpeakerACN9) return "9";
            if (speaker == kSpeakerACN10) return "10";
            if (speaker == kSpeakerACN11) return "11";
            if (speaker == kSpeakerACN12) return "12";
            if (speaker == kSpeakerACN13) return "13";
            if (speaker == kSpeakerACN14) return "14";
            if (speaker == kSpeakerACN15) return "15";

            if (speaker == kSpeakerTsl) return "Tsl";
            if (speaker == kSpeakerTsr) return "Tsr";
            if (speaker == kSpeakerLcs) return "Lcs";
            if (speaker == kSpeakerRcs) return "Rcs";

            if (speaker == kSpeakerBfl) return "Bfl";
            if (speaker == kSpeakerBfc) return "Bfc";
            if (speaker == kSpeakerBfr) return "Bfr";
            if (speaker == kSpeakerPl) return "Pl";
            if (speaker == kSpeakerPr) return "Pr";
            if (speaker == kSpeakerBsl) return "Bsl";
            if (speaker == kSpeakerBsr) return "Bsr";
            if (speaker == kSpeakerBrl) return "Brl";
            if (speaker == kSpeakerBrc) return "Brc";
            if (speaker == kSpeakerBrr) return "Brr";

            return string.Empty;
        }
    }
}
