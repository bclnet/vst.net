using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static Jacobi.Vst3.PresetFile.ChunkType;
using static Jacobi.Vst3.TResult;
using ChunkID = System.Int32;
using IOFileStream = System.IO.FileStream;
using ProgramListID = System.Int32;
using TSize = System.Int64;
using UnitID = System.Int32;

namespace Jacobi.Vst3
{
    //------------------------------------------------------------------------
    /* 
        VST 3 Preset File Format Definition
       ===================================

    0   +---------------------------+
        | HEADER                    |
        | header id ('VST3')        |       4 Bytes
        | version                   |       4 Bytes (int32)
        | ASCII-encoded class id    |       32 Bytes 
     +--| offset to chunk list      |       8 Bytes (int64)
     |  +---------------------------+
     |  | DATA AREA                 |<-+
     |  | data of chunks 1..n       |  |
     |  ...                       ...  |
     |  |                           |  |
     +->+---------------------------+  |
        | CHUNK LIST                |  |
        | list id ('List')          |  |    4 Bytes
        | entry count               |  |    4 Bytes (int32)
        +---------------------------+  |
        |  1..n                     |  |
        |  +----------------------+ |  |
        |  | chunk id             | |  |    4 Bytes
        |  | offset to chunk data |----+    8 Bytes (int64)
        |  | size of chunk data   | |       8 Bytes (int64)
        |  +----------------------+ |
    EOF +---------------------------+
    */

    /// <summary>
    /// Handler for a VST 3 Preset File.
    /// </summary>
    public unsafe class PresetFile
    {
        //const int sizeof_ChunkID = 4;

        IBStream stream;
        Guid classID;       ///< classID is the FUID of the component (processor) part
        const int kMaxEntries = 128;
        readonly Entry[] entries = Enumerable.Repeat(new Entry(), kMaxEntries).ToArray();
        int entryCount;

        static readonly ChunkID[] commonChunks = {
            'V' << 24 | 'S' << 16 | 'T' << 8 | '3',	// kHeader
	        'C' << 24 | 'o' << 16 | 'm' << 8 | 'p',	// kComponentState
	        'C' << 24 | 'o' << 16 | 'n' << 8 | 't',	// kControllerState
	        'P' << 24 | 'r' << 16 | 'o' << 8 | 'g',	// kProgramData
	        'I' << 24 | 'n' << 16 | 'f' << 8 | 'o',	// kMetaInfo
	        'L' << 24 | 'i' << 16 | 's' << 8 | 't'	// kChunkList
        };

        // Preset Header: header id + version + class id + list offset
        const int kFormatVersion = 1;
        const int kClassIDSize = 32; // ASCII-encoded FUID
        const int kHeaderSize = sizeof(ChunkID) + sizeof(int) + kClassIDSize + sizeof(TSize);
        const int kListOffsetPos = kHeaderSize - sizeof(TSize);

        public enum ChunkType
        {
            kHeader,
            kComponentState,
            kControllerState,
            kProgramData,
            kMetaInfo,
            kChunkList,
            kNumPresetChunks
        }

        // Internal structure used for chunk handling
        public class Entry
        {
            public ChunkID id;
            public TSize offset;
            public TSize size;
        }

        public static ref ChunkID GetChunkID(ChunkType type) => ref commonChunks[(int)type];
        public static bool IsEqualID(ChunkID id1, ChunkID id2) => id1 == id2;

        static bool Verify(TResult result) => result == kResultOk || result == kNotImplemented;

        static bool CopyStream(IBStream inStream, IBStream outStream)
        {
            if (inStream == null || outStream == null) return false;

            var buffer = stackalloc byte[8192];
            while (inStream.Read((IntPtr)buffer, 8192, out var read) == kResultTrue && read > 0)
                if (outStream.Write((IntPtr)buffer, read, out var written) != kResultTrue)
                    return false;
            return true;
        }

        public PresetFile(IBStream stream) ///< Constructor of Preset file based on a stream
        {
            this.stream = stream;
            //memset(entries, 0, sizeof(entries));
            //if (stream) stream->addRef();
        }
        public void Dispose()
        {
            //if (stream != null) stream.Release();
        }

        public IBStream GetStream() => stream;            ///< Returns the associated stream.

        public Guid GetClassID() => classID;   ///< Returns the associated classID (component ID: Processor part (not the controller!)).
        public void SetClassID(Guid uid) => classID = uid; ///< Sets the associated classID (component ID: Processor part (not the controller!)).

