using System;

namespace Jacobi.Vst3.Core
{
    [Flags]
    public enum Speaker : ulong
    {
        kSpeakerL = 1L << 0,            // Left (L)
        kSpeakerR = 1L << 1,            // Right (R)
        kSpeakerC = 1L << 2,            // Center (C)
        kSpeakerLfe = 1L << 3,          // Subbass (Lfe)
        kSpeakerLs = 1L << 4,           // Left Surround (Ls)
        kSpeakerRs = 1L << 5,           // Right Surround (Rs)
        kSpeakerLc = 1L << 6,           // Left of Center (Lc) - Front Left Center
        kSpeakerRc = 1L << 7,           // Right of Center (Rc) - Front Right Center
        kSpeakerS = 1L << 8,            // Surround (S)
        kSpeakerCs = kSpeakerS,         // Center of Surround (Cs) - Back Center - Surround (S)
        kSpeakerSl = 1L << 9,           // Side Left (Sl)
        kSpeakerSr = 1L << 10,          // Side Right (Sr)
        kSpeakerTc = 1L << 11,          // Top Center Over-head, Top Middle (Tc)
        kSpeakerTfl = 1L << 12,         // Top Front Left (Tfl)
        kSpeakerTfc = 1L << 13,         // Top Front Center (Tfc)
        kSpeakerTfr = 1L << 14,         // Top Front Right (Tfr)
        kSpeakerTrl = 1L << 15,         // Top Rear/Back Left (Trl)
        kSpeakerTrc = 1L << 16,         // Top Rear/Back Center (Trc)
        kSpeakerTrr = 1L << 17,         // Top Rear/Back Right (Trr)
        kSpeakerLfe2 = 1L << 18,        // Subbass 2 (Lfe2)
        kSpeakerM = 1L << 19,           // Mono (M)

        kSpeakerACN0 = 1L << 20,	    // Ambisonic ACN 0
        kSpeakerACN1 = 1L << 21,	    // Ambisonic ACN 1
        kSpeakerACN2 = 1L << 22,	    // Ambisonic ACN 2
        kSpeakerACN3 = 1L << 23,	    // Ambisonic ACN 3
        kSpeakerACN4 = 1L << 38,	    // Ambisonic ACN 4
        kSpeakerACN5 = 1L << 39,	    // Ambisonic ACN 5
        kSpeakerACN6 = 1L << 40,	    // Ambisonic ACN 6
        kSpeakerACN7 = 1L << 41,	    // Ambisonic ACN 7
        kSpeakerACN8 = 1L << 42,	    // Ambisonic ACN 8
        kSpeakerACN9 = 1L << 43,	    // Ambisonic ACN 9
        kSpeakerACN10 = 1L << 44,	    // Ambisonic ACN 10
        kSpeakerACN11 = 1L << 45,	    // Ambisonic ACN 11
        kSpeakerACN12 = 1L << 46,	    // Ambisonic ACN 12
        kSpeakerACN13 = 1L << 47,	    // Ambisonic ACN 13
        kSpeakerACN14 = 1L << 48,	    // Ambisonic ACN 14
        kSpeakerACN15 = 1L << 49,	    // Ambisonic ACN 15

        kSpeakerTsl = 1L << 24,         // Top Side Left (Tsl)
        kSpeakerTsr = 1L << 25,         // Top Side Right (Tsr)
        kSpeakerLcs = 1L << 26,         // Left of Center Surround (Lcs) - Back Left Center
        kSpeakerRcs = 1L << 27,         // Right of Center Surround (Rcs) - Back Right Center

        kSpeakerBfl = 1L << 28,         // Bottom Front Left (Bfl)
        kSpeakerBfc = 1L << 29,         // Bottom Front Center (Bfc)
        kSpeakerBfr = 1L << 30,         // Bottom Front Right (Bfr)

        kSpeakerPl = 1L << 31,          // Proximity Left (Pl)
        kSpeakerPr = 1L << 32,          // Proximity Right (Pr)

        kSpeakerBsl = 1L << 33,         // Bottom Side Left (Bsl)
        kSpeakerBsr = 1L << 34,         // Bottom Side Right (Bsr)
        kSpeakerBrl = 1L << 35,         // Bottom Rear Left (Brl)
        kSpeakerBrc = 1L << 36,         // Bottom Rear Center (Brc)
        kSpeakerBrr = 1L << 37,	        // Bottom Rear Right (Brr)
    }
}
