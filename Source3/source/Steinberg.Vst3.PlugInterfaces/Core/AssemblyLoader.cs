using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;

namespace Steinberg.Vst3
{
    public sealed class AssemblyLoader : IDisposable
    {
        readonly string _basePath;
        readonly AssemblyLoadContext _context;

        public AssemblyLoader(string basePath)
        {
            _basePath = basePath;

            _context = AssemblyLoadContext.GetLoadContext(GetType().Assembly);
            _context.Resolving += LoadContext_ResolvingAssembly;
        }

        public void Dispose()
            => _context.Resolving -= LoadContext_ResolvingAssembly;

        public Assembly LoadPlugin(string pluginName)
            => LoadAssembly($"{pluginName}.dll");

        public Assembly LoadAssembly(string fileName)
        {
            var filePath = Path.Combine(_basePath, fileName);
            return File.Exists(filePath) ? _context.LoadFromAssemblyPath(filePath) : null;
        }

        Assembly LoadContext_ResolvingAssembly(AssemblyLoadContext assemblyLoadContext, AssemblyName assemblyName)
            => LoadAssembly($"{assemblyName.Name}.dll");
    }
}
