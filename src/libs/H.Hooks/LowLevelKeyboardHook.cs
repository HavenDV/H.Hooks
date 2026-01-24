using H.Hooks.Core.Interop;
using H.Hooks.Extensions;

namespace H.Hooks;

/// <summary>
/// 
/// </summary>
#if NET5_0_OR_GREATER
[System.Runtime.Versioning.SupportedOSPlatform("windows5.1.2600")]
#elif NETSTANDARD2_0_OR_GREATER || NET451_OR_GREATER
#else
#error Target Framework is not supported
#endif
public sealed class LowLevelKeyboardHook : Hook
{
    #region Properties

    /// <summary>
    /// Sends multiple events while key pressed. <br/>
    /// Default value: <see langword="true"/>.
    /// </summary>
    public bool OneUpEvent { get; set; } = true;

    /// <summary>
    /// Allows handle modifier keys. <br/>
    /// Default value: <see langword="false"/>.
    /// </summary>
    public bool HandleModifierKeys { get; set; }

    /// <summary>
    /// Allows common key combinations, like 1 + 2 + 3. <br/>
    /// Default value: <see langword="false"/>.
    /// </summary>
    public bool IsExtendedMode { get; set; }

    /// <summary>
    /// Events will contains separate Left/Right keys. <br/>
    /// Default value: <see langword="false"/>.
    /// </summary>
    public bool IsLeftRightGranularity { get; set; }

    /// <summary>
    /// Uses User32.GetKeyboardState instead User32.GetKeyState. <br/>
    /// Disable this if any problem. <br/>
    /// Default value: <see langword="true"/>.
    /// </summary>
    public bool UseKeyboardState { get; set; } = true;

    /// <summary>
    /// Adds <see cref="Key.Caps"/> to each event if CapsLock is toggled. <br/>
    /// Default value: <see langword="true"/>.
    /// </summary>
    public bool IsCapsLock { get; set; }

    private Tuple<uint, KBDLLHOOKSTRUCT_FLAGS, uint, nuint, bool>? LastState { get; set; }

    #endregion

    #region Events

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler<KeyboardEventArgs>? Down;

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler<KeyboardEventArgs>? Up;

    private void OnDown(KeyboardEventArgs value)
    {
        Down?.Invoke(this, value, PushToThreadPool);
    }

    private void OnUp(KeyboardEventArgs value)
    {
        Up?.Invoke(this, value, PushToThreadPool);
    }

    #endregion

    #region Constructors

    /// <summary>
    /// 
    /// </summary>
    public LowLevelKeyboardHook() : base(WINDOWS_HOOK_ID.WH_KEYBOARD_LL)
    {
    }

    #endregion

    #region Protected methods

    /// <summary>
    /// 
    /// </summary>
    /// <param name="nCode"></param>
    /// <param name="wParam"></param>
    /// <param name="lParam"></param>
    /// <returns></returns>
    internal override unsafe bool InternalCallback(int nCode, WPARAM wParam, LPARAM lParam)
    {
        var value = (KBDLLHOOKSTRUCT*)lParam.Value;
        if (OneUpEvent &&
            LastState != null &&
            LastState.Item1 == value->vkCode &&
            LastState.Item2 == value->flags &&
            LastState.Item3 == value->scanCode &&
            LastState.Item4 == value->dwExtraInfo)
        {
            return LastState.Item5;
        }

        var keys = new List<Key>();
        var mainKey = (Key)value->vkCode;
        keys.Add(mainKey);

        keys.AddRange(
            Keyboard.GetPressedKeys(
                UseKeyboardState, 
                IsCapsLock, 
                IsLeftRightGranularity, 
                IsExtendedMode
                )
        );
        var flags = value->flags;

        var isSimulator =
            flags.HasFlag(KBDLLHOOKSTRUCT_FLAGS.LLKHF_INJECTED);
        var newKeys = new Keys(keys.Distinct().ToArray());
        var args = new KeyboardEventArgs(newKeys, mainKey,isSimulator);
        var isKeyDown = !value->flags.HasFlag(KBDLLHOOKSTRUCT_FLAGS.LLKHF_UP);
        if (isKeyDown)
        {
            OnDown(args);
        }
        else
        {
            OnUp(args);
        }

        var isHandled = args.IsHandled;

        // Disabling handling for modifier keys.
        // If you handle separate modifier keys, you can't handle combinations.
        if (!HandleModifierKeys &&
            keys.All(key => new[]
            {
                Key.LCtrl, Key.LAlt, Key.LShift, Key.LWin,
                Key.RCtrl, Key.RAlt, Key.RShift, Key.RWin,
                Key.Ctrl, Key.Alt, Key.Shift,
                Key.Caps,
            }.Contains(key)))
        {
            isHandled = false;
        }

        if (OneUpEvent)
        {
            LastState = Tuple.Create(
                value->vkCode,
                value->flags,
                value->scanCode,
                value->dwExtraInfo,
                isHandled);
        }

        return isHandled;
    }

    #endregion
}
