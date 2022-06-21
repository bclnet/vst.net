using Jacobi.Vst3.Core;
using Jacobi.Vst3.Core.Test;
using Jacobi.Vst3.Host;
using System;

namespace Jacobi.Vst3.TestSuite
{
    /// <summary>
    /// Test Scan Editor Classes.
    /// </summary>
    public class EditorClassesTest : TestBase
    {
        public override string Name => "Scan Editor Classes";

        public EditorClassesTest(ITestPlugProvider plugProvider) : base(plugProvider) { }

        public override bool Run(ITestResult testResult)
        {
            if (testResult == null || vstPlug == null) return false;

            PrintTestHeader(testResult);

            // no controller is allowed...
            if (vstPlug is IEditController)
            {
                testResult.AddMessage("Processor and edit controller united.");
                return true;
            }

            var controllerClassTUID = Guid.Empty;
            if (vstPlug.GetControllerClassId(ref controllerClassTUID) != TResult.S_OK)
            {
                testResult.AddMessage("This component does not export an edit controller class ID!!!");
                return true;
            }
            var controllerClassUID = controllerClassTUID;
            if (controllerClassUID == Guid.Empty)
            {
                testResult.AddErrorMessage("The edit controller class has no valid UID!!!");
                return false;
            }

            testResult.AddMessage("This component has an edit controller class");

            testResult.AddMessage($"   Controller CID: {controllerClassUID}");

            return true;
        }
    }
}
