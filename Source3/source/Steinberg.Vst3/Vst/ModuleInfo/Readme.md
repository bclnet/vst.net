
# ModuleInfoLib

This is a c# library to parse and create the Steinberg moduleinfo.json files.

## Parsing

Now to parse a moduleinfo.json file in code you need to read the moduleinfo.json into a memory buffer and call

``` c#
var moduleInfo = ModuleInfoLib.ParseCompatibilityJson(std::string_view (buffer, bufferSize), &std::cerr);
```

Afterwards if parsing succeeded the moduleInfo optional has a value containing the ModuleInfo.

## Creating

The VST3 SDK contains the moduleinfotool utility that can create moduleinfo.json files from VST3 modules.

Now you can use the two methods in moduleinfocreator.h to create a moduleinfo.json file:

``` c#
var moduleInfo = ModuleInfoLib.CreateModuleInfo(module, false);
ModuleInfoLib.outputJson(moduleInfo, std::cout);
```
