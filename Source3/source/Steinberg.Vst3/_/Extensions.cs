﻿//using System.Linq;

//namespace Jacobi.Vst3
//{
//    public static class Extensions
//    {
//        //
//        // ProgramList
//        //

//        public static Parameter CreateChangeProgramParameter(this ProgramList programs, int unitId = 0)
//        {
//            var valueInfo = new ParameterValueInfo(precision: 0);
//            valueInfo.ParameterInfo.UnitId = unitId;
//            valueInfo.ParameterInfo.Flags = ParameterFlags.IsProgramChange | ParameterFlags.CanAutomate | ParameterFlags.IsList;

//            var listParam = new StringListParameter(valueInfo);
//            listParam.Values.AddRange(programs.Select(p => p.Name));
//            return listParam;
//        }
//    }
//}