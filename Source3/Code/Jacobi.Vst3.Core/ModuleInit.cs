using System.Collections.Generic;
using System.Linq;
using ModuleInitFunction = System.Action;

namespace Jacobi.Vst3.Core
{
    public struct ModuleInit
    {
        static readonly List<(int first, ModuleInitFunction second)> InitVector = new();
        static readonly List<(int first, ModuleInitFunction second)> TermVector = new();
        public static void AddInitFunction(ModuleInitFunction func, int prio) => InitVector.Add((prio, func));
        public static void AddTerminateFunction(ModuleInitFunction func, int prio) => TermVector.Add((prio, func));
        static bool SortAndRunFunctions(List<(int first, ModuleInitFunction second)> array)
        {
            foreach ((int first, ModuleInitFunction second) in array.OrderBy(x => x.first)) second();
            return true;
        }
        public static bool InitModule() => SortAndRunFunctions(InitVector);
        public static bool DeinitModule() => SortAndRunFunctions(TermVector);
    }

    public class ModuleInitializer
    {
        /// <summary>
        /// Register a function which is called when the module is loaded
        /// </summary>
        /// <param name="func">function to call</param>
        /// <param name="prio">priority</param>
        public ModuleInitializer(ModuleInitFunction func, int prio = 10000) => ModuleInit.AddInitFunction(func, prio);
    }

    public class ModuleTerminator
    {
        /// <summary>
        /// Register a function which is called when the module is unloaded
        /// </summary>
        /// <param name="func">function to call</param>
        /// <param name="prio">priority</param>
        public ModuleTerminator(ModuleInitFunction func, int prio = 10000) => ModuleInit.AddTerminateFunction(func, prio);
    }
}
