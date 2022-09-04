using System;
using System.Runtime.InteropServices;

namespace Steinberg.Vst3
{
    /// <summary>
    /// Extended host callback interface Vst::IComponentHandler3 for an edit controller. 
    /// A plug-in can ask the host to create a context menu for a given exported parameter ID or a generic context menu.\n
    /// 
    /// The host may pre-fill this context menu with specific items regarding the parameter ID like "Show automation for parameter",
    /// "MIDI learn" etc...\n
    /// </summary>
    [ComImport, Guid(Interfaces.IComponentHandler3), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IComponentHandler3
    {
        /// <summary>
        /// Creates a host context menu for a plug-in:
		/// - If paramID is zero, the host may create a generic context menu.
		/// - The IPlugView object must be valid.
		/// - The return IContextMenu object needs to be released afterwards by the plug-in.
        /// </summary>
        /// <param name="plugView"></param>
        /// <param name="paramID"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Interface)]
        IContextMenu CreateContextMenu(
            [MarshalAs(UnmanagedType.Interface), In] IPlugView plugView,
            [MarshalAs(UnmanagedType.U4), In, Out] ref UInt32 paramID);
    }

    partial class Interfaces
    {
        public const string IComponentHandler3 = "69F11617-D26B-400D-A4B6-B9647B6EBBAB";
    }

    /// <summary>
    /// Context Menu Item Target interface: Vst::IContextMenuTarget
    /// A receiver of a menu item should implement this interface, which will be called after the user has selected
    /// this menu item.
    /// </summary>
    [ComImport, Guid(Interfaces.IContextMenuTarget), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IContextMenuTarget
    {
        /// <summary>
        /// Called when an menu item was executed.
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult ExecuteMenuItem(
            [MarshalAs(UnmanagedType.I4), In] Int32 tag);
    }

    partial class Interfaces
    {
        public const string IContextMenuTarget = "3CDF2E75-85D3-4144-BF86-D36BD7C4894D";
    }

    /// <summary>
    /// IContextMenuItem is an entry element of the context menu.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = Platform.CharacterSet, Pack = Platform.StructurePack)]
    public struct ContextMenuItem
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Platform.Fixed128)] public String Name; // Name of the item
        [MarshalAs(UnmanagedType.I4)] public Int32 Tag;						// Identifier tag of the item
        [MarshalAs(UnmanagedType.I4)] public ItemFlags Flags;				// Flags of the item

        public enum ItemFlags
        {
            IsSeparator = 1 << 0,				// Item is a separator
            IsDisabled = 1 << 1,				// Item is disabled
            IsChecked = 1 << 2,					// Item is checked
            IsGroupStart = 1 << 3 | IsDisabled,	// Item is a group start (like sub folder)
            IsGroupEnd = 1 << 4 | IsSeparator,	// Item is a group end
        }
    }

    /// <summary>
    /// Context Menu interface: Vst::IContextMenu
    /// A context menu is composed of Item (entry). A Item is defined by a name, a tag, a flag
    /// and a associated target(called when this item will be selected/executed). 
    /// With IContextMenu the plug-in can retrieve a Item, add a Item, remove a Item and pop-up the menu.
    /// </summary>
    [ComImport, Guid(Interfaces.IContextMenu), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IContextMenu
    {
        /// <summary>
        /// Gets the number of menu items.
        /// </summary>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.I4)]
        Int32 GetItemCount();

        /// <summary>
        /// Gets a menu item and its target (target could be not assigned).
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult GetItem(
            [MarshalAs(UnmanagedType.I4), In] Int32 index,
            [MarshalAs(UnmanagedType.Struct), In, Out] ref ContextMenuItem item,
            [MarshalAs(UnmanagedType.Interface), In, Out] ref IContextMenuTarget target);

        /// <summary>
        /// Adds a menu item and its target.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult AddItem(
            [MarshalAs(UnmanagedType.Struct), In] ref ContextMenuItem item,
            [MarshalAs(UnmanagedType.Interface), In] IContextMenuTarget target);

        /// <summary>
        /// Removes a menu item.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult RemoveItem(
            [MarshalAs(UnmanagedType.Struct), In] ref ContextMenuItem item,
            [MarshalAs(UnmanagedType.Interface), In] IContextMenuTarget target);

        /// <summary>
        /// Pop-ups the menu. Coordinates are relative to the top-left position of the plug-ins view.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        [PreserveSig]
        [return: MarshalAs(UnmanagedType.Error)]
        TResult Popup(
            [MarshalAs(UnmanagedType.I4), In] Int32 x,
            [MarshalAs(UnmanagedType.I4), In] Int32 y);
    }

    partial class Interfaces
    {
        public const string IContextMenu = "2E93C863-0C9C-4588-97DB-ECF5AD17817D";
    }
}
