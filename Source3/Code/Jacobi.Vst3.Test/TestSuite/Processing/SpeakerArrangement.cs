using Jacobi.Vst3.Core;
using Jacobi.Vst3.Core.Test;
using static Jacobi.Vst3.Core.SpeakerArrangement;

namespace Jacobi.Vst3.TestSuite
{
    /// <summary>
    /// Test Speaker Arrangement.
    /// </summary>
    public class SpeakerArrangementTest : ProcessTest
    {
        SpeakerArrangement inSpArr;
        SpeakerArrangement outSpArr;

        public SpeakerArrangementTest(ITestPlugProvider plugProvider, SymbolicSampleSizes sampl, SpeakerArrangement inSpArr, SpeakerArrangement outSpArr) : base(plugProvider, sampl)
        {
            this.inSpArr = inSpArr;
            this.outSpArr = outSpArr;
        }

        static string GetSpeakerArrangementName(SpeakerArrangement spArr)
            => spArr switch
            {
                kMono => "Mono",
                kStereo => "Stereo",
                kStereoSurround => "StereoSurround",
                kStereoCenter => "StereoCenter",
                kStereoSide => "StereoSide",
                kStereoCLfe => "StereoCLfe",
                k30Cine => "30Cine",
                k30Music => "30Music",
                k31Cine => "31Cine",
                k31Music => "31Music",
                k40Cine => "40Cine",
                k40Music => "40Music",
                k41Cine => "41Cine",
                k41Music => "41Music",
                k50 => "50",
                k51 => "51",
                k60Cine => "60Cine",
                k60Music => "60Music",
                k61Cine => "61Cine",
                k61Music => "61Music",
                k70Cine => "70Cine",
                k70Music => "70Music",
                k71Cine => "71Cine",
                k71Music => "71Music",
                k80Cine => "80Cine",
                k80Music => "80Music",
                k81Cine => "81Cine",
                k81Music => "81Music",
                k102 => "102",
                k122 => "122",
                k80Cube => "80Cube",
                k90 => "9.0",
                k91 => "9.1",
                k100 => "10.0",
                k101 => "10.1",
                k110 => "11.0",
                k111 => "11.1",
                k130 => "13.0",
                k131 => "13.1",
                k222 => "22.2",
                kEmpty => "Empty",
                _ => "Unknown",
            };

        public override string Name
        {
            get
            {
                var inSaName = GetSpeakerArrangementName(inSpArr);
                var outSaName = GetSpeakerArrangementName(outSpArr);
                return inSaName != null && outSaName != null
                    ? $"In: {inSaName}: {inSpArr.GetChannelCount()} Channels, Out: {outSaName}: {outSpArr.GetChannelCount()} Channels"
                    : "error";
            }
        }

        protected override bool PrepareProcessing()
        {
            if (vstPlug == null || audioEffect == null) return false;

            var ret = true;
            var is_ = vstPlug.GetBusCount(MediaTypes.Audio, BusDirections.Input);
            var inSpArrs = new SpeakerArrangement[is_];
            for (var i = 0; i < is_; ++i) inSpArrs[i] = inSpArr;

            var os = vstPlug.GetBusCount(MediaTypes.Audio, BusDirections.Output);
            var outSpArrs = new SpeakerArrangement[os];
            for (var o = 0; o < os; o++) outSpArrs[o] = outSpArr;

            if (audioEffect.SetBusArrangements(inSpArrs, is_, outSpArrs, os) != TResult.S_True) ret = false;

            // activate only the extra IO (index > 0), the main ones (index 0) were already activated in TestBase::setup ()
            for (var i = 1; i < is_; i++) vstPlug.ActivateBus(MediaTypes.Audio, BusDirections.Input, i, true);
            for (var i = 1; i < os; i++) vstPlug.ActivateBus(MediaTypes.Audio, BusDirections.Output, i, true);

            ret &= base.PrepareProcessing();

            inSpArrs = null;
            outSpArrs = null;

            return ret;
        }

        public override bool Run(ITestResult testResult)
        {
            if (testResult == null || audioEffect == null || vstPlug == null) return false;

            PrintTestHeader(testResult);

            var spArr = kEmpty;
            var compareSpArr = kEmpty;
            var bd = BusDirections.Input;
            var busInfo = new BusInfo();
            var count = 0;
            do
            {
                count++;
                var numBusses = 0;
                if (bd == BusDirections.Input) { numBusses = processData._.NumInputs; compareSpArr = inSpArr; }
                else { numBusses = processData._.NumOutputs; compareSpArr = outSpArr; }
                for (var i = 0; i < numBusses; ++i)
                {
                    if (audioEffect.GetBusArrangement(bd, i, spArr) != TResult.S_True)
                    {
                        testResult.AddErrorMessage("IAudioProcessor::getBusArrangement (..) failed.");
                        return false;
                    }
                    if (spArr != compareSpArr)
                        testResult.AddMessage($"    {GetSpeakerArrangementName(compareSpArr)} {(bd == BusDirections.Input ? "Input-" : "Output-")}SpeakerArrangement is not supported. Plug-in suggests: {GetSpeakerArrangementName(spArr)}.");
                    if (vstPlug.GetBusInfo(MediaTypes.Audio, bd, i, busInfo) != TResult.S_True)
                    {
                        testResult.AddErrorMessage("IComponent::getBusInfo (..) failed.");
                        return false;
                    }
                    if (spArr == compareSpArr && spArr.GetChannelCount() != busInfo.ChannelCount)
                    {
                        testResult.AddErrorMessage("SpeakerArrangement mismatch (BusInfo::channelCount inconsistency).");
                        return false;
                    }
                }
                bd = BusDirections.Output;
            } while (count < 2);

            var ret = true;
            // not a Pb ret &= verifySA (processData.numInputs, processData.inputs, inSpArr, testResult);
            // not a Pb ret &= verifySA (processData.numOutputs, processData.outputs, outSpArr, testResult);
            ret &= base.Run(testResult);

            return ret;
        }
    }
}
