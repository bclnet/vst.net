using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Object Data Archive Interface.
    /// - store data/objects/binary/subattributes in the archive
    /// - read stored data from the archive
    /// 
    /// All data stored to the archive are identified by a string (IAttrID), which must be unique on each
    /// IAttribute level.
    /// 
    /// The basic set/get methods make use of the FVariant class defined in 'funknown.h'.
    /// For a more convenient usage of this interface, you should use the functions defined
    /// in namespace PAttributes (public.sdk/source/common/pattributes.h+cpp) !! 
    /// </summary>
    [ComImport, Guid(Interfaces.IAttributes), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IAttributes
    {
        // Methods to write attributes

        /// <summary>
        /// Store any data in the archive. It is even possible to store sub-attributes by creating
	    /// a new IAttributes instance via the IHostClasses interface and pass it to the parent in the
		/// FVariant. In this case the archive must take the ownership of the newly created object, which
		/// is true for all objects that have been created only for storing. You tell the archive to take
		/// ownership by adding the FVariant::kOwner flag to the FVariant::type member (data.type |= FVariant::kOwner).
		/// When using the PAttributes functions, this is done through a function parameter.
        /// </summary>
        /// <param name="attrID"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 Set(
            [MarshalAs(UnmanagedType.LPStr), In] String attrID,
            [MarshalAs(UnmanagedType.Struct), In] ref FVariant data);

        /// <summary>
        /// Store a list of data in the archive. Please note that the type of data is not mixable! So
	    /// you can only store a list of integers or a list of doubles/strings/etc. You can also store a list
		/// of subattributes or other objects that implement the IPersistent interface.
        /// </summary>
        /// <param name="listID"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 Queue(
            [MarshalAs(UnmanagedType.LPStr), In] String listID,
            [MarshalAs(UnmanagedType.Struct), In] ref FVariant data);

        /// <summary>
        /// Store binary data in the archive. Parameter 'copyBytes' specifies if the passed data should be copied.
	    /// The archive cannot take the ownership of binary data. Either it just references a buffer in order
		/// to write it to a file (copyBytes = false) or it copies the data to its own buffers (copyBytes = true).
		/// When binary data should be stored in the default pool for example, you must always copy it!
        /// </summary>
        /// <param name="attrID"></param>
        /// <param name="data"></param>
        /// <param name="bytes"></param>
        /// <param name="copyBytes"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 SetBinaryData(
            [MarshalAs(UnmanagedType.LPStr), In] String attrID,
            [MarshalAs(UnmanagedType.SysInt), In] IntPtr data,
            [MarshalAs(UnmanagedType.U4), In] UInt32 bytes,
            [MarshalAs(UnmanagedType.U1), In] Boolean copyBytes);

        // Methods to read attributes 

        /// <summary>
        /// Get data previously stored to the archive.
        /// </summary>
        /// <param name="attrID"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 Get(
            [MarshalAs(UnmanagedType.LPStr), In] String attrID,
            [MarshalAs(UnmanagedType.Struct), In, Out] ref FVariant data);

        /// <summary>
        /// Get list of data previously stored to the archive. As long as there are queue members the method
	    /// will return kResultTrue. When the queue is empty, the methods returns kResultFalse. All lists except from
		/// object lists can be reset which means that the items can be read once again. \see IAttributes::resetQueue
        /// </summary>
        /// <param name="listID"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 Unqueue(
            [MarshalAs(UnmanagedType.LPStr), In] String listID,
            [MarshalAs(UnmanagedType.Struct), In, Out] ref FVariant data);

        /// <summary>
        /// Get the amount of items in a queue.
        /// </summary>
        /// <param name="attrID"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.I4)]
        Int32 GetQueueItemCount(
            [MarshalAs(UnmanagedType.LPStr), In] String attrID);

        /// <summary>
        /// Reset a queue. If you need to restart reading a queue, you have to reset it. You can reset a queue at any time.
        /// </summary>
        /// <param name="attrID"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 ResetQueue(
            [MarshalAs(UnmanagedType.LPStr), In] String attrID);

        /// <summary>
        /// Reset all queues in the archive.
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 ResetAllQueues();

        /// <summary>
        /// Read binary data from the archive. The data is copied into the passed buffer. The size of that buffer
	    /// must fit the size of data stored in the archive which can be queried via IAttributes::getBinaryDataSize
        /// </summary>
        /// <param name="attrID"></param>
        /// <param name="data"></param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        Int32 GetBinaryData(
            [MarshalAs(UnmanagedType.LPStr), In] String attrID,
            [MarshalAs(UnmanagedType.SysInt), In, Out] ref IntPtr data,
            [MarshalAs(UnmanagedType.U4), In] UInt32 bytes);

        /// <summary>
        /// Get the size in bytes of binary data in the archive.
        /// </summary>
        /// <param name="attrID"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.U4)]
        UInt32 GetBinaryDataSize(
            [MarshalAs(UnmanagedType.LPStr), In] String attrID);
    }

    static partial class Interfaces
    {
        public const string IAttributes = "FA1E32F9-CA6D-46F5-A982-F956B1191B58";
    }

    /// <summary>
    /// Extended access to Attributes; supports Attribute retrieval via iteration.
    /// </summary>
    [ComImport, Guid(Interfaces.IAttributes2), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IAttributes2 : IAttributes
    {
        //[PreserveSig]
        //[return: MarshalAs(UnmanagedType.Error)]
        //Int32 Set(
        //    [MarshalAs(UnmanagedType.LPStr), In] String attrID,
        //    [MarshalAs(UnmanagedType.Struct), In] ref FVariant data);

        //[PreserveSig]
        //[return: MarshalAs(UnmanagedType.Error)]
        //Int32 Queue(
        //    [MarshalAs(UnmanagedType.LPStr), In] String listID,
        //    [MarshalAs(UnmanagedType.Struct), In] ref FVariant data);

        //[PreserveSig]
        //[return: MarshalAs(UnmanagedType.Error)]
        //Int32 SetBinaryData(
        //    [MarshalAs(UnmanagedType.LPStr), In] String attrID,
        //    [MarshalAs(UnmanagedType.SysInt), In] IntPtr data,
        //    [MarshalAs(UnmanagedType.U4), In] UInt32 bytes,
        //    [MarshalAs(UnmanagedType.U1), In] Boolean copyBytes);

        //[PreserveSig]
        //[return: MarshalAs(UnmanagedType.Error)]
        //Int32 Get(
        //    [MarshalAs(UnmanagedType.LPStr), In] String attrID,
        //    [MarshalAs(UnmanagedType.Struct), In, Out] ref FVariant data);

        //[PreserveSig]
        //[return: MarshalAs(UnmanagedType.Error)]
        //Int32 Unqueue(
        //    [MarshalAs(UnmanagedType.LPStr), In] String listID,
        //    [MarshalAs(UnmanagedType.Struct), In, Out] ref FVariant data);

        //[PreserveSig]
        //[return: MarshalAs(UnmanagedType.I4)]
        //Int32 GetQueueItemCount(
        //    [MarshalAs(UnmanagedType.LPStr), In] String attrID);

        //[PreserveSig]
        //[return: MarshalAs(UnmanagedType.Error)]
        //Int32 ResetQueue(
        //    [MarshalAs(UnmanagedType.LPStr), In] String attrID);

        //[PreserveSig]
        //[return: MarshalAs(UnmanagedType.Error)]
        //Int32 ResetAllQueues();

        //[PreserveSig]
        //[return: MarshalAs(UnmanagedType.Error)]
        //Int32 GetBinaryData(
        //    [MarshalAs(UnmanagedType.LPStr), In] String attrID,
        //    [MarshalAs(UnmanagedType.SysInt), In, Out] ref IntPtr data,
        //    [MarshalAs(UnmanagedType.U4), In] UInt32 bytes);

        //[PreserveSig]
        //[return: MarshalAs(UnmanagedType.U4)]
        //UInt32 GetBinaryDataSize(
        //    [MarshalAs(UnmanagedType.LPStr), In] String attrID);

        //---------------------------------------------------------------------

        /// <summary>
        /// Returns the number of existing attributes.
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.I4)]
        Int32 CountAttributes();

        /// <summary>
        /// Returns the attribute's ID for the given index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.LPStr)]
        String GetAttributeID(
            [MarshalAs(UnmanagedType.I4), In] Int32 index);
    }

    static partial class Interfaces
    {
        public const string IAttributes2 = "1382126A-FECA-4871-97D5-2A45B042AE99";
    }
}
