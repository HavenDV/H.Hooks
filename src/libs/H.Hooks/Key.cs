using System;

#pragma warning disable CS1591

// ReSharper disable UnusedMember.Global
// ReSharper disable CommentTypo
namespace H.Hooks
{
    /// <summary>
    /// https://github.com/tpn/winsdk-10/blob/master/Include/10.0.10240.0/um/WinUser.h#L437
    /// </summary>
    public enum Key
    {
        None,

        LButton = 0x01,
        LeftButton = LButton,
        RButton,
        RightButton = RButton,
        Cancel,
        MButton,            /* NOT contiguous with L & RBUTTON */

        XButton1 = 0x05,    /* NOT contiguous with L & RBUTTON */
        XButton2,           /* NOT contiguous with L & RBUTTON */

        Back = 0x08,
        Tab,

        LineFeed = 0x0A,

        Clear = 0x0C,
        Return,
        Enter = Return,

        Shift = 0x10,
        Control,
        Menu,
        Alt = Menu,
        Pause,
        Capital,
        CapsLock = Capital,

        KanaMode = 0x15,
        HanguelMode = KanaMode, /* old name - should be here for compatibility */
        HangulMode = KanaMode, 

        JunjaMode = 0x17,
        FinalMode,
        HanjaMode,
        KanjiMode = HanjaMode,

        Escape = 0x1B,

        Convert = 0x1C,
        ImeConvert = Convert,
        NonConvert,
        ImeNonConvert = NonConvert,
        Accept,
        ImeAccept = Accept,
        ModeChange,
        ImeModeChange = ModeChange,

        Space = 0x20,
        Prior,
        PageUp = Prior,
        Next,
        PageDown = Next,
        End,
        Home,
        Left,
        Up,
        Right,
        Down,
        Select,
        Print,
        Execute,
        Snapshot,
        PrintScreen = Snapshot,
        Insert,
        Delete,
        Help,

        D0 = 0x30,
        D1,
        D2,
        D3,
        D4,
        D5,
        D6,
        D7,
        D8,
        D9,

        A = 0x41,
        B,
        C,
        D,
        E,
        F,
        G,
        H,
        I,
        J,
        K,
        L,
        M,
        N,
        O,
        P,
        Q,
        R,
        S,
        T,
        U,
        V,
        W,
        X,
        Y,
        Z,

        LWin = 0x5B,
        LWindows = LWin,
        LeftWindows = LWin,
        RWin,
        RWindows = RWin,
        RightWindows = RWin,
        Apps,

        Sleep = 0x5F,

        NumPad0 = 0x60,
        NumPad1,
        NumPad2,
        NumPad3,
        NumPad4,
        NumPad5,
        NumPad6,
        NumPad7,
        NumPad8,
        NumPad9,
        Multiply,
        Add,
        Separator,
        Subtract,
        Decimal,
        Divide,
        F1,
        F2,
        F3,
        F4,
        F5,
        F6,
        F7,
        F8,
        F9,
        F10,
        F11,
        F12,
        F13,
        F14,
        F15,
        F16,
        F17,
        F18,
        F19,
        F20,
        F21,
        F22,
        F23,
        F24,

        NavigationView = 0x88,
        NavigationMenu,
        NavigationUp,
        NavigationDown,
        NavigationLeft,
        NavigationRight,
        NavigationAccept,
        NavigationCancel,

        NumLock = 0x90,
        Scroll,

        OemNecEqual = 0x92,   // '=' key on numpad

        OemFjJisho = 0x92,    // 'Dictionary' key
        OemFjMasshou,         // 'Unregister word' key
        OemFjTouroku,         // 'Register word' key
        OemFjLoya,            // 'Left OYAYUBI' key
        OemFjRoya,            // 'Right OYAYUBI' key

        LShift = 0xA0,
        LeftShift = LShift,
        RShift,
        RightShift = RShift,
        LControl,
        LeftControl = LControl,
        LCtrl = LControl,
        LeftCtrl = LControl,
        RControl,
        RightControl = RControl,
        RCtrl = RControl,
        RightCtrl = RControl,
        LMenu,
        LeftMenu = LMenu,
        LAlt = LMenu,
        LeftAlt = LMenu,
        RMenu,
        RightMenu = RMenu,
        RAlt = RMenu,
        RightAlt = RMenu,

