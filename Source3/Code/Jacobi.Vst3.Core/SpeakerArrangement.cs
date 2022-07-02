using static Jacobi.Vst3.Core.Speaker;

namespace Jacobi.Vst3.Core
{
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
}
