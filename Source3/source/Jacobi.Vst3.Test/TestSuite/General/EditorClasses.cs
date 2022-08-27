using Jacobi.Vst3;
using System;
using static Jacobi.Vst3.TResult;

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

            if (vstPlug.GetControllerClassId(out var controllerClassTUID) != kResultOk)
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
