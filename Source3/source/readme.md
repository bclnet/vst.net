# VST 3

The thin Interop layer that loads the plugin factory seems to work nicely. **Windows-Only** though.

The managed plugin exposes a public `PluginFactory` derived from the `PluginClassFactory` in Core.

The aim of this library is to be as close to the original C++ API as possible in order for the official docs to apply to `VST.NET3`.

Go here for the official VST3 documentation: https://steinbergmedia.github.io/vst3_doc/

For questions about the VST3 API itself, use the Steinberg forum (just remember they think you're talking C++): https://sdk.steinberg.net/index.php

## Issues

- Do not include the [plugin].deps.json in the deployment. Loading will report seeing the same module with different extensions.
Something is going on with setting the current directory..?
`vsthost.exe` (Hermann Seib) does load the .vst3 plugin correctly without having to copy over Ijwhost.dll...
- The validator.exe points out a few issues:
    - BUG? IEditController::setState passes in a IBStream that is at the end. https://sdk.steinberg.net/viewtopic.php?f=4&t=818



## Research

COM (VST3)
https://github.com/dotnet/runtime/blob/master/docs/design/features/COM-activation.md#NET-Framework-Class-COM-Activation
(we don't need activation)

COM Interop Best Practices
https://docs.microsoft.com/en-us/dotnet/standard/native-interop/best-practices


https://jpassing.com/2009/03/26/rcw-reference-counting-rules-com-reference-counting-rules/
http://dotnetdebug.net/2005/07/07/marshalreleasecomobject-and-cpu-spinning/


## TODOs

- remove methods from interop structs and make them extension methods.
- add default by-pass parameter to audio effects (otherwise it might be interpretted as an instrument)
- Research if TResult return values can be typed instead of `int`. Will `struct TResult { int value; }` work?
- Decide if the API is to be naked (PreserveSig) with all the TResults (as int's) and out/ref parameters? 
    Or that we remove these - the COM wrapper interop can handle that.
- No bools in structs!


https://stackoverflow.com/questions/2187425/how-do-i-use-a-com-dll-with-loadlibrary-in-c
https://steinbergmedia.github.io/vst3_dev_portal/pages/Technical+Documentation/API+Documentation/Index.html
https://steinbergmedia.github.io/vst3_doc/
