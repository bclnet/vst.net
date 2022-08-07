﻿using Jacobi.Vst3.Core;
using Jacobi.Vst3.Core.ModuleInfo;
using Jacobi.Vst3.Hosting;
using System.Collections.Generic;
using System.IO;

namespace Jacobi.Vst3.TestSuite
{
    /// <summary>
    /// PlugCompat
    /// </summary>
    public class PlugCompat
    {
        public static bool CheckPluginCompatibility(Module module, IPluginCompatibility compat, out string errorStream)
        {
            var failure = false;
            var moduleInfoPath = Module.GetModuleInfoPath(module.Path);
            if (moduleInfoPath != null)
            {
                failure = true;
                errorStream = "Error: The module contains a moduleinfo.json file and the module factory exports a IPluginCompatibility class. Only one is allowed, while the moduleinfo.json one is prefered.\n";
            }

            List<ModuleInfo.Compatibility_> result;
            using var s = new MemoryStream();
            var strStream = new BStream(s);
            if (compat.GetCompatibilityJSON(strStream) != TResult.S_True)
            {
                errorStream = "Error: Call to IPluginCompatiblity::getCompatibilityJSON (IBStream*) failed\n";
                failure = true;
            }
            else if ((result = ModuleInfoLib.ParseCompatibilityJson(s.ToString(), out errorStream)) != null) { } // TODO: Check that the "New" classes are part of the Module;
            else failure = true;
            return !failure;
        }
    }
}
