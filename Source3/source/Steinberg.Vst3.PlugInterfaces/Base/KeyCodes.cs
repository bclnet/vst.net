using System;
using System.Runtime.CompilerServices;
using static Steinberg.Vst3.VirtualKeyCodes;

namespace Steinberg.Vst3
{
    public enum VirtualKeyCodes
    {
        KEY_BACK = 1,
        KEY_TAB,
        KEY_CLEAR,
        KEY_RETURN,
        KEY_PAUSE,
        KEY_ESCAPE,
        KEY_SPACE,
        KEY_NEXT,
        KEY_END,
        KEY_HOME,

        KEY_LEFT,
        KEY_UP,
        KEY_RIGHT,
        KEY_DOWN,
        KEY_PAGEUP,
        KEY_PAGEDOWN,

        KEY_SELECT,
        KEY_PRINT,
        KEY_ENTER,
        KEY_SNAPSHOT,
        KEY_INSERT,
        KEY_DELETE,
        KEY_HELP,
        KEY_NUMPAD0,
        KEY_NUMPAD1,
        KEY_NUMPAD2,
        KEY_NUMPAD3,
        KEY_NUMPAD4,
        KEY_NUMPAD5,
        KEY_NUMPAD6,
        KEY_NUMPAD7,
        KEY_NUMPAD8,
        KEY_NUMPAD9,
        KEY_MULTIPLY,
        KEY_ADD,
        KEY_SEPARATOR,
        KEY_SUBTRACT,
        KEY_DECIMAL,
        KEY_DIVIDE,
        KEY_F1,
        KEY_F2,
        KEY_F3,
        KEY_F4,
        KEY_F5,
        KEY_F6,
        KEY_F7,
        KEY_F8,
        KEY_F9,
        KEY_F10,
        KEY_F11,
        KEY_F12,
        KEY_NUMLOCK,
        KEY_SCROLL,

        KEY_SHIFT,
        KEY_CONTROL,
        KEY_ALT,

        KEY_EQUALS,				// only occurs on a Mac
        KEY_CONTEXTMENU,		// Windows only

        // multimedia keys
        KEY_MEDIA_PLAY,
        KEY_MEDIA_STOP,
        KEY_MEDIA_PREV,
        KEY_MEDIA_NEXT,
        KEY_VOLUME_UP,
        KEY_VOLUME_DOWN,

        KEY_F13,
        KEY_F14,
        KEY_F15,
        KEY_F16,
        KEY_F17,
        KEY_F18,
        KEY_F19,

        VKEY_FIRST_CODE = KEY_BACK,
        VKEY_LAST_CODE = KEY_F19,

        VKEY_FIRST_ASCII = 128
        /*
         KEY_0 - KEY_9 are the same as ASCII '0' - '9' (0x30 - 0x39) + FIRST_ASCII
         KEY_A - KEY_Z are the same as ASCII 'A' - 'Z' (0x41 - 0x5A) + FIRST_ASCII
        */
    }

    /// <summary>
    /// Utility functions to handle key-codes.
    /// </summary>
    public static partial class KeyCodes
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char VirtualKeyCodeToChar(byte vKey)
        {
            if (vKey >= (byte)VKEY_FIRST_ASCII)
                return (char)(vKey - VKEY_FIRST_ASCII + 0x30);
            else if (vKey == (byte)KEY_SPACE)
                return ' ';
            return (char)0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte CharToVirtualKeyCode(char character)
        {
            if ((character >= 0x30 && character <= 0x39) || (character >= 0x41 && character <= 0x5A))
                return (byte)(character - 0x30 + (byte)VKEY_FIRST_ASCII);
            if (character == ' ')
                return (byte)KEY_SPACE;
            return 0;
        }
    }

    /// <summary>
    /// OS-independent enumeration of virtual modifier-codes.
    /// </summary>
    public enum KeyModifier
    {
        ShiftKey = 1 << 0,          // same on both PC and Mac
        AlternateKey = 1 << 1,      // same on both PC and Mac
        CommandKey = 1 << 2,        // windows ctrl key; mac cmd key (apple button)
        ControlKey = 1 << 3         // windows: not assigned, mac: ctrl key
    }

    /// <summary>
    /// Simple data-struct representing a key-stroke on the keyboard.
    /// </summary>
    public struct KeyCode
    {
        /// <summary>
        /// The associated character.
        /// </summary>
        public char character;
        /// <summary>
        /// The associated virtual key-code.
        /// </summary>
        public byte virt;
        /// <summary>
        /// The associated virtual modifier-code.
        /// </summary>
        public byte modifier;

        /// <summary>
        /// Constructs a new KeyCode.
        /// </summary>
        /// <param name="character"></param>
        /// <param name="virt"></param>
        /// <param name="modifier"></param>
        public KeyCode(char character = (char)0, byte virt = 0, byte modifier = 0)
        {
            this.character = character;
            this.virt = virt;
            this.modifier = modifier;
        }
        public KeyCode(KeyCode other)
        {
            this.character = other.character;
            this.virt = other.virt;
            this.modifier = other.modifier;
        }
    }

    public static partial class KeyCodes
    {
        /// <summary>
        /// Is only a modifier pressed on the keyboard?
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsModifierOnlyKey(KeyCode key)
            => key.character == 0 && (key.virt == (byte)KEY_SHIFT || key.virt == (byte)KEY_ALT || key.virt == (byte)KEY_CONTROL);
    }
}