        public Entry GetEntry(ChunkType which)        ///< Returns an entry for a given chunk type.
        {
            var id = GetChunkID(which);
            for (var i = 0; i < entryCount; i++)
                if (IsEqualID(entries[i].id, id))
                    return entries[i];
            return null;
        }
        public Entry GetLastEntry()                   ///< Returns the last available entry.
            => entryCount > 0 ? entries[entryCount - 1] : null;
        public int GetEntryCount() => entryCount;   ///< Returns the number of total entries in the current stream.
        public Entry At(int index) => entries[index];  ///< Returns the entry at a given position.
        public bool Contains(ChunkType which) => GetEntry(which) != null;  ///< Checks if a given chunk type exist in the stream.

        protected bool ReadID(out ChunkID id)
        {
            id = default;
            fixed (int* _ = &id)
            {
                stream.Read((IntPtr)_, sizeof(ChunkID), out var numBytesRead);
                return numBytesRead == sizeof(ChunkID);
            }
        }
        protected bool WriteID(ChunkID id)
        {
            stream.Write((IntPtr)(&id), sizeof(ChunkID), out var numBytesWritten);
            return numBytesWritten == sizeof(ChunkID);
        }

        protected bool ReadEqualID(ChunkID id)
            => ReadID(out var temp) && IsEqualID(temp, id);

        protected bool ReadSize(out TSize size)
        {
            size = default;
            fixed (TSize* _ = &size)
            {
                stream.Read((IntPtr)_, sizeof(TSize), out var numBytesRead);
                if (!BitConverter.IsLittleEndian) Platform.Swap64(_);
                return numBytesRead == sizeof(TSize);
            }
        }
        protected bool WriteSize(TSize size)
        {
            if (!BitConverter.IsLittleEndian) Platform.Swap64(&size);
            stream.Write((IntPtr)(&size), sizeof(TSize), out var numBytesWritten);
            return numBytesWritten == sizeof(TSize);
        }
        protected bool ReadInt32(out int value)
        {
            value = default;
            fixed (int* _ = &value)
            {
                stream.Read((IntPtr)_, sizeof(int), out var numBytesRead);
                if (!BitConverter.IsLittleEndian) Platform.Swap32(_);
                return numBytesRead == sizeof(int);
            }
        }
        protected bool WriteInt32(int value)
        {
            if (!BitConverter.IsLittleEndian) Platform.Swap32(&value);
            stream.Write((IntPtr)(&value), sizeof(int), out var numBytesWritten);
            return numBytesWritten == sizeof(int);
        }
        protected bool SeekTo(TSize offset)
        {
            stream.Seek(offset, SeekOrigin.Begin, out var result);
            return result == offset;
        }

        protected bool BeginChunk(Entry e, ChunkType which)
        {
            if (entryCount >= kMaxEntries)
                return false;

            var id = GetChunkID(which);
            e.id = id;
            stream.Tell(out e.offset);
            e.size = 0;
            return true;
        }

        protected bool EndChunk(Entry e)
        {
            if (entryCount >= kMaxEntries)
                return false;

            stream.Tell(out var pos);
            e.size = pos - e.offset;
            entries[entryCount++] = e;
            return true;
        }

        public bool ReadChunkList()       ///< Reads and build the chunk list (including the header chunk).
        {
            SeekTo(0);
            entryCount = 0;

            var classString = stackalloc byte[kClassIDSize + 1];

            // Read header
            if (!(ReadEqualID(GetChunkID(kHeader)) && ReadInt32(out var version) &&
                  Verify(stream.Read((IntPtr)classString, kClassIDSize, out var z)) && ReadSize(out var listOffset) &&
                  listOffset > 0 && SeekTo(listOffset)))
                return false;
            classID = new Guid(Encoding.ASCII.GetString(classString, kClassIDSize));

            // Read list
            var count = 0;
            if (!ReadEqualID(GetChunkID(kChunkList)))
                return false;
            if (!ReadInt32(out count))
                return false;

            if (count > kMaxEntries)
                count = kMaxEntries;

            for (var i = 0; i < count; i++)
            {
                var e = entries[i];
                if (!(ReadID(out e.id) && ReadSize(out e.offset) && ReadSize(out e.size)))
                    break;

                entryCount++;
            }

            return entryCount > 0;
        }

        public bool WriteHeader()     ///< Writes into the stream the main header.
        {
            // header id + version + class id + list offset (unknown yet)
            fixed (byte* classString = Encoding.ASCII.GetBytes(classID.ToString().Replace("-", "")))
                return SeekTo(0) && WriteID(GetChunkID(kHeader)) && WriteInt32(kFormatVersion) &&
                       Verify(stream.Write((IntPtr)classString, kClassIDSize, out var z)) && WriteSize(0);
        }

