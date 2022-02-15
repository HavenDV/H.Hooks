# [H.Hooks](https://github.com/HavenDV/H.Hooks/) 

[![Language](https://img.shields.io/badge/language-C%23-blue.svg?style=flat-square)](https://github.com/HavenDV/H.Hooks/search?l=C%23&o=desc&s=&type=Code) 
[![License](https://img.shields.io/github/license/HavenDV/H.Hooks.svg?label=License&maxAge=86400)](LICENSE.md) 
[![Requirements](https://img.shields.io/badge/Requirements-.NET%20Standard%202.0-blue.svg)](https://github.com/dotnet/standard/blob/master/docs/versions/netstandard2.0.md)
[![Build Status](https://github.com/HavenDV/H.Hooks/workflows/.NET/badge.svg?branch=master)](https://github.com/HavenDV/H.Hooks/actions?query=workflow%3A%22.NET%22)

It uses Win32 `kernel32.dll` and `user32.dll` calls inside.
Contains LowLevelKeyboardHook and LowLevelMouseHook.

Features:
- Global key handling and cancellation
- Allows handling combination like 1 + 2 + 3
- Only one Up event per combination
- Handle special buttons like Mouse.XButton
- Optimized, runs hooks in a separate thread. Does not cause freezes when debugging the rest of the code.
- By default, it delivers events from a ThreadPool instead of a hook thread, which makes it possible to do any action in event handlers without affecting system performance.

### Nuget

[![NuGet](https://img.shields.io/nuget/dt/H.Hooks.svg?style=flat-square&label=H.Hooks)](https://www.nuget.org/packages/H.Hooks/)

```
Install-Package H.Hooks
```

### Usage

```cs
using var keyboardHook = new LowLevelKeyboardHook();
keyboardHook.Up += (_, args) => Console.WriteLine($"{nameof(keyboardHook.Up)}: {args}");
keyboardHook.Down += (_, args) => Console.WriteLine($"{nameof(keyboardHook.Down)}: {args}");

keyboardHook.Start();

using var mouseHook = new LowLevelMouseHook();
mouseHook.Down += (_, args) => Console.WriteLine($"{nameof(mouseHook.Down)}: {args}");
mouseHook.Move += (_, args) => Console.WriteLine($"{nameof(mouseHook.Move)}: {args}");

mouseHook.Start();

// Check keys
if (args.Keys.Are(Key.Control, Key.Escape)) {
    // Exit?
}
```

### Interception of input and cancellation
Allows you to intercept input for other applications and cancel events (via `args.IsHandled = true`).  
Do not enable this unless you need it.  
When enabled, overrides the automatic dispatch of events to the ThreadPool
and may cause performance issues with any slow handlers. In this case,
you need to use `ThreadPool.QueueUserWorkItem(WaitCallback)`
when handling events (after set up `args.IsHandled = true`).

```cs
hook.Handling = true;
hook.Up += (_, args) => args.IsHandled = true;
```

### Advanced usage
```cs
// Enables Move events.
mouseHook.GenerateMouseMoveEvents = true;

// Adds keyboard keys. Allows getting combinations like Shift + LeftMouse.
mouseHook.AddKeyboardKeys = true;

// Sends multiple events while key pressed.
keyboardHook.OneUpEvent = false;

// Allows handle modifier keys.
keyboardHook.HandleModifierKeys = true;

// Allows common key combinations, like 1 + 2 + 3.
hook.IsExtendedMode = true;

// Events will contains separate Left/Right keys.
hook.IsLeftRightGranularity = true;

// Uses User32.GetKeyboardState instead User32.GetKeyState.
// Disable this if any problem.
hook.UseKeyboardState = false;

// Adds Key.Caps to each event if CapsLock is toggled.
hook.IsCapsLock = false;
```

### Contacts
* [mail](mailto:havendv@gmail.com)
