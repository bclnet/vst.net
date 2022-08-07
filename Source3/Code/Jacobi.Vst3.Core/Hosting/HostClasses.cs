using Jacobi.Vst3.Core;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Jacobi.Vst3.Hosting
{
    /// <summary>
    /// Implementation's example of IHostApplication.
    /// </summary>
    public class HostApplication : IHostApplication
    {
        PlugInterfaceSupport _plugInterfaceSupport = new();

        HostApplication() { }

        //--- IHostApplication ---------------
        public int GetName(StringBuilder name)
        {
            name.Append("My VST3 HostApplication");
            return TResult.S_True;
        }

        HostMessage _hostMessage;
        HostAttributeList _hostAttributeList;
        public int CreateInstance(ref Guid cid, ref Guid iid, out IntPtr obj)
        {
            Type imessage = typeof(IMessage), iattributeList = typeof(IAttributeList);

            obj = default;
            if (cid == imessage.GUID && iid == imessage.GUID)
            {
                var objx = _hostMessage = new HostMessage();
                obj = Marshal.GetComInterfaceForObject(objx, imessage);
                return TResult.S_True;
            }
            else if (cid == iattributeList.GUID && iid == iattributeList.GUID)
            {
                var al = _hostAttributeList = new HostAttributeList();
                if (al != null)
                {
                    obj = Marshal.GetComInterfaceForObject(al, iattributeList);
                    return TResult.S_True;
                }
                return TResult.E_OutOfMemory;
            }
            obj = default;
            return TResult.S_False;
        }

        public PlugInterfaceSupport GetPlugInterfaceSupport() => _plugInterfaceSupport;
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

            public Attribute(byte _) { this = default; type = Type.kUninitialized; }
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

        public int SetInt(string id, long value)
        {
            if (id == null) return TResult.E_InvalidArg;
            list[id] = new Attribute(value);
            return TResult.S_True;
        }

        public int GetInt(string id, out long value)
        {
            if (id == null) { value = default; return TResult.E_InvalidArg; }
            if (list.TryGetValue(id, out var z) && z.GetType() == Attribute.Type.kInteger) { value = z.IntValue(); return TResult.S_True; }
            value = default; return TResult.S_False;
        }

        public int SetFloat(string id, double value)
        {
            if (id == null) return TResult.E_InvalidArg;
            list[id] = new Attribute(value);
            return TResult.S_True;
        }

        public int GetFloat(string id, out double value)
        {
            if (id == null) { value = default; return TResult.E_InvalidArg; }
            if (list.TryGetValue(id, out var z) && z.GetType() == Attribute.Type.kFloat) { value = z.FloatValue(); return TResult.S_True; }
            value = default; return TResult.S_False;
        }

        public int SetString(string id, string str)
        {
            if (id == null) return TResult.E_InvalidArg;
            var length = str.Length + 1; // + 1 for the null-terminate
            list[id] = new Attribute(str, (uint)length);
            return TResult.S_True;
        }

        public int GetString(string id, StringBuilder str, ref uint size)
        {
            if (id == null) { return TResult.E_InvalidArg; }
            if (list.TryGetValue(id, out var z) && z.GetType() == Attribute.Type.kString) { str.Append(z.StringValue(out size)); return TResult.S_True; }
            return TResult.S_False;
        }

        public int SetBinary(string id, IntPtr data, uint size)
        {
            if (id == null) return TResult.E_InvalidArg;
            list[id] = new Attribute(data, size);
            return TResult.S_True;
        }

        public int GetBinary(string id, IntPtr data, ref uint size)
        {
            if (id == null) { return TResult.E_InvalidArg; }
            if (list.TryGetValue(id, out var z) && z.GetType() == Attribute.Type.kBinary) { data = z.BinaryValue(out size); return TResult.S_True; }
            return TResult.S_False;
        }
    }

    /// <summary>
    /// Example implementation of IMessage.
    /// </summary>
    public class HostMessage : IMessage
    {
        string messageId;
        IAttributeList attributeList = new HostAttributeList();

        public string GetMessageID() => messageId;
        public void SetMessageID(string mid) => messageId = mid;
        public IAttributeList GetAttributes() => attributeList;
    }
}
