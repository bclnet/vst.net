﻿using static Jacobi.Vst3.Utility.Testing;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Utility
{
    /*
     * From: https://docs.microsoft.com/en-us/dotnet/standard/native-interop/best-practices
     * 
     * If the struct is blittable, use sizeof() instead of Marshal.SizeOf<MyStruct>() for better performance. 
     * You can validate that the type is blittable by attempting to create a pinned GCHandle. 
     * If the type is not a string or considered blittable, GCHandle.Alloc will throw an ArgumentException.
     */
    public static class BlittableStructsTest
    {
        public static void Touch() { var _ = InitTests; }

        static ModuleInitializer InitTests = new(() =>
        {
            RegisterTest("BlittableStructs", "Blittable Structs", (ITestResult testResult) =>
            {
                //AssertIsBlittable(new AudioBusBuffers());
                AssertIsBlittable(new Chord());
                AssertIsBlittable(new DataEvent());
                AssertIsBlittable(new NoteExpressionValueDescription());
                AssertIsBlittable(new NoteExpressionValueEvent());
                AssertIsBlittable(new NoteOffEvent());
                AssertIsBlittable(new NoteOnEvent());
                AssertIsBlittable(new PolyPressureEvent());
                AssertIsBlittable(new ProcessContext());
                AssertIsBlittable(new ProcessData());
                AssertIsBlittable(new ProcessSetup());
                AssertIsBlittable(new RoutingInfo());
                AssertIsBlittable(new ViewRect());

                // contains string
                //AssertIsBlittable(new BusInfo());
                //AssertIsBlittable(new KeyswitchInfo());
                //AssertIsBlittable(new NoteExpressionTextEvent());
                //AssertIsBlittable(new NoteExpressionTypeInfo());
                //AssertIsBlittable(new ParameterInfo());
                //AssertIsBlittable(new PClassInfo());
                //AssertIsBlittable(new PClassInfo2());
                //AssertIsBlittable(new PClassInfoW());
                //AssertIsBlittable(new PFactoryInfo());
                //AssertIsBlittable(new ProgramListInfo());
                //AssertIsBlittable(new RepresentationInfo());
                //AssertIsBlittable(new UnitInfo());

                // structs with unions: won't load
                // => Because a string cannot overlap with primitive types (ints etc)
                AssertIsBlittable(new Event());
                //AssertIsBlittable(new FVariant());

                return true;
            });
        });

        static void AssertIsBlittable<T>(T uot) where T : struct
        {
            var handle = GCHandle.Alloc(uot, GCHandleType.Pinned);
            var abc = handle != null;
        }
    }
}