        public bool WriteChunkList()      ///< Writes into the stream the chunk list (should be at the end).
        {
            // Update list offset
            stream.Tell(out var pos);
            if (!(SeekTo(kListOffsetPos) && WriteSize(pos) && SeekTo(pos)))
                return false;

            // Write list
            if (!WriteID(GetChunkID(kChunkList)))
                return false;
            if (!WriteInt32(entryCount))
                return false;

            for (var i = 0; i < entryCount; i++)
            {
                var e = entries[i];
                if (!(WriteID(e.id) && WriteSize(e.offset) && WriteSize(e.size)))
                    return false;
            }
            return true;
        }

        // Reads the meta XML info and its size, the size could be retrieved by passing zero as xmlBuffer.
        public bool ReadMetaInfo(StringBuilder xmlBuffer, ref int size)
        {
            var result = false;
            var e = GetEntry(kMetaInfo);
            if (e != null)
            {
                if (xmlBuffer != null)
                {
                    var buf = new char[size];
                    fixed (char* _ = buf)
                        result = SeekTo(e.offset) && Verify(stream.Read((IntPtr)_, size, out size));
                    xmlBuffer.Append(new string(buf));
                }
                else
                {
                    size = (int)e.size;
                    result = size > 0;
                }
            }
            return result;
        }

        // Writes the meta XML info, -1 means null-terminated, forceWriting to true will force to rewrite the XML Info when the chunk already exists.
        public bool WriteMetaInfo(StringBuilder xmlBuffer, int size = -1, bool forceWriting = false)
        {
            if (Contains(kMetaInfo)) // already exists!
            {
                if (!forceWriting)
                    return false;
            }
            if (!PrepareMetaInfoUpdate())
                return false;

            if (size == -1)
                size = xmlBuffer.Length;

            Entry e = new();
            fixed (char* _ = xmlBuffer.ToString())
                return BeginChunk(e, kMetaInfo) && Verify(stream.Write((IntPtr)_, size, out var z)) &&
                       EndChunk(e);
        }

        public bool PrepareMetaInfoUpdate()   ///< checks if meta info chunk is the last one and jump to correct position.
        {
            TSize writePos = 0;
            var e = GetEntry(kMetaInfo);
            if (e != null)
            {
                // meta info must be the last entry!
                if (e != GetLastEntry())
                    return false;

                writePos = e.offset;
                entryCount--;
            }
            else
            {
                // entries must be sorted ascending by offset!
                e = GetLastEntry();
                writePos = e != null ? e.offset + e.size : kHeaderSize;
            }

            return SeekTo(writePos);
        }

        // Writes a given data of a given size as "which" chunk type.
        public bool WriteChunk(byte[] data, int size, ChunkType which = kComponentState)
        {
            if (Contains(which)) // already exists!
                return false;

            fixed (byte* _ = data)
            {
                Entry e = new();
                return BeginChunk(e, which) && Verify(stream.Write((IntPtr)_, size, out var z)) && EndChunk(e);
            }
        }

        //-------------------------------------------------------------
        // for storing and restoring the whole plug-in state (component and controller states)
        public bool SeekToComponentState()                            ///< Seeks to the begin of the Component State.
        {
            var e = GetEntry(kComponentState);
            return e != null && SeekTo(e.offset);
        }

        public bool StoreComponentState(IComponent component)        ///< Stores the component state (only one time).
        {
            if (Contains(kComponentState)) // already exists!
                return false;

            Entry e = new();
            return BeginChunk(e, kComponentState) && Verify(component.GetState(stream)) && EndChunk(e);
        }

        public bool StoreComponentState(IBStream componentStream)    ///< Stores the component state from stream (only one time).
        {
            if (Contains(kComponentState)) // already exists!
                return false;

            Entry e = new();
            return BeginChunk(e, kComponentState) && CopyStream(componentStream, stream) && EndChunk(e);
        }

        public bool RestoreComponentState(IComponent component)      ///< Restores the component state.
        {
            var e = GetEntry(kComponentState);
            if (e == null)
                return false;

            var readOnlyBStream = new ReadOnlyBStream(stream, e.offset, e.size);
            return Verify(component.SetState(readOnlyBStream));
        }

