using System;
using System.Runtime.InteropServices;

namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// IContextMenuItem is an entry element of the context menu.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = Platform.CharacterSet, Pack = Platform.StructurePack)]
    public struct ContextMenuItem
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constants.Fixed128)] public String Name; // Name of the item
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
        Int32 GetItem(
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
        Int32 AddItem(
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
        Int32 RemoveItem(
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
        Int32 Popup(
            [MarshalAs(UnmanagedType.I4), In] Int32 x,
            [MarshalAs(UnmanagedType.I4), In] Int32 y);
    }

    static partial class Interfaces
    {
        public const string IContextMenu = "2E93C863-0C9C-4588-97DB-ECF5AD17817D";
    }
}
