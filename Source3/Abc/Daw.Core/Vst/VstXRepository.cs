using System;
using System.IO;
using System.Linq;
using Daw.Core;
using Jacobi.Vst3.Core;
using Jacobi.Vst3.Hosting;
using Jacobi.Vst3.Plugin;

namespace Daw.Vst
{
    public class Vst2Repository
    {
        public void Refresh()
        {
            var option = OptionManager.Value.Vst2;
            var ctx = new Vst2AudioHost();
            var dllCount = option.Dlls.Count;
            foreach (var folder in option.Folders) AddFolder(folder, option, ctx);
            if (dllCount != option.Dlls.Count) OptionManager.Save();
        }

        //:recursive
        void AddFolder(string path, Vst2Option option, Vst2AudioHost ctx)
        {
            if (!Directory.Exists(path)) return;
            foreach (var dir in new DirectoryInfo(path).GetDirectories()) AddFolder(dir.FullName, option, ctx);
            foreach (var file in new DirectoryInfo(path).GetFiles().Where(x => string.Equals(x.Extension, ".dll", StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine($"FOUND: {file.FullName}");
                if (option.Dlls.Contains(file.FullName)) continue;
                var info = ctx.Load(file);
                option.Dlls.Add(file.FullName); // exclude from future search
                if (info == null) continue;
                if ((info.AudioInputs > 0 && info.AudioOutputs > 0) || info.CanReceiveVstMidiEvent) option.Classes.Add(info);
            }
        }
    }

    public class Vst3Repository
    {
        public void Refresh()
        {
            var option = OptionManager.Value.Vst3;
            var dllCount = option.Vst3s.Count;
            foreach (var folder in Module.GetModulePaths()) AddFolder(folder, option);
            if (dllCount != option.Vst3s.Count) OptionManager.Save();
        }

        void AddFolder(string path, Vst3Option option)
        {
            Console.WriteLine($"FOUND: {path}");
            if (option.Vst3s.Contains(path)) return;
            var module = Module.Create(path, out var error);
            option.Vst3s.Add(path);
            if (module == null) { Console.WriteLine($"Could not create Module for file: {path}\nError: {error}"); return; }
            var factory = module.Factory;
            foreach (var info in factory.ClassInfos)
                if (info.Category == Constants.kVstAudioEffectClass) option.Classes.Add(info);
        }
    }
}
