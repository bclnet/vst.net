#include "pch.h"
#include <pluginterfaces/base/funknown.h>
#include <pluginterfaces/base/ipluginbase.h>

using namespace System;
using namespace Steinberg::Vst3;

bool PLUGIN_API InitDll() { return true; }
bool PLUGIN_API ExitDll() { return true; }
bool PLUGIN_API ManagedDll() { return true; }

IPluginFactory^ LoadPlugin()
{
	auto interopPath = Reflection::Assembly::GetExecutingAssembly()->Location;
	auto pluginPath = IO::Path::GetDirectoryName(interopPath);
	auto pluginName = IO::Path::GetFileNameWithoutExtension(interopPath);

	auto loader = gcnew AssemblyLoader(pluginPath);
	auto pluginAssembly = loader->LoadPlugin(pluginName);

	Type^ pluginType = nullptr;
	for each (auto type in pluginAssembly->GetTypes())
		if (type->IsPublic && type->GetInterface("Steinberg.Vst3.IPluginFactory"))
		{
			pluginType = type;
			break;
		}

	return pluginType != nullptr
		? safe_cast<IPluginFactory^>(Activator::CreateInstance(pluginType))
		: nullptr;
}

Steinberg::IPluginFactory* PLUGIN_API GetPluginFactory()
{
	auto pluginFactory = LoadPlugin();
	if (!pluginFactory) return nullptr;

	auto unknownPtr = Runtime::InteropServices::Marshal::GetComInterfaceForObject(
		pluginFactory, IPluginFactory::typeid);
	if (unknownPtr == IntPtr::Zero) return nullptr;

	auto unknown = (Steinberg::FUnknown*)unknownPtr.ToPointer();
	Steinberg::IPluginFactory* plugin = nullptr;
	unknown->queryInterface(Steinberg::IPluginFactory_iid, (void**)&plugin);
	return plugin;
}
