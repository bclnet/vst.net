﻿//using System;
//using System.IO;

//namespace Jacobi.Vst3
//{
//    public class VstStreamReader
//    {
//        readonly BStream _stream;

//        public VstStreamReader(IBStream stream)
//            => _stream = new BStream(stream, StreamAccessMode.Read);

//        public VstStreamReader(BStream stream)
//        {
//            if (!stream.CanRead) throw new ArgumentException("Cannot read from stream.", nameof(stream));

//            _stream = stream;
//        }

//        public virtual void ReadParameters(ParameterContainer parameters)
//        {
//            var reader = new BinaryReader(_stream);
//            var count = reader.ReadInt32();

//            for (var i = 0; i < count; i++)
//            {
//                var id = reader.ReadUInt32();
//                var value = reader.ReadDouble();
//                //var param = parameters.GetParameter(id);
//                //if (param != null) param.PlainValue = value;
//            }
//        }

//        public virtual void ReadPrograms(ProgramList programs) => throw new NotImplementedException();

//        //public virtual void ReadUnits(UnitCollection units) => throw new NotImplementedException();
//    }
//}