        public bool RestoreComponentState(IEditController editController)///< Restores the component state and apply it to the controller.
        {
            var e = GetEntry(kComponentState);
            if (e != null)
            {
                var readOnlyBStream = new ReadOnlyBStream(stream, e.offset, e.size);
                return Verify(editController.SetComponentState(readOnlyBStream));
            }
            return false;
        }

        public bool SeekToControllerState()                           ///< Seeks to the begin of the Controller State.
        {
            var e = GetEntry(kControllerState);
            return e != null && SeekTo(e.offset);
        }

        public bool StoreControllerState(IEditController editController)///< Stores the controller state (only one time).
        {
            if (Contains(kControllerState)) // already exists!
                return false;

            Entry e = new();
            return BeginChunk(e, kControllerState) && Verify(editController.GetState(stream)) &&
                   EndChunk(e);
        }

        public bool StoreControllerState(IBStream editStream)            ///< Stores the controller state from stream (only one time).
        {
            if (Contains(kControllerState)) // already exists!
                return false;

            Entry e = new();
            return BeginChunk(e, kControllerState) && CopyStream(editStream, stream) && EndChunk(e);
        }

        public bool RestoreControllerState(IEditController editController)///< Restores the controller state.
        {
            var e = GetEntry(kControllerState);
            if (e != null)
            {
                var readOnlyBStream = new ReadOnlyBStream(stream, e.offset, e.size);
                return Verify(editController.SetState(readOnlyBStream));
            }
            return false;
        }


        //--- ----------------------------------------------------------
        // Store program data or unit data from stream (including the header chunk).
        //\param inStream 
        //\param listID could be ProgramListID or UnitID. */
        public bool StoreProgramData(IBStream inStream, ProgramListID listID)
        {
            if (Contains(kProgramData)) // already exists!
                return false;

            WriteHeader();

            Entry e = new();
            if (BeginChunk(e, kProgramData))
                if (WriteInt32(listID))
                {
                    if (!CopyStream(inStream, stream))
                        return false;

                    return EndChunk(e);
                }
            return false;
        }

        //---when plug-in uses IProgramListData-----------------------
        // Stores a IProgramListData with a given identifier and index (including the header chunk).
        public bool StoreProgramData(IProgramListData programListData, ProgramListID programListID, int programIndex)
        {
            if (Contains(kProgramData)) // already exists!
                return false;

            WriteHeader();

            Entry e = new();
            return BeginChunk(e, kProgramData) && WriteInt32(programListID) &&
                   Verify(programListData.GetProgramData(programListID, programIndex, stream)) && EndChunk(e);
        }

        // Restores a IProgramListData with a given identifier and index.
        public bool RestoreProgramData(IProgramListData programListData, ProgramListID? programListID = null, int programIndex = 0)
        {
            var e = GetEntry(kProgramData);
            if (e != null && SeekTo(e.offset))
                if (ReadInt32(out var savedProgramListID))
                {
                    if (programListID.HasValue && programListID != savedProgramListID)
                        return false;

                    var alreadyRead = sizeof(int);
                    var readOnlyBStream = new ReadOnlyBStream(stream, e.offset + alreadyRead, e.size - alreadyRead);
                    return programListData != null && Verify(programListData.SetProgramData(savedProgramListID, programIndex, readOnlyBStream));
                }
            return false;
        }

        //---when plug-in uses IUnitData------------------------------
        // Stores a IUnitData with a given unitID (including the header chunk).
        public bool StoreProgramData(IUnitData unitData, UnitID unitID)
        {
            if (Contains(kProgramData)) // already exists!
                return false;

            WriteHeader();

            Entry e = new();
            return BeginChunk(e, kProgramData) && WriteInt32(unitID) &&
                   Verify(unitData.GetUnitData(unitID, stream)) && EndChunk(e);
        }

        // Restores a IUnitData with a given unitID (optional).
        public bool RestoreProgramData(IUnitData unitData, UnitID? unitID = null)
        {
            var e = GetEntry(kProgramData);
            if (e != null && SeekTo(e.offset))
                if (ReadInt32(out var savedUnitID))
                {
                    if (unitID.HasValue && unitID != savedUnitID)
                        return false;

                    var alreadyRead = sizeof(int);
                    var readOnlyBStream = new ReadOnlyBStream(stream, e.offset + alreadyRead, e.size - alreadyRead);
                    return unitData != null && Verify(unitData.SetUnitData(savedUnitID, readOnlyBStream));
                }
            return false;
        }

