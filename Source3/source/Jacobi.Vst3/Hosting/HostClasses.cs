﻿using Jacobi.Vst3;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using static Jacobi.Vst3.TResult;

namespace Jacobi.Vst3.Hosting
{
    /// <summary>
    /// Implementation's example of IHostApplication.
    /// </summary>
    public class HostApplication : IHostApplication
    {
        PlugInterfaceSupport mPlugInterfaceSupport = new();

        public HostApplication() { }

        //--- IHostApplication ---------------
        //TODO: Change StringBuilder
        public TResult GetName(StringBuilder name)
        {
            name.Append("My VST3 HostApplication");
            return kResultTrue;
        }

        HostMessage _hostMessage;
        HostAttributeList _hostAttributeList;
        public TResult CreateInstance(ref Guid cid, ref Guid iid, out IntPtr obj)
        {
            Type imessage = typeof(IMessage), iattributeList = typeof(IAttributeList);

            obj = default;
            if (cid == imessage.GUID &&
                iid == imessage.GUID)
            {
                var objx = _hostMessage = new HostMessage();
                obj = Marshal.GetComInterfaceForObject(objx, imessage);
                return kResultTrue;
            }
            else if (cid == iattributeList.GUID &&
                iid == iattributeList.GUID)
            {
                var al = _hostAttributeList = new HostAttributeList();
                if (al != null)
                {
                    obj = Marshal.GetComInterfaceForObject(al, iattributeList);
                    return kResultTrue;
                }
                return kOutOfMemory;
            }
            obj = default;
            return kResultFalse;
        }

        public PlugInterfaceSupport GetPlugInterfaceSupport()
            => mPlugInterfaceSupport;
    }

    /// <summary>
    /// Example implementation of IMessage.
    /// </summary>
    public class HostMessage : IMessage
    {
        string messageId;
        IAttributeList attributeList;

        public string GetMessageID() => messageId;
        public void SetMessageID(string mid) => messageId = mid;
        public IAttributeList GetAttributes() => attributeList != null
            ? attributeList
            : attributeList = new HostAttributeList();
    }

    /// <summary>
    /// Example, ready to use implementation of IAttributeList.
    /// </summary>
    public class HostAttributeList : IAttributeList
    {
        [StructLayout(LayoutKind.Auto)]
        public struct Attribute
        {
            public enum Type : int
            {
                kUninitialized,
                kInteger,
                kFloat,
                kString,
                kBinary
            }

            uint size;
            Type type;
            long v_intValue;
            double v_floatValue;
            string v_stringValue;
            IntPtr v_binaryValue;

            public Attribute(object _) { this = default; type = Type.kUninitialized; }
            public Attribute(long value) { this = default; type = Type.kInteger; v_intValue = value; }
            public Attribute(double value) { this = default; type = Type.kFloat; v_floatValue = value; }
            public Attribute(string value, uint sizeInCodeUnit) { this = default; size = sizeInCodeUnit; type = Type.kString; v_stringValue = value; }
            public Attribute(IntPtr value, uint sizeInBytes) { this = default; size = sizeInBytes; type = Type.kBinary; v_binaryValue = value; }
            public Attribute(Attribute o) { this = o; }

            public long IntValue() => v_intValue;
            public double FloatValue() => v_floatValue;
            public string StringValue(out uint sizeInCodeUnit) { sizeInCodeUnit = size; return v_stringValue; }
            public IntPtr BinaryValue(out uint sizeInBytes) { sizeInBytes = size; return v_binaryValue; }
            public new Type GetType() => type;
        }

        Dictionary<string, Attribute> list = new();

        public TResult SetInt(string aid, long value)
        {
            if (aid == null) return kInvalidArgument;
            list[aid] = new Attribute(value);
            return kResultTrue;
        }

        public TResult GetInt(string aid, out long value)
        {
            if (aid == null) { value = default; return kInvalidArgument; }
            if (list.TryGetValue(aid, out var z) && z.GetType() == Attribute.Type.kInteger) { value = z.IntValue(); return kResultTrue; }
            value = default; return kResultFalse;
        }

        public TResult SetFloat(string aid, double value)
        {
            if (aid == null) return kInvalidArgument;
            list[aid] = new Attribute(value);
            return kResultTrue;
        }

        public TResult GetFloat(string aid, out double value)
        {
            if (aid == null) { value = default; return kInvalidArgument; }
            if (list.TryGetValue(aid, out var z) && z.GetType() == Attribute.Type.kFloat) { value = z.FloatValue(); return kResultTrue; }
            value = default; return kResultFalse;
        }

        public TResult SetString(string aid, string str)
        {
            if (aid == null) return kInvalidArgument;
            var length = str.Length + 1; // + 1 for the null-terminate
            list[aid] = new Attribute(str, (uint)length);
            return kResultTrue;
        }

        public TResult GetString(string aid, out string value, ref uint size)
        {
            if (aid == null) { value = default; return kInvalidArgument; }
            if (list.TryGetValue(aid, out var z) && z.GetType() == Attribute.Type.kString) { value = z.StringValue(out size); return kResultTrue; }
            value = default; return kResultFalse;
        }

        //TODO: remove StringBuilder?
        public TResult GetString(string aid, StringBuilder str, ref uint size)
        {
            if (aid == null) { return kInvalidArgument; }
            if (list.TryGetValue(aid, out var z) && z.GetType() == Attribute.Type.kString) { str.Append(z.StringValue(out size)); return kResultTrue; }
            return kResultFalse;
        }

        public TResult SetBinary(string aid, IntPtr data, uint size)
        {
            if (aid == null) return kInvalidArgument;
            list[aid] = new Attribute(data, size);
            return kResultTrue;
        }

        public TResult GetBinary(string aid, IntPtr data, ref uint size)
        {
            if (aid == null) { return kInvalidArgument; }
            if (list.TryGetValue(aid, out var z) && z.GetType() == Attribute.Type.kBinary) { data = z.BinaryValue(out size); return kResultTrue; }
            return kResultFalse;
        }
    }
}
