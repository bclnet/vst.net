﻿//using System;
//using System.IO;

//namespace Jacobi.Vst3
//{
//    public class VstStreamWriter
//    {
//        readonly BStream _stream;

//        public VstStreamWriter(IBStream stream)
//            => _stream = new BStream(stream, StreamAccessMode.Write);

//        public VstStreamWriter(BStream stream)
//        {
//            if (!stream.CanWrite) throw new ArgumentException("Cannot write to stream.", nameof(stream));

//            _stream = stream;
//        }

//        public virtual void WriteParameters(ParameterContainer parameters)
//        {
//            var writer = new BinaryWriter(_stream);
//            writer.Write(parameters.ParameterCount);

//            //foreach (var parameter in parameters)
//            //{
//            //    writer.Write(parameter.Id);
//            //    writer.Write(parameter.PlainValue);
//            //}
//        }

//        public virtual void WritePrograms(ProgramList programs) => throw new NotImplementedException();

//        //public virtual void WriteUnits(UnitCollection units) => throw new NotImplementedException();
//    }
//}