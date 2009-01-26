#pragma once

namespace Jacobi {
namespace Vst {
namespace Interop {

class Utils
{
public:
	static void ShowWarning(System::String^ message);
	static void ShowError(System::Exception^ e);
	static System::String^ GetCurrentFileName()
	{ return System::Reflection::Assembly::GetExecutingAssembly()->Location; }
};

}}} // Jacobi::Vst::Interop