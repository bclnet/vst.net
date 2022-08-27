namespace Jacobi.Vst3
{
    /// <summary>
    /// Predefined Preset Attributes
    /// </summary>
    public static class PresetAttributes
    {
        public const string PlugInName = "PlugInName";             // Plug-in name
        public const string PlugInCategory = "PlugInCategory";     // eg. "Fx|Dynamics", "Instrument", "Instrument|Synth"

        public const string Instrument = "MusicalInstrument";      // eg. instrument group (like 'Piano' or 'Piano|A. Piano')
        public const string Style = "MusicalStyle";                // eg. 'Pop', 'Jazz', 'Classic'
        public const string Character = "MusicalCharacter";        // eg. instrument nature (like 'Soft' 'Dry' 'Acoustic')

        public const string StateType = "StateType";               // Type of the given state see \ref StateType : Project / Default Preset or Normal Preset
        public const string FilePathStringType = "FilePathString"; // Full file path string (if available) where the preset comes from (be sure to use a bigger string when asking for it (with 1024 characters))
        public const string Name = "Name";                         // name of the preset
        public const string FileName = "FileName";			       // filename of the preset (including extension)
    }

    /// <summary>
    /// Predefined StateType used for Key kStateType
    /// </summary>
    public static class StateType
    {
        public const string Project = "Project";        // the state is restored from a project loading or it is saved in a project
        public const string Default = "Default";        // the state is restored from a preset (marked as default) or the host wants to store a default state of the plug-in
    }

    /// <summary>
    /// Predefined Musical Instrument
    /// </summary>
    public static class MusicalInstrument
    {
        public const string Accordion = "Accordion";
        public const string AccordionAccordion = "Accordion|Accordion";
        public const string AccordionHarmonica = "Accordion|Harmonica";
        public const string AccordionOther = "Accordion|Other";

        public const string Bass = "Bass";
        public const string BassABass = "Bass|A. Bass";
        public const string BassEBass = "Bass|E. Bass";
        public const string BassSynthBass = "Bass|Synth Bass";
        public const string BassOther = "Bass|Other";

        public const string Brass = "Brass";
        public const string BrassFrenchHorn = "Brass|French Horn";
        public const string BrassTrumpet = "Brass|Trumpet";
        public const string BrassTrombone = "Brass|Trombone";
        public const string BrassTuba = "Brass|Tuba";
        public const string BrassSection = "Brass|Section";
        public const string BrassSynth = "Brass|Synth";
        public const string BrassOther = "Brass|Other";

        public const string ChromaticPerc = "Chromatic Perc";
        public const string ChromaticPercBell = "Chromatic Perc|Bell";
        public const string ChromaticPercMallett = "Chromatic Perc|Mallett";
        public const string ChromaticPercWood = "Chromatic Perc|Wood";
        public const string ChromaticPercPercussion = "Chromatic Perc|Percussion";
        public const string ChromaticPercTimpani = "Chromatic Perc|Timpani";
        public const string ChromaticPercOther = "Chromatic Perc|Other";

        public const string DrumPerc = "Drum&Perc";
        public const string DrumPercDrumsetGM = "Drum&Perc|Drumset GM";
        public const string DrumPercDrumset = "Drum&Perc|Drumset";
        public const string DrumPercDrumMenues = "Drum&Perc|Drum Menues";
        public const string DrumPercBeats = "Drum&Perc|Beats";
        public const string DrumPercPercussion = "Drum&Perc|Percussion";
        public const string DrumPercKickDrum = "Drum&Perc|Kick Drum";
        public const string DrumPercSnareDrum = "Drum&Perc|Snare Drum";
        public const string DrumPercToms = "Drum&Perc|Toms";
        public const string DrumPercHiHats = "Drum&Perc|HiHats";
        public const string DrumPercCymbals = "Drum&Perc|Cymbals";
        public const string DrumPercOther = "Drum&Perc|Other";

        public const string Ethnic = "Ethnic";
        public const string EthnicAsian = "Ethnic|Asian";
        public const string EthnicAfrican = "Ethnic|African";
        public const string EthnicEuropean = "Ethnic|European";
        public const string EthnicLatin = "Ethnic|Latin";
        public const string EthnicAmerican = "Ethnic|American";
        public const string EthnicAlien = "Ethnic|Alien";
        public const string EthnicOther = "Ethnic|Other";

        public const string Guitar = "Guitar/Plucked";
        public const string GuitarAGuitar = "Guitar/Plucked|A. Guitar";
        public const string GuitarEGuitar = "Guitar/Plucked|E. Guitar";
        public const string GuitarHarp = "Guitar/Plucked|Harp";
        public const string GuitarEthnic = "Guitar/Plucked|Ethnic";
        public const string GuitarOther = "Guitar/Plucked|Other";

        public const string Keyboard = "Keyboard";
        public const string KeyboardClavi = "Keyboard|Clavi";
        public const string KeyboardEPiano = "Keyboard|E. Piano";
        public const string KeyboardHarpsichord = "Keyboard|Harpsichord";
        public const string KeyboardOther = "Keyboard|Other";

        public const string MusicalFX = "Musical FX";
        public const string MusicalFXHitsStabs = "Musical FX|Hits&Stabs";
        public const string MusicalFXMotion = "Musical FX|Motion";
        public const string MusicalFXSweeps = "Musical FX|Sweeps";
        public const string MusicalFXBeepsBlips = "Musical FX|Beeps&Blips";
        public const string MusicalFXScratches = "Musical FX|Scratches";
        public const string MusicalFXOther = "Musical FX|Other";

        public const string Organ = "Organ";
        public const string OrganElectric = "Organ|Electric";
        public const string OrganPipe = "Organ|Pipe";
        public const string OrganOther = "Organ|Other";

        public const string Piano = "Piano";
        public const string PianoAPiano = "Piano|A. Piano";
        public const string PianoEGrand = "Piano|E. Grand";
        public const string PianoOther = "Piano|Other";

        public const string SoundFX = "Sound FX";
        public const string SoundFXNature = "Sound FX|Nature";
        public const string SoundFXMechanical = "Sound FX|Mechanical";
        public const string SoundFXSynthetic = "Sound FX|Synthetic";
        public const string SoundFXOther = "Sound FX|Other";

        public const string Strings = "Strings";
        public const string StringsViolin = "Strings|Violin";
        public const string StringsViola = "Strings|Viola";
        public const string StringsCello = "Strings|Cello";
        public const string StringsBass = "Strings|Bass";
        public const string StringsSection = "Strings|Section";
        public const string StringsSynth = "Strings|Synth";
        public const string StringsOther = "Strings|Other";

        public const string SynthLead = "Synth Lead";
        public const string SynthLeadAnalog = "Synth Lead|Analog";
        public const string SynthLeadDigital = "Synth Lead|Digital";
        public const string SynthLeadArpeggio = "Synth Lead|Arpeggio";
        public const string SynthLeadOther = "Synth Lead|Other";

        public const string SynthPad = "Synth Pad";
        public const string SynthPadSynthChoir = "Synth Pad|Synth Choir";
        public const string SynthPadAnalog = "Synth Pad|Analog";
        public const string SynthPadDigital = "Synth Pad|Digital";
        public const string SynthPadMotion = "Synth Pad|Motion";
        public const string SynthPadOther = "Synth Pad|Other";

        public const string SynthComp = "Synth Comp";
        public const string SynthCompAnalog = "Synth Comp|Analog";
        public const string SynthCompDigital = "Synth Comp|Digital";
        public const string SynthCompOther = "Synth Comp|Other";

        public const string Vocal = "Vocal";
        public const string VocalLeadVocal = "Vocal|Lead Vocal";
        public const string VocalAdlibs = "Vocal|Adlibs";
        public const string VocalChoir = "Vocal|Choir";
        public const string VocalSolo = "Vocal|Solo";
        public const string VocalFX = "Vocal|FX";
        public const string VocalSpoken = "Vocal|Spoken";
        public const string VocalOther = "Vocal|Other";

        public const string Woodwinds = "Woodwinds";
        public const string WoodwindsEthnic = "Woodwinds|Ethnic";
        public const string WoodwindsFlute = "Woodwinds|Flute";
        public const string WoodwindsOboe = "Woodwinds|Oboe";
        public const string WoodwindsEnglHorn = "Woodwinds|Engl. Horn";
        public const string WoodwindsClarinet = "Woodwinds|Clarinet";
        public const string WoodwindsSaxophone = "Woodwinds|Saxophone";
        public const string WoodwindsBassoon = "Woodwinds|Bassoon";
        public const string WoodwindsOther = "Woodwinds|Other";
    }

    /// <summary>
    /// Predefined Musical Style
    /// </summary>
    public static class MusicalStyle
    {
        public const string AlternativeIndie = "Alternative/Indie";
        public const string AlternativeIndieGothRock = "Alternative/Indie|Goth Rock";
        public const string AlternativeIndieGrunge = "Alternative/Indie|Grunge";
        public const string AlternativeIndieNewWave = "Alternative/Indie|New Wave";
        public const string AlternativeIndiePunk = "Alternative/Indie|Punk";
        public const string AlternativeIndieCollegeRock = "Alternative/Indie|College Rock";
        public const string AlternativeIndieDarkWave = "Alternative/Indie|Dark Wave";
        public const string AlternativeIndieHardcore = "Alternative/Indie|Hardcore";

        public const string AmbientChillOut = "Ambient/ChillOut";
        public const string AmbientChillOutNewAgeMeditation = "Ambient/ChillOut|New Age/Meditation";
        public const string AmbientChillOutDarkAmbient = "Ambient/ChillOut|Dark Ambient";
        public const string AmbientChillOutDowntempo = "Ambient/ChillOut|Downtempo";
        public const string AmbientChillOutLounge = "Ambient/ChillOut|Lounge";

        public const string Blues = "Blues";
        public const string BluesAcousticBlues = "Blues|Acoustic Blues";
        public const string BluesCountryBlues = "Blues|Country Blues";
        public const string BluesElectricBlues = "Blues|Electric Blues";
        public const string BluesChicagoBlues = "Blues|Chicago Blues";

        public const string Classical = "Classical";
        public const string ClassicalBaroque = "Classical|Baroque";
        public const string ClassicalChamberMusic = "Classical|Chamber Music";
        public const string ClassicalMedieval = "Classical|Medieval";
        public const string ClassicalModernComposition = "Classical|Modern Composition";
        public const string ClassicalOpera = "Classical|Opera";
        public const string ClassicalGregorian = "Classical|Gregorian";
        public const string ClassicalRenaissance = "Classical|Renaissance";
        public const string ClassicalClassic = "Classical|Classic";
        public const string ClassicalRomantic = "Classical|Romantic";
        public const string ClassicalSoundtrack = "Classical|Soundtrack";

        public const string Country = "Country";
        public const string CountryCountryWestern = "Country|Country/Western";
        public const string CountryHonkyTonk = "Country|Honky Tonk";
        public const string CountryUrbanCowboy = "Country|Urban Cowboy";
        public const string CountryBluegrass = "Country|Bluegrass";
        public const string CountryAmericana = "Country|Americana";
        public const string CountrySquaredance = "Country|Squaredance";
        public const string CountryNorthAmericanFolk = "Country|North American Folk";

        public const string ElectronicaDance = "Electronica/Dance";
        public const string ElectronicaDanceMinimal = "Electronica/Dance|Minimal";
        public const string ElectronicaDanceClassicHouse = "Electronica/Dance|Classic House";
        public const string ElectronicaDanceElektroHouse = "Electronica/Dance|Elektro House";
        public const string ElectronicaDanceFunkyHouse = "Electronica/Dance|Funky House";
        public const string ElectronicaDanceIndustrial = "Electronica/Dance|Industrial";
        public const string ElectronicaDanceElectronicBodyMusic = "Electronica/Dance|Electronic Body Music";
        public const string ElectronicaDanceTripHop = "Electronica/Dance|Trip Hop";
        public const string ElectronicaDanceTechno = "Electronica/Dance|Techno";
        public const string ElectronicaDanceDrumNBassJungle = "Electronica/Dance|Drum'n'Bass/Jungle";
        public const string ElectronicaDanceElektro = "Electronica/Dance|Elektro";
        public const string ElectronicaDanceTrance = "Electronica/Dance|Trance";
        public const string ElectronicaDanceDub = "Electronica/Dance|Dub";
        public const string ElectronicaDanceBigBeats = "Electronica/Dance|Big Beats";

        public const string Experimental = "Experimental";
        public const string ExperimentalNewMusic = "Experimental|New Music";
        public const string ExperimentalFreeImprovisation = "Experimental|Free Improvisation";
        public const string ExperimentalElectronicArtMusic = "Experimental|Electronic Art Music";
        public const string ExperimentalNoise = "Experimental|Noise";

        public const string Jazz = "Jazz";
        public const string JazzNewOrleansJazz = "Jazz|New Orleans Jazz";
        public const string JazzTraditionalJazz = "Jazz|Traditional Jazz";
        public const string JazzOldtimeJazzDixiland = "Jazz|Oldtime Jazz/Dixiland";
        public const string JazzFusion = "Jazz|Fusion";
        public const string JazzAvantgarde = "Jazz|Avantgarde";
        public const string JazzLatinJazz = "Jazz|Latin Jazz";
        public const string JazzFreeJazz = "Jazz|Free Jazz";
        public const string JazzRagtime = "Jazz|Ragtime";

        public const string Pop = "Pop";
        public const string PopBritpop = "Pop|Britpop";
        public const string PopRock = "Pop|Pop/Rock";
        public const string PopTeenPop = "Pop|Teen Pop";
        public const string PopChartDance = "Pop|Chart Dance";
        public const string Pop80sPop = "Pop|80's Pop";
        public const string PopDancehall = "Pop|Dancehall";
        public const string PopDisco = "Pop|Disco";

        public const string RockMetal = "Rock/Metal";
        public const string RockMetalBluesRock = "Rock/Metal|Blues Rock";
        public const string RockMetalClassicRock = "Rock/Metal|Classic Rock";
        public const string RockMetalHardRock = "Rock/Metal|Hard Rock";
        public const string RockMetalRockRoll = "Rock/Metal|Rock &amp; Roll";
        public const string RockMetalSingerSongwriter = "Rock/Metal|Singer/Songwriter";
        public const string RockMetalHeavyMetal = "Rock/Metal|Heavy Metal";
        public const string RockMetalDeathBlackMetal = "Rock/Metal|Death/Black Metal";
        public const string RockMetalNuMetal = "Rock/Metal|NuMetal";
        public const string RockMetalReggae = "Rock/Metal|Reggae";
        public const string RockMetalBallad = "Rock/Metal|Ballad";
        public const string RockMetalAlternativeRock = "Rock/Metal|Alternative Rock";
        public const string RockMetalRockabilly = "Rock/Metal|Rockabilly";
        public const string RockMetalThrashMetal = "Rock/Metal|Thrash Metal";
        public const string RockMetalProgressiveRock = "Rock/Metal|Progressive Rock";

        public const string UrbanHipHopRB = "Urban (Hip-Hop / R&B)";
        public const string UrbanHipHopRBClassic = "Urban (Hip-Hop / R&B)|Classic R&B";
        public const string UrbanHipHopRBModern = "Urban (Hip-Hop / R&B)|Modern R&B";
        public const string UrbanHipHopRBPop = "Urban (Hip-Hop / R&B)|R&B Pop";
        public const string UrbanHipHopRBWestCoastHipHop = "Urban (Hip-Hop / R&B)|WestCoast Hip-Hop";
        public const string UrbanHipHopRBEastCoastHipHop = "Urban (Hip-Hop / R&B)|EastCoast Hip-Hop";
        public const string UrbanHipHopRBRapHipHop = "Urban (Hip-Hop / R&B)|Rap/Hip Hop";
        public const string UrbanHipHopRBSoul = "Urban (Hip-Hop / R&B)|Soul";
        public const string UrbanHipHopRBFunk = "Urban (Hip-Hop / R&B)|Funk";

        public const string WorldEthnic = "World/Ethnic";
        public const string WorldEthnicAfrica = "World/Ethnic|Africa";
        public const string WorldEthnicAsia = "World/Ethnic|Asia";
        public const string WorldEthnicCeltic = "World/Ethnic|Celtic";
        public const string WorldEthnicEurope = "World/Ethnic|Europe";
        public const string WorldEthnicKlezmer = "World/Ethnic|Klezmer";
        public const string WorldEthnicScandinavia = "World/Ethnic|Scandinavia";
        public const string WorldEthnicEasternEurope = "World/Ethnic|Eastern Europe";
        public const string WorldEthnicIndiaOriental = "World/Ethnic|India/Oriental";
        public const string WorldEthnicNorthAmerica = "World/Ethnic|North America";
        public const string WorldEthnicSouthAmerica = "World/Ethnic|South America";
        public const string WorldEthnicAustralia = "World/Ethnic|Australia";
    }

    /// <summary>
    /// Predefined Musical Character
    /// </summary>
    public static class MusicalCharacter
    {
        public const string Mono = "Mono";
        public const string Poly = "Poly";

        public const string Split = "Split";
        public const string Layer = "Layer";

        public const string Glide = "Glide";
        public const string Glissando = "Glissando";

        public const string Major = "Major";
        public const string Minor = "Minor";

        public const string Single = "Single";
        public const string Ensemble = "Ensemble";

        public const string Acoustic = "Acoustic";
        public const string Electric = "Electric";

        public const string Analog = "Analog";
        public const string Digital = "Digital";

        public const string Vintage = "Vintage";
        public const string Modern = "Modern";

        public const string Old = "Old";
        public const string New = "New";

        //----TONE------------------------------------
        public const string Clean = "Clean";
        public const string Distorted = "Distorted";

        public const string Dry = "Dry";
        public const string Processed = "Processed";

        public const string Harmonic = "Harmonic";
        public const string Dissonant = "Dissonant";

        public const string Clear = "Clear";
        public const string Noisy = "Noisy";

        public const string Thin = "Thin";
        public const string Rich = "Rich";

        public const string Dark = "Dark";
        public const string Bright = "Bright";

        public const string Cold = "Cold";
        public const string Warm = "Warm";

        public const string Metallic = "Metallic";
        public const string Wooden = "Wooden";

        public const string Glass = "Glass";
        public const string Plastic = "Plastic";

        //----ENVELOPE------------------------------------
        public const string Percussive = "Percussive";
        public const string Soft = "Soft";

        public const string Fast = "Fast";
        public const string Slow = "Slow";

        public const string Short = "Short";
        public const string Long = "Long";

        public const string Attack = "Attack";
        public const string Release = "Release";

        public const string Decay = "Decay";
        public const string Sustain = "Sustain";

        public const string FastAttack = "Fast Attack";
        public const string SlowAttack = "Slow Attack";

        public const string ShortRelease = "Short Release";
        public const string LongRelease = "Long Release";

        public const string Static = "Static";
        public const string Moving = "Moving";

        public const string Loop = "Loop";
        public const string OneShot = "One Shot";
    }
}