        //--- ----------------------------------------------------------
        // for keeping the controller part in sync concerning preset data stream, unitProgramListID could be ProgramListID or UnitID.
        public bool RestoreProgramData(IUnitInfo unitInfo, int unitProgramListID, int programIndex = -1)
        {
            var e = GetEntry(kProgramData);
            if (e != null && SeekTo(e.offset))
                if (ReadInt32(out var savedProgramListID))
                {
                    if (unitProgramListID != savedProgramListID)
                        return false;

                    var alreadyRead = sizeof(int);
                    var readOnlyBStream = new ReadOnlyBStream(stream, e.offset + alreadyRead, e.size - alreadyRead);
                    return unitInfo != null && unitInfo.SetUnitProgramData(unitProgramListID, programIndex, readOnlyBStream) != kResultOk;
                }
            return false;
        }

        // Gets the unitProgramListID saved in the kProgramData chunk (if available).
        public bool GetUnitProgramListID(out int unitProgramListID)
        {
            var e = GetEntry(kProgramData);
            if (e != null && SeekTo(e.offset))
                if (ReadInt32(out unitProgramListID))
                    return true;
            unitProgramListID = -1;
            return false;
        }

        //--- ---------------------------------------------------------------------
        // Shortcut helper to create preset from component/controller state. classID is the FUID of the component (processor) part.
        public static bool SavePreset(IBStream stream, Guid classID, IComponent component, IEditController editController = null, StringBuilder xmlBuffer = null, int xmlSize = -1)
        {
            PresetFile pf = new(stream);
            pf.SetClassID(classID);
            if (!pf.WriteHeader())
                return false;

            if (!pf.StoreComponentState(component))
                return false;

            if (editController != null && !pf.StoreControllerState(editController))
                return false;

            if (xmlBuffer != null && !pf.WriteMetaInfo(xmlBuffer, xmlSize))
                return false;

            return pf.WriteChunkList();
        }

        public static bool SavePreset(IBStream stream, Guid classID, IBStream componentStream, IBStream editStream = null, StringBuilder xmlBuffer = null, int xmlSize = -1)
        {
            PresetFile pf = new(stream);
            pf.SetClassID(classID);
            if (!pf.WriteHeader())
                return false;

            if (!pf.StoreComponentState(componentStream))
                return false;

            if (editStream != null && !pf.StoreControllerState(editStream))
                return false;

            if (xmlBuffer != null && !pf.WriteMetaInfo(xmlBuffer, xmlSize))
                return false;

            return pf.WriteChunkList();
        }

        // Shortcut helper to load preset with component/controller state. classID is the FUID of the component (processor) part.
        public static bool LoadPreset(IBStream stream, Guid classID, IComponent component, IEditController editController = null, List<Guid> otherClassIDArray = null)
        {
            PresetFile pf = new(stream);
            if (!pf.ReadChunkList())
                return false;

            if (pf.GetClassID() != classID)
            {
                if (otherClassIDArray != null)
                {
                    // continue to load only if found in supported ID else abort load
                    if (!otherClassIDArray.Contains(pf.GetClassID()))
                        return false;
                }
                else
                    return false;
            }

            if (!pf.RestoreComponentState(component))
                return false;

            if (editController != null)
            {
                // assign component state to controller
                if (!pf.RestoreComponentState(editController))
                    return false;

                // restore controller-only state (if present)
                if (pf.Contains(kControllerState) && !pf.RestoreControllerState(editController))
                    return false;
            }
            return true;
        }
    }

    /// <summary>
    /// Stream implementation for a file using stdio. 
    /// </summary>
    public unsafe class FileStream : IBStream
    {
        IOFileStream file;

        protected FileStream(IOFileStream file)
            => this.file = file;

        public void Dispose()
            => file?.Close();

        public static IBStream Open(string filename, FileMode mode) ///< open a stream using stdio function
        {
            var file = File.Open(filename, mode);
            return file != null ? new FileStream(file) : null;
        }

        //---from IBStream------------------
        public TResult Read(IntPtr buffer, int numBytes, out int numBytesRead)
        {
            var result = file.Read(new Span<byte>((void*)buffer, numBytes));
            numBytesRead = result;
            return result == numBytes ? kResultOk : kResultFalse;
        }

        public TResult Write(IntPtr buffer, int numBytes, out int numBytesWritten)
        {
            var result = numBytes; file.Write(new Span<byte>((void*)buffer, numBytes));
            numBytesWritten = result;
            return result == numBytes ? kResultOk : kResultFalse;
        }