        BrowserBack = 0xA6,
        BrowserForward,
        BrowserRefresh,
        BrowserStop,
        BrowserSearch,
        BrowserFavorites,
        BrowserHome,

        VolumeMute = 0xAD,
        VolumeDown,
        VolumeUp,
        MediaNextTrack,
        MediaPreviousTrack,
        MediaStop,
        MediaPlayPause,
        LaunchMail,
        SelectMedia,
        LaunchApp1,
        LaunchApplication1 = LaunchApp1,
        LaunchApp2,
        LaunchApplication2 = LaunchApp2,

        Oem1 = 0xBA,            // ';:' for US
        OemSemicolon = Oem1,    
        OemPlus,                // '+' any country
        OemComma,               // ',' any country
        OemMinus,               // '-' any country      
        OemPeriod,              // '.' any country
        Oem2,                   // '/?' for US
        OemQuestion = Oem2,
        Oem3,                   // '`~' for US
        OemTilde = Oem3,

        GamepadA = 0xC3,
        GamepadB,
        GamepadX,
        GamepadY,
        GamepadRightShoulder,
        GamepadLeftShoulder,
        GamepadLeftTrigger,
        GamepadRightTrigger,
        GamepadDPadUp,
        GamepadDPadDown,
        GamepadDPadLeft,
        GamepadDPadRight,
        GamepadMenu,
        GamepadView,
        GamepadLeftThumbStickButton,
        GamepadRightThumbStickButton,
        GamepadLeftThumbStickUp,
        GamepadLeftThumbStickDown,
        GamepadLeftThumbStickRight,
        GamepadLeftThumbStickLeft,
        GamepadRightThumbStickUp,
        GamepadRightThumbStickDown,
        GamepadRightThumbStickRight,
        GamepadRightThumbStickLeft,

        Oem4 = 0xDB,                //  '[{' for US
        OemOpenBrackets = Oem4,
        Oem5,                       //  '\|' for US
        OemPipe = Oem5,
        Oem6,                       //  ']}' for US
        OemCloseBrackets = Oem6,
        Oem7,                       //  ''"' for US
        OemQuotes = Oem7,
        Oem8,

        OemAx = 0xE1,               //  'AX' key on Japanese AX kbd
        Oem102,                     //  "<>" or "\|" on RT 102-key kbd.
        OemBackslash = Oem102,
        IcoHelp,                    //  Help key on ICO
        Ico00,                      //  00 key on ICO

        ProcessKey = 0xE5,

        IcoClear = 0xE6,

        Packet = 0xE7,

        OemReset = 0xE9,
        OemJump,
        OemPa1,
        OemPa2,
        OemPa3,
        OemWsCtrl,
        OemCuSel,
        OemAttn,
        OemFinish,
        OemCopy,
        OemAuto,
        OemEnLw,
        OemBackTab,

        Attn = 0xF6,
        CrSel,
        ExSel,
        ErEof,
        EraseEof = ErEof,
        Play,
        Zoom,
        NoName,
        Pa1,
        OemClear,
    }

    public static class KeyExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="text"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public static Key Parse(string text)
        {
            text = text ?? throw new ArgumentNullException(nameof(text));
            if (string.IsNullOrWhiteSpace(text))
            {
                return Key.None;
            }

            return Enum.TryParse<Key>(text, true, out var result)
                ? result
                : Key.None;
        }

        /// <summary>
        /// Returns fixed name if multiple enumeration members have the same underlying value.
        /// </summary>
        /// <param name="key"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public static string ToFixedString(this Key key)
        {
            return key switch
            {
                Key.LCtrl => nameof(Key.LCtrl),
                Key.LAlt => nameof(Key.LAlt),
                Key.LShift => nameof(Key.LShift),
                Key.LWin => nameof(Key.LWin),
                Key.RCtrl => nameof(Key.RCtrl),
                Key.RAlt => nameof(Key.RAlt),
                Key.RShift => nameof(Key.RShift),
                Key.RWin => nameof(Key.RWin),
                _ => $"{key:G}",
            };
        }
    }
}