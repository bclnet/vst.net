using Jacobi.Vst3.Core;
using Jacobi.Vst3.Core.Test;
using System;
using static Jacobi.Vst3.Utility.Testing;

namespace Jacobi.Vst3.Utility
{
    public static class VersionParserTest
    {
        public static void Touch()
        {
            var _ = InitVersionParserTests;
        }

        static ModuleInitializer InitVersionParserTests = new(() =>
        {
            RegisterTest("VersionParser", "Parsing 'SDK 3.7'", (ITestResult testResult) =>
            {
                var version = VersionX.Parse("SDK 3.7");
                if (version.Major != 3 || version.Minor != 7 || version.Build != 0 || version.MinorRevision != 0)
                {
                    testResult.AddErrorMessage("Parsing 'SDK 3.7' failed");
                    return false;
                }
                return true;
            });
            RegisterTest("VersionParser", "Parsing 'SDK 3.7.1.38'", (ITestResult testResult) =>
            {
                var version = VersionX.Parse("3.7.1.38");
                if (version.Major != 3 || version.Minor != 7 || version.Build != 1 || version.MinorRevision != 38)
                {
                    testResult.AddErrorMessage("Parsing '3.7.1.38' failed");
                    return false;
                }
                return true;
            });
            RegisterTest("VersionParser", "Parsing 'SDK 3.7 Prerelease'", (ITestResult testResult) =>
            {
                var version = VersionX.Parse("SDK 3.7 Prerelease");
                if (version.Major != 3 || version.Minor != 7 || version.Build != 0 || version.MinorRevision != 0)
                {
                    testResult.AddErrorMessage("Parsing 'SDK 3.7 Prerelease' failed");
                    return false;
                }
                return true;
            });
            RegisterTest("VersionParser", "Parsing 'SDK 3.7-99'", (ITestResult testResult) =>
            {
                var version = VersionX.Parse("SDK 3.7-99");
                if (version.Major != 3 || version.Minor != 7 || version.Build != 0 || version.MinorRevision != 0)
                {
                    testResult.AddErrorMessage("Parsing 'SDK 3.7-99' failed");
                    return false;
                }
                return true;
            });
            RegisterTest("VersionParser", "Parsing 'No version at all'", (ITestResult testResult) =>
            {
                var version = VersionX.Parse("No version at all");
                if (version.Major != 0 || version.Minor != 0 || version.Build != 0 || version.MinorRevision != 0)
                {
                    testResult.AddErrorMessage("Parsing 'No version at all' failed");
                    return false;
                }
                return true;
            });
        });
    }
}