        public TResult Seek(long pos, SeekOrigin mode, out long result)
        {
            if (file.Seek(pos, mode) == 0)
            {
                result = file.Position;
                return kResultOk;
            }
            result = default;
            return kResultFalse;
        }

        public TResult Tell(out long pos)
        {
            pos = file.Position;
            return kResultOk;
        }
    }

    /// <summary>
    /// Stream representing a Read-Only subsection of its source stream.
    /// </summary>
    public class ReadOnlyBStream : IBStream
    {
        IBStream sourceStream;
        TSize sourceOffset;
        TSize sectionSize;
        TSize seekPosition;

        public ReadOnlyBStream(IBStream sourceStream, TSize sourceOffset, TSize sectionSize)
        {
            this.sourceStream = sourceStream;
            this.sourceOffset = sourceOffset;
            this.sectionSize = sectionSize;
            //if (sourceStream) sourceStream->addRef();
        }

        //public void Dispose()
        //    => sourceStream?.Release();

        //TResult queryInterface (const TUID _iid, void** obj)
        //{
        // return sourceStream ? sourceStream->queryInterface (_iid, obj) : kResultFalse;
        //}

        //---from IBStream------------------
        public TResult Read(IntPtr buffer, int numBytes, out int numBytesRead)
        {
            numBytesRead = 0;

            if (sourceStream == null)
                return kNotInitialized;

            var maxBytesToRead = (int)(sectionSize - seekPosition);
            if (numBytes > maxBytesToRead)
                numBytes = maxBytesToRead;
            if (numBytes <= 0)
                return kResultOk;

            var result = sourceStream.Seek(sourceOffset + seekPosition, SeekOrigin.Begin, out var z);
            if (result != kResultOk)
                return result;

            result = sourceStream.Read(buffer, numBytes, out var numRead);

            if (numRead > 0)
                seekPosition += numRead;
            numBytesRead = numRead;

            return result;
        }

        public TResult Write(IntPtr buffer, int numBytes, out int numBytesWritten)
        {
            numBytesWritten = 0;
            return kNotImplemented;
        }

        public TResult Seek(long pos, SeekOrigin mode, out long result)
        {
            switch (mode)
            {
                case SeekOrigin.Begin: seekPosition = pos; break;
                case SeekOrigin.Current: seekPosition += pos; break;
                case SeekOrigin.End: seekPosition = sectionSize + pos; break;
            }

            if (seekPosition < 0)
                seekPosition = 0;
            if (seekPosition > sectionSize)
                seekPosition = sectionSize;

            result = seekPosition;
            return kResultOk;
        }

        public TResult Tell(out long pos)
        {
            pos = seekPosition;
            return kResultOk;
        }
    }

    /// <summary>
    /// Stream implementation for a memory buffer. 
    /// </summary>
    public unsafe class MemoryBStream : IBStream
    {
        MemoryStream mBuffer = new();

        //---from IBStream------------------
        public TResult Read(IntPtr buffer, int numBytes, out int numBytesRead)
        {
            var size = mBuffer.Read(new Span<byte>((void*)buffer, numBytes));
            numBytesRead = (int)size;

            return kResultTrue;
        }

        public TResult Write(IntPtr buffer, int numBytes, out int numBytesWritten)
        {
            try
            {
                mBuffer.Write(new Span<byte>((void*)buffer, numBytes));
                numBytesWritten = numBytes;
                return kResultTrue;
            }
            catch
            {
                numBytesWritten = 0;
                return kResultFalse;
            }
        }

        public TResult Seek(long pos, SeekOrigin mode, out long result)
        {
            var res = true;
            switch (mode)
            {
                //--- -----------------
                case SeekOrigin.Begin:
                    {
                        var tmp = pos;
                        if (tmp < 0)
                            tmp = 0;
                        mBuffer.Position = tmp;
                    }
                    break;

                //--- -----------------
                case SeekOrigin.Current:
                    {
                        var tmp = mBuffer.Position + pos;
                        if (tmp < 0)
                            tmp = 0;
                        mBuffer.Position = tmp;
                    }
                    break;

                //--- -----------------
                case SeekOrigin.End:
                    {
                        var tmp = mBuffer.Length - pos;
                        if (tmp < 0)
                            tmp = 0;
                        mBuffer.Position = tmp;
                    }
                    break;
            }
            result = res ? mBuffer.Position : 0;

            return res ? kResultTrue : kResultFalse;
        }

        public TResult Tell(out long pos)
        {
            pos = mBuffer.Position;
            return pos != 0 ? kResultTrue : kResultFalse;
        }
    }
}
