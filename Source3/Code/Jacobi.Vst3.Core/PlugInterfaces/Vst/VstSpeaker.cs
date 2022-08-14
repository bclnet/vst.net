using System;
using System.Runtime.CompilerServices;
using static Jacobi.Vst3.Core.Speaker;
using static Jacobi.Vst3.Core.SpeakerArrangement;
using static Jacobi.Vst3.Core.SpeakerArrangementString;

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

    /// <summary>
    /// Speaker Arrangement Definitions.
    /// </summary>
    public enum SpeakerArrangement : ulong
    {
        /// <summary>
        /// empty arrangement
        /// </summary>
        kEmpty = 0,
        /// <summary>
        /// M
        /// </summary>
        kMono = kSpeakerM,
        /// <summary>
        /// L R
        /// </summary>
        kStereo = kSpeakerL | kSpeakerR,
        /// <summary>
        /// Ls Rs
        /// </summary>
        kStereoSurround = kSpeakerLs | kSpeakerRs,
        /// <summary>
        /// Lc Rc
        /// </summary>
        kStereoCenter = kSpeakerLc | kSpeakerRc,
        /// <summary>
        /// Sl Sr
        /// </summary>
        kStereoSide = kSpeakerSl | kSpeakerSr,
        /// <summary>
        /// C Lfe
        /// </summary>
        kStereoCLfe = kSpeakerC | kSpeakerLfe,
        /// <summary>
        /// Tfl Tfr
        /// </summary>
        kStereoTF = kSpeakerTfl | kSpeakerTfr,
        /// <summary>
        /// Tsl Tsr
        /// </summary>
        kStereoTS = kSpeakerTsl | kSpeakerTsr,
        /// <summary>
        /// Trl Trr
        /// </summary>
        kStereoTR = kSpeakerTrl | kSpeakerTrr,
        /// <summary>
        /// Bfl Bfr
        /// </summary>
        kStereoBF = kSpeakerBfl | kSpeakerBfr,
        /// <summary>
        /// L R C Lc Rc
        /// </summary>
        kCineFront = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLc | kSpeakerRc,

        /// <summary>
        /// L R C
        /// </summary>
        k30Cine = kSpeakerL | kSpeakerR | kSpeakerC,
        /// <summary>
        /// L R C   Lfe
        /// </summary>
        k31Cine = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLfe,
        /// <summary>
        /// L R S
        /// </summary>
        k30Music = kSpeakerL | kSpeakerR | kSpeakerCs,
        /// <summary>
        /// L R Lfe S
        /// </summary>
        k31Music = kSpeakerL | kSpeakerR | kSpeakerLfe | kSpeakerCs,
        /// <summary>
        /// L R C   S (LCRS)
        /// </summary>
        k40Cine = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerCs,
        /// <summary>
        /// L R C   Lfe S (LCRS+Lfe)
        /// </summary>
        k41Cine = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLfe | kSpeakerCs,
        /// <summary>
        /// L R Ls  Rs (Quadro)
        /// </summary>
        k40Music = kSpeakerL | kSpeakerR | kSpeakerLs | kSpeakerRs,
        /// <summary>
        /// L R Lfe Ls Rs (Quadro+Lfe)
        /// </summary>
        k41Music = kSpeakerL | kSpeakerR | kSpeakerLfe | kSpeakerLs | kSpeakerRs,
        /// <summary>
        /// L R C   Ls Rs - 5.0 (ITU 0+5+0.0 Sound System B)
        /// </summary>
        k50 = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLs | kSpeakerRs,
        /// <summary>
        /// L R C  Lfe Ls Rs - 5.1 (ITU 0+5+0.1 Sound System B)
        /// </summary>
        k51 = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLfe | kSpeakerLs | kSpeakerRs,
        /// <summary>
        /// L R C  Ls  Rs Cs
        /// </summary>
        k60Cine = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLs | kSpeakerRs | kSpeakerCs,
        /// <summary>
        /// L R C   Lfe Ls Rs Cs
        /// </summary>
        k61Cine = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLfe | kSpeakerLs | kSpeakerRs | kSpeakerCs,
        /// <summary>
        /// L R Ls Rs  Sl Sr 
        /// </summary>
        k60Music = kSpeakerL | kSpeakerR | kSpeakerLs | kSpeakerRs | kSpeakerSl | kSpeakerSr,
        /// <summary>
        /// L R Lfe Ls  Rs Sl Sr 
        /// </summary>
        k61Music = kSpeakerL | kSpeakerR | kSpeakerLfe | kSpeakerLs | kSpeakerRs | kSpeakerSl | kSpeakerSr,
        /// <summary>
        /// L R C   Ls  Rs Lc Rc 
        /// </summary>
        k70Cine = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLs | kSpeakerRs | kSpeakerLc | kSpeakerRc,
        /// <summary>
        /// L R C Lfe Ls Rs Lc Rc
        /// </summary>
        k71Cine = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLfe | kSpeakerLs | kSpeakerRs | kSpeakerLc | kSpeakerRc,
        k71CineFullFront = k71Cine,
        /// <summary>
        /// L R C   Ls  Rs Sl Sr
        /// </summary>
        k70Music = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLs | kSpeakerRs | kSpeakerSl | kSpeakerSr,
        /// <summary>
        /// L R C Lfe Ls Rs Sl Sr
        /// </summary>
        k71Music = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLfe | kSpeakerLs | kSpeakerRs | kSpeakerSl | kSpeakerSr,

        /// <summary>
        /// L R C Lfe Ls Rs Lcs Rcs
        /// </summary>
        k71CineFullRear = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLfe | kSpeakerLs | kSpeakerRs | kSpeakerLcs | kSpeakerRcs,
        k71CineSideFill = k61Music,
        /// <summary>
        /// L R C Lfe Ls Rs Pl Pr
        /// </summary>
        k71Proximity = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLfe | kSpeakerLs | kSpeakerRs | kSpeakerPl | kSpeakerPr,

        /// <summary>
        /// L R C Ls  Rs Lc Rc Cs
        /// </summary>
        k80Cine = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLs | kSpeakerRs | kSpeakerLc | kSpeakerRc | kSpeakerCs,
        /// <summary>
        /// L R C Lfe Ls Rs Lc Rc Cs
        /// </summary>
        k81Cine = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLfe | kSpeakerLs | kSpeakerRs | kSpeakerLc | kSpeakerRc | kSpeakerCs,
        /// <summary>
        /// L R C Ls  Rs Cs Sl Sr
        /// </summary>
        k80Music = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLs | kSpeakerRs | kSpeakerCs | kSpeakerSl | kSpeakerSr,
        /// <summary>
        /// L R C Lfe Ls Rs Cs Sl Sr 
        /// </summary>
        k81Music = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLfe | kSpeakerLs | kSpeakerRs | kSpeakerCs | kSpeakerSl | kSpeakerSr,
        /// <summary>
        /// L R C Ls Rs Lc Rc Sl Sr
        /// </summary>
        k90Cine = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLs | kSpeakerRs | kSpeakerLc | kSpeakerRc |
                  kSpeakerSl | kSpeakerSr,
        /// <summary>
        /// L R C Lfe Ls Rs Lc Rc Sl Sr
        /// </summary>
        k91Cine = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLfe | kSpeakerLs | kSpeakerRs | kSpeakerLc | kSpeakerRc |
                  kSpeakerSl | kSpeakerSr,
        /// <summary>
        /// L R C Ls Rs Lc Rc Cs Sl Sr
        /// </summary>
        k100Cine = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLs | kSpeakerRs | kSpeakerLc | kSpeakerRc | kSpeakerCs |
                   kSpeakerSl | kSpeakerSr,
        /// <summary>
        /// L R C Lfe Ls Rs Lc Rc Cs Sl Sr
        /// </summary>
        k101Cine = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLfe | kSpeakerLs | kSpeakerRs | kSpeakerLc | kSpeakerRc | kSpeakerCs |
                   kSpeakerSl | kSpeakerSr,

        /// <summary>
        /// First-Order with Ambisonic Channel Number (ACN) ordering and SN3D normalization
        /// </summary>
        kAmbi1stOrderACN = kSpeakerACN0 | kSpeakerACN1 | kSpeakerACN2 | kSpeakerACN3,
        /// <summary>
        /// Second-Order with Ambisonic Channel Number (ACN) ordering and SN3D normalization
        /// </summary>
        kAmbi2cdOrderACN = kAmbi1stOrderACN | kSpeakerACN4 | kSpeakerACN5 | kSpeakerACN6 | kSpeakerACN7 | kSpeakerACN8,
        /// <summary>
        /// Third-Order with Ambisonic Channel Number (ACN) ordering and SN3D normalization
        /// </summary>
        kAmbi3rdOrderACN = kAmbi2cdOrderACN | kSpeakerACN9 | kSpeakerACN10 | kSpeakerACN11 | kSpeakerACN12 | kSpeakerACN13 | kSpeakerACN14 | kSpeakerACN15,


        // 3D formats
        /// <summary>
        /// L R Ls Rs Tfl Tfr Trl Trr - 4.0.4
        /// </summary>
        k80Cube = kSpeakerL | kSpeakerR | kSpeakerLs | kSpeakerRs | kSpeakerTfl | kSpeakerTfr | kSpeakerTrl | kSpeakerTrr,
        k40_4 = k80Cube,

        /// <summary>
        /// L R C Lfe Ls Rs Cs Tc - 6.1.1
        /// </summary>
        k71CineTopCenter = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLfe | kSpeakerLs | kSpeakerRs | kSpeakerCs | kSpeakerTc,

        /// <summary>
        /// L R C Lfe Ls Rs Cs Tfc - 6.1.1
        /// </summary>
        k71CineCenterHigh = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLfe | kSpeakerLs | kSpeakerRs | kSpeakerCs | kSpeakerTfc,

        /// <summary>
        /// L R C Ls Rs Tfl Tfr - 5.0.2 (ITU 2+5+0.0 Sound System C)
        /// </summary>
        k70CineFrontHigh = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLs | kSpeakerRs | kSpeakerTfl | kSpeakerTfr,
        k70MPEG3D = k70CineFrontHigh,
        k50_2 = k70CineFrontHigh,

        /// <summary>
        /// L R C Lfe Ls Rs Tfl Tfr - 5.1.2 (ITU 2+5+0.1 Sound System C)
        /// </summary>
        k71CineFrontHigh = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLfe | kSpeakerLs | kSpeakerRs | kSpeakerTfl | kSpeakerTfr,
        k71MPEG3D = k71CineFrontHigh,
        k51_2 = k71CineFrontHigh,

        /// <summary>
        /// L R C Lfe Ls Rs Tsl Tsr - 5.1.2 (Side)
        /// </summary>
        k71CineSideHigh = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLfe | kSpeakerLs | kSpeakerRs | kSpeakerTsl | kSpeakerTsr,

        /// <summary>
        /// L R Lfe Ls Rs Tfl Tfc Tfr Bfc - 4.1.3.1
        /// </summary>
        k81MPEG3D = kSpeakerL | kSpeakerR | kSpeakerLfe | kSpeakerLs | kSpeakerRs |
                    kSpeakerTfl | kSpeakerTfc | kSpeakerTfr | kSpeakerBfc,
        k41_4_1 = k81MPEG3D,

        /// <summary>
        /// L R C Ls Rs Tfl Tfr Trl Trr - 5.0.4 (ITU 4+5+0.0 Sound System D)
        /// </summary>
        k90 = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLs | kSpeakerRs |
              kSpeakerTfl | kSpeakerTfr | kSpeakerTrl | kSpeakerTrr,
        k50_4 = k90,

        /// <summary>
        /// L R C Lfe Ls Rs Tfl Tfr Trl Trr - 5.1.4 (ITU 4+5+0.1 Sound System D)
        /// </summary>
        k91 = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLfe | kSpeakerLs | kSpeakerRs |
              kSpeakerTfl | kSpeakerTfr | kSpeakerTrl | kSpeakerTrr,
        k51_4 = k91,

        /// <summary>
        /// L R C Ls Rs Tfl Tfr Trl Trr Bfc - 5.0.4.1 (ITU 4+5+1.0 Sound System E)
        /// </summary>
        k50_4_1 = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLs | kSpeakerRs |
                  kSpeakerTfl | kSpeakerTfr | kSpeakerTrl | kSpeakerTrr | kSpeakerBfc,

        /// <summary>
        /// L R C Lfe Ls Rs Tfl Tfr Trl Trr Bfc - 5.1.4.1 (ITU 4+5+1.1 Sound System E)
        /// </summary>
        k51_4_1 = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLfe | kSpeakerLs | kSpeakerRs |
                  kSpeakerTfl | kSpeakerTfr | kSpeakerTrl | kSpeakerTrr | kSpeakerBfc,

        /// <summary>
        /// L R C Ls Rs Sl Sr Tsl Tsr - 7.0.2
        /// </summary>
        k70_2 = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLs | kSpeakerRs |
                kSpeakerSl | kSpeakerSr | kSpeakerTsl | kSpeakerTsr,

        /// <summary>
        /// L R C Lfe Ls Rs Sl Sr Tsl Tsr - 7.1.2
        /// </summary>
        k71_2 = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLfe | kSpeakerLs | kSpeakerRs |
                kSpeakerSl | kSpeakerSr | kSpeakerTsl | kSpeakerTsr,
        /// <summary>
        /// 9.1 Dolby Atmos (3D)
        /// </summary>
        k91Atmos = k71_2,

        /// <summary>
        /// L R C Ls Rs Sl Sr Tsl Tsr Trc - 7.0.3 (ITU 3+7+0.0 Sound System F)
        /// </summary>
        k70_3 = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLs | kSpeakerRs |
                kSpeakerSl | kSpeakerSr | kSpeakerTsl | kSpeakerTsr | kSpeakerTrc,

        /// <summary>
        /// L R C Lfe Ls Rs Sl Sr Tsl Tsr Trc Lfe2 - 7.2.3 (ITU 3+7+0.2 Sound System F)
        /// </summary>
        k72_3 = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLfe | kSpeakerLs | kSpeakerRs |
                kSpeakerSl | kSpeakerSr | kSpeakerTsl | kSpeakerTsr | kSpeakerTrc | kSpeakerLfe2,

        /// <summary>
        /// L R C Ls Rs Sl Sr Tfl Tfr Trl Trr - 7.0.4 (ITU 4+7+0.0 Sound System J)
        /// </summary>
        k70_4 = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLs | kSpeakerRs | kSpeakerSl | kSpeakerSr |
                kSpeakerTfl | kSpeakerTfr | kSpeakerTrl | kSpeakerTrr,

        /// <summary>
        /// L R C Lfe Ls Rs Sl Sr Tfl Tfr Trl Trr - 7.1.4 (ITU 4+7+0.1 Sound System J)
        /// </summary>
        k71_4 = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLfe | kSpeakerLs | kSpeakerRs | kSpeakerSl | kSpeakerSr |
                kSpeakerTfl | kSpeakerTfr | kSpeakerTrl | kSpeakerTrr,
        k111MPEG3D = k71_4,

        /// <summary>
        /// L R C Ls Rs Sl Sr Tfl Tfr Trl Trr Tsl Tsr - 7.0.6
        /// </summary>
        k70_6 = kSpeakerL | kSpeakerR | kSpeakerC |
                kSpeakerLs | kSpeakerRs | kSpeakerSl | kSpeakerSr |
                kSpeakerTfl | kSpeakerTfr | kSpeakerTrl | kSpeakerTrr | kSpeakerTsl | kSpeakerTsr,

        /// <summary>
        /// L R C Lfe Ls Rs Sl Sr Tfl Tfr Trl Trr Tsl Tsr - 7.1.6
        /// </summary>
        k71_6 = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLfe |
                kSpeakerLs | kSpeakerRs | kSpeakerSl | kSpeakerSr |
                kSpeakerTfl | kSpeakerTfr | kSpeakerTrl | kSpeakerTrr | kSpeakerTsl | kSpeakerTsr,

        /// <summary>
        /// L R C Ls Rs Lc Rc Sl Sr Tfl Tfr Trl Trr - 9.0.4 (ITU 4+9+0.0 Sound System G)
        /// </summary>
        k90_4 = kSpeakerL | kSpeakerR | kSpeakerC |
                kSpeakerLs | kSpeakerRs | kSpeakerLc | kSpeakerRc | kSpeakerSl | kSpeakerSr |
                kSpeakerTfl | kSpeakerTfr | kSpeakerTrl | kSpeakerTrr,

        /// <summary>
        /// L R C Lfe Ls Rs Lc Rc Sl Sr Tfl Tfr Trl Trr - 9.1.4 (ITU 4+9+0.1 Sound System G)
        /// </summary>
        k91_4 = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLfe |
                kSpeakerLs | kSpeakerRs | kSpeakerLc | kSpeakerRc | kSpeakerSl | kSpeakerSr |
                kSpeakerTfl | kSpeakerTfr | kSpeakerTrl | kSpeakerTrr,

        /// <summary>
        /// L R C Lfe Ls Rs Lc Rc Sl Sr Tfl Tfr Trl Trr Tsl Tsr - 9.0.6
        /// </summary>
        k90_6 = kSpeakerL | kSpeakerR | kSpeakerC |
                kSpeakerLs | kSpeakerRs | kSpeakerLc | kSpeakerRc | kSpeakerSl | kSpeakerSr |
                kSpeakerTfl | kSpeakerTfr | kSpeakerTrl | kSpeakerTrr | kSpeakerTsl | kSpeakerTsr,

        /// <summary>
        /// L R C Lfe Ls Rs Lc Rc Sl Sr Tfl Tfr Trl Trr Tsl Tsr - 9.1.6
        /// </summary>
        k91_6 = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLfe |
                kSpeakerLs | kSpeakerRs | kSpeakerLc | kSpeakerRc | kSpeakerSl | kSpeakerSr |
                kSpeakerTfl | kSpeakerTfr | kSpeakerTrl | kSpeakerTrr | kSpeakerTsl | kSpeakerTsr,

        /// <summary>
        /// L R C Ls Rs Tc Tfl Tfr Trl Trr - 5.0.5
        /// </summary>
        k100 = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLs | kSpeakerRs |
               kSpeakerTc | kSpeakerTfl | kSpeakerTfr | kSpeakerTrl | kSpeakerTrr,
        k50_5 = k100,

        /// <summary>
        /// L R C Lfe Ls Rs Tc Tfl Tfr Trl Trr - 5.1.5
        /// </summary>
        k101 = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLfe | kSpeakerLs | kSpeakerRs |
               kSpeakerTc | kSpeakerTfl | kSpeakerTfr | kSpeakerTrl | kSpeakerTrr,
        k101MPEG3D = k101,
        k51_5 = k101,

        /// <summary>
        /// L R C Lfe Ls Rs Tfl Tfc Tfr Trl Trr Lfe2 - 5.2.5
        /// </summary>
        k102 = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLfe | kSpeakerLs | kSpeakerRs |
               kSpeakerTfl | kSpeakerTfc | kSpeakerTfr | kSpeakerTrl | kSpeakerTrr | kSpeakerLfe2,
        k52_5 = k102,

        /// <summary>
        /// L R C Ls Rs Tc Tfl Tfc Tfr Trl Trr - 5.0.6
        /// </summary>
        k110 = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLs | kSpeakerRs |
               kSpeakerTc | kSpeakerTfl | kSpeakerTfc | kSpeakerTfr | kSpeakerTrl | kSpeakerTrr,
        k50_6 = k110,

        /// <summary>
        /// L R C Lfe Ls Rs Tc Tfl Tfc Tfr Trl Trr - 5.1.6
        /// </summary>
        k111 = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLfe | kSpeakerLs | kSpeakerRs |
               kSpeakerTc | kSpeakerTfl | kSpeakerTfc | kSpeakerTfr | kSpeakerTrl | kSpeakerTrr,
        k51_6 = k111,

        /// <summary>
        /// L R C Lfe Ls Rs Lc Rc Tfl Tfc Tfr Trl Trr Lfe2 - 7.2.5
        /// </summary>
        k122 = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLfe | kSpeakerLs | kSpeakerRs | kSpeakerLc | kSpeakerRc |
               kSpeakerTfl | kSpeakerTfc | kSpeakerTfr | kSpeakerTrl | kSpeakerTrr | kSpeakerLfe2,
        k72_5 = k122,

        /// <summary>
        /// L R C Ls Rs Sl Sr Tc Tfl Tfc Tfr Trl Trr - 7.0.6
        /// </summary>
        k130 = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLs | kSpeakerRs | kSpeakerSl | kSpeakerSr |
               kSpeakerTc | kSpeakerTfl | kSpeakerTfc | kSpeakerTfr | kSpeakerTrl | kSpeakerTrr,

        /// <summary>
        /// L R C Lfe Ls Rs Sl Sr Tc Tfl Tfc Tfr Trl Trr - 7.1.6
        /// </summary>
        k131 = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLfe | kSpeakerLs | kSpeakerRs | kSpeakerSl | kSpeakerSr |
               kSpeakerTc | kSpeakerTfl | kSpeakerTfc | kSpeakerTfr | kSpeakerTrl | kSpeakerTrr,

        /// <summary>
        /// L R Ls Rs Sl Sr Tfl Tfr Trl Trr Bfl Bfr Brl Brr - 6.0.4.4
        /// </summary>
        k140 = kSpeakerL | kSpeakerR | kSpeakerLs | kSpeakerRs | kSpeakerSl | kSpeakerSr |
               kSpeakerTfl | kSpeakerTfr | kSpeakerTrl | kSpeakerTrr |
               kSpeakerBfl | kSpeakerBfr | kSpeakerBrl | kSpeakerBrr,
        k60_4_4 = k140,

        /// <summary>
        /// L R C Ls Rs Lc Rc Cs Sl Sr Tc Tfl Tfc Tfr Trl Trc Trr Tsl Tsr Bfl Bfc Bfr - 10.0.9.3 (ITU 9+10+3.0 Sound System H)
        /// </summary>
        k220 = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLs | kSpeakerRs | kSpeakerLc | kSpeakerRc | kSpeakerCs | kSpeakerSl | kSpeakerSr |
               kSpeakerTc | kSpeakerTfl | kSpeakerTfc | kSpeakerTfr | kSpeakerTrl | kSpeakerTrc | kSpeakerTrr | kSpeakerTsl | kSpeakerTsr |
               kSpeakerBfl | kSpeakerBfc | kSpeakerBfr,
        k100_9_3 = k220,

        /// <summary>
        /// L R C Lfe Ls Rs Lc Rc Cs Sl Sr Tc Tfl Tfc Tfr Trl Trc Trr Lfe2 Tsl Tsr Bfl Bfc Bfr - 10.2.9.3 (ITU 9+10+3.2 Sound System H)
        /// </summary>
        k222 = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLfe | kSpeakerLs | kSpeakerRs | kSpeakerLc | kSpeakerRc | kSpeakerCs | kSpeakerSl | kSpeakerSr |
               kSpeakerTc | kSpeakerTfl | kSpeakerTfc | kSpeakerTfr | kSpeakerTrl | kSpeakerTrc | kSpeakerTrr | kSpeakerLfe2 | kSpeakerTsl | kSpeakerTsr |
               kSpeakerBfl | kSpeakerBfc | kSpeakerBfr,
        k102_9_3 = k222,

        /// <summary>
        /// L R C Ls Rs Tfl Tfc Tfr Trl Trr Bfl Bfc Bfr - 5.0.5.3
        /// </summary>
        k50_5_3 = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLs | kSpeakerRs | kSpeakerTfl | kSpeakerTfc | kSpeakerTfr | kSpeakerTrl | kSpeakerTrr |
                  kSpeakerBfl | kSpeakerBfc | kSpeakerBfr,

        /// <summary>
        /// L R C Lfe Ls Rs Tfl Tfc Tfr Trl Trr Bfl Bfc Bfr - 5.1.5.3
        /// </summary>
        k51_5_3 = kSpeakerL | kSpeakerR | kSpeakerC | kSpeakerLfe | kSpeakerLs | kSpeakerRs | kSpeakerTfl | kSpeakerTfc | kSpeakerTfr | kSpeakerTrl | kSpeakerTrr |
                  kSpeakerBfl | kSpeakerBfc | kSpeakerBfr,
    }

    /// <summary>
    /// Speaker Arrangement String Representation.
    /// </summary>
    public static partial class SpeakerArrangementString
    {
        public const string kStringEmpty = "";
        public const string kStringMono = "Mono";
        public const string kStringStereo = "Stereo";
        public const string kStringStereoR = "Stereo (Ls Rs)";
        public const string kStringStereoC = "Stereo (Lc Rc)";
        public const string kStringStereoSide = "Stereo (Sl Sr)";
        public const string kStringStereoCLfe = "Stereo (C LFE)";
        public const string kStringStereoTF = "Stereo (Tfl Tfr)";
        public const string kStringStereoTS = "Stereo (Tsl Tsr)";
        public const string kStringStereoTR = "Stereo (Trl Trr)";
        public const string kStringStereoBF = "Stereo (Bfl Bfr)";
        public const string kStringCineFront = "Cine Front";

        public const string kString30Cine = "LRC";
        public const string kString30Music = "LRS";
        public const string kString31Cine = "LRC+LFE";
        public const string kString31Music = "LRS+LFE";
        public const string kString40Cine = "LRCS";
        public const string kString40Music = "Quadro";
        public const string kString41Cine = "LRCS+LFE";
        public const string kString41Music = "Quadro+LFE";
        public const string kString50 = "5.0";
        public const string kString51 = "5.1";
        public const string kString60Cine = "6.0 Cine";
        public const string kString60Music = "6.0 Music";
        public const string kString61Cine = "6.1 Cine";
        public const string kString61Music = "6.1 Music";
        public const string kString70Cine = "7.0 SDDS";
        public const string kString70CineOld = "7.0 Cine (SDDS)";
        public const string kString70Music = "7.0";
        public const string kString70MusicOld = "7.0 Music (Dolby)";
        public const string kString71Cine = "7.1 SDDS";
        public const string kString71CineOld = "7.1 Cine (SDDS)";
        public const string kString71Music = "7.1";
        public const string kString71MusicOld = "7.1 Music (Dolby)";
        public const string kString71CineTopCenter = "7.1 Cine Top Center";
        public const string kString71CineCenterHigh = "7.1 Cine Center High";
        public const string kString71CineFrontHigh = "7.1 Cine Front High";
        public const string kString70CineFrontHigh = "7.0 Cine Front High";
        public const string kString71CineSideHigh = "7.1 Cine Side High";
        public const string kString71CineFullRear = "7.1 Cine Full Rear";
        public const string kString71Proximity = "7.1 Proximity";
        public const string kString80Cine = "8.0 Cine";
        public const string kString80Music = "8.0 Music";
        public const string kString80Cube = "8.0 Cube";
        public const string kString81Cine = "8.1 Cine";
        public const string kString81Music = "8.1 Music";
        public const string kString90Cine = "9.0 Cine";
        public const string kString91Cine = "9.1 Cine";
        public const string kString100Cine = "10.0 Cine";
        public const string kString101Cine = "10.1 Cine";
        public const string kString102 = "10.2 Experimental";
        public const string kString122 = "12.2";
        public const string kString50_4 = "5.0.4";
        public const string kString51_4 = "5.1.4";
        public const string kString50_4_1 = "5.0.4.1";
        public const string kString51_4_1 = "5.1.4.1";
        public const string kString70_2 = "7.0.2";
        public const string kString71_2 = "7.1.2";
        public const string kString70_3 = "7.0.3";
        public const string kString72_3 = "7.2.3";
        public const string kString70_4 = "7.0.4";
        public const string kString71_4 = "7.1.4";
        public const string kString70_6 = "7.0.6";
        public const string kString71_6 = "7.1.6";
        public const string kString90_4 = "9.0.4";
        public const string kString91_4 = "9.1.4";
        public const string kString90_6 = "9.0.6";
        public const string kString91_6 = "9.1.6";
        public const string kString100 = "10.0 Auro-3D";
        public const string kString101 = "10.1 Auro-3D";
        public const string kString110 = "11.0 Auro-3D";
        public const string kString111 = "11.1 Auro-3D";
        public const string kString130 = "13.0 Auro-3D";
        public const string kString131 = "13.1 Auro-3D";
        public const string kString81MPEG = "8.1 MPEG";
        public const string kString140 = "14.0";
        public const string kString222 = "22.2";
        public const string kString220 = "22.0";
        public const string kString50_5_3 = "5.0.5.3";
        public const string kString51_5_3 = "5.1.5.3";
        public const string kStringAmbi1stOrder = "1st Order Ambisonics";
        public const string kStringAmbi2cdOrder = "2nd Order Ambisonics";
        public const string kStringAmbi3rdOrder = "3rd Order Ambisonics";
    }

    /// <summary>
    /// Speaker Arrangement String Representation with Speakers Name.
    /// </summary>
    public static partial class SpeakerArrangementString
    {
        public const string kStringMonoS = "M";
        public const string kStringStereoS = "L R";
        public const string kStringStereoRS = "Ls Rs";
        public const string kStringStereoCS = "Lc Rc";
        public const string kStringStereoSS = "Sl Sr";
        public const string kStringStereoCLfeS = "C LFE";
        public const string kStringStereoTFS = "Tfl Tfr";
        public const string kStringStereoTSS = "Tsl Tsr";
        public const string kStringStereoTRS = "Trl Trr";
        public const string kStringStereoBFS = "Bfl Bfr";
        public const string kStringCineFrontS = "L R C Lc Rc";
        public const string kString30CineS = "L R C";
        public const string kString30MusicS = "L R S";
        public const string kString31CineS = "L R C LFE";
        public const string kString31MusicS = "L R LFE S";
        public const string kString40CineS = "L R C S";
        public const string kString40MusicS = "L R Ls Rs";
        public const string kString41CineS = "L R C LFE S";
        public const string kString41MusicS = "L R LFE Ls Rs";
        public const string kString50S = "L R C Ls Rs";
        public const string kString51S = "L R C LFE Ls Rs";
        public const string kString60CineS = "L R C Ls Rs Cs";
        public const string kString60MusicS = "L R Ls Rs Sl Sr";
        public const string kString61CineS = "L R C LFE Ls Rs Cs";
        public const string kString61MusicS = "L R LFE Ls Rs Sl Sr";
        public const string kString70CineS = "L R C Ls Rs Lc Rc";
        public const string kString70MusicS = "L R C Ls Rs Sl Sr";
        public const string kString71CineS = "L R C LFE Ls Rs Lc Rc";
        public const string kString71MusicS = "L R C LFE Ls Rs Sl Sr";
        public const string kString80CineS = "L R C Ls Rs Lc Rc Cs";
        public const string kString80MusicS = "L R C Ls Rs Cs Sl Sr";
        public const string kString81CineS = "L R C LFE Ls Rs Lc Rc Cs";
        public const string kString81MusicS = "L R C LFE Ls Rs Cs Sl Sr";
        public const string kString80CubeS = "L R Ls Rs Tfl Tfr Trl Trr";
        public const string kString71CineTopCenterS = "L R C LFE Ls Rs Cs Tc";
        public const string kString71CineCenterHighS = "L R C LFE Ls Rs Cs Tfc";
        public const string kString71CineFrontHighS = "L R C LFE Ls Rs Tfl Tfr";
        public const string kString70CineFrontHighS = "L R C Ls Rs Tfl Tfr";
        public const string kString71CineSideHighS = "L R C LFE Ls Rs Tsl Tsr";
        public const string kString71CineFullRearS = "L R C LFE Ls Rs Lcs Rcs";
        public const string kString71ProximityS = "L R C LFE Ls Rs Pl Pr";
        public const string kString90CineS = "L R C Ls Rs Lc Rc Sl Sr";
        public const string kString91CineS = "L R C LFE Ls Rs Lc Rc Sl Sr";
        public const string kString100CineS = "L R C Ls Rs Lc Rc Cs Sl Sr";
        public const string kString101CineS = "L R C LFE Ls Rs Lc Rc Cs Sl Sr";
        public const string kString50_4S = "L R C Ls Rs Tfl Tfr Trl Trr";
        public const string kString51_4S = "L R C LFE Ls Rs Tfl Tfr Trl Trr";
        public const string kString50_4_1S = "L R C Ls Rs Tfl Tfr Trl Trr Bfc";
        public const string kString51_4_1S = "L R C LFE Ls Rs Tfl Tfr Trl Trr Bfc";
        public const string kString70_2S = "L R C Ls Rs Sl Sr Tsl Tsr";
        public const string kString71_2S = "L R C LFE Ls Rs Sl Sr Tsl Tsr";
        public const string kString70_3S = "L R C Ls Rs Sl Sr Tsl Tsr Trc";
        public const string kString72_3S = "L R C LFE Ls Rs Sl Sr Tsl Tsr Trc LFE";
        public const string kString70_4S = "L R C Ls Rs Sl Sr Tfl Tfr Trl Trr";
        public const string kString71_4S = "L R C LFE Ls Rs Sl Sr Tfl Tfr Trl Trr";
        public const string kString70_6S = "L R C Ls Rs Sl Sr Tfl Tfr Trl Trr Tsl Tsr";
        public const string kString71_6S = "L R C LFE Ls Rs Sl Sr Tfl Tfr Trl Trr Tsl Tsr";
        public const string kString90_4S = "L R C Ls Rs Lc Rc Sl Sr Tfl Tfr Trl Trr";
        public const string kString91_4S = "L R C LFE Ls Rs Lc Rc Sl Sr Tfl Tfr Trl Trr";
        public const string kString90_6S = "L R C Ls Rs Lc Rc Sl Sr Tfl Tfr Trl Trr Tsl Tsr";
        public const string kString91_6S = "L R C LFE Ls Rs Lc Rc Sl Sr Tfl Tfr Trl Trr Tsl Tsr";
        public const string kString100S = "L R C Ls Rs Tc Tfl Tfr Trl Trr";
        public const string kString101S = "L R C LFE Ls Rs Tc Tfl Tfr Trl Trr";
        public const string kString110S = "L R C Ls Rs Tc Tfl Tfc Tfr Trl Trr";
        public const string kString111S = "L R C LFE Ls Rs Tc Tfl Tfc Tfr Trl Trr";
        public const string kString130S = "L R C Ls Rs Sl Sr Tc Tfl Tfc Tfr Trl Trr";
        public const string kString131S = "L R C LFE Ls Rs Sl Sr Tc Tfl Tfc Tfr Trl Trr";
        public const string kString102S = "L R C LFE Ls Rs Tfl Tfc Tfr Trl Trr LFE2";
        public const string kString122S = "L R C LFE Ls Rs Lc Rc Tfl Tfc Tfr Trl Trr LFE2";
        public const string kString81MPEGS = "L R LFE Ls Rs Tfl Tfc Tfr Bfc";
        public const string kString140S = "L R Ls Rs Sl Sr Tfl Tfr Trl Trr Bfl Bfr Brl Brr";
        public const string kString222S = "L R C LFE Ls Rs Lc Rc Cs Sl Sr Tc Tfl Tfc Tfr Trl Trc Trr LFE2 Tsl Tsr Bfl Bfc Bfr";
        public const string kString220S = "L R C Ls Rs Lc Rc Cs Sl Sr Tc Tfl Tfc Tfr Trl Trc Trr Tsl Tsr Bfl Bfc Bfr";
        public const string kString50_5_3S = "L R C Ls Rs Tfl Tfc Tfr Trl Trr Bfl Bfc Bfr";
        public const string kString51_5_3S = "L R C LFE Ls Rs Tfl Tfc Tfr Trl Trr Bfl Bfc Bfr";

        public const string kStringAmbi1stOrderS = "0 1 2 3";
        public const string kStringAmbi2cdOrderS = "0 1 2 3 4 5 6 7 8";
        public const string kStringAmbi3rdOrderS = "0 1 2 3 4 5 6 7 8 9 10 11 12 13 14 15";
    }

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
