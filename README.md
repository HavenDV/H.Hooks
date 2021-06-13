# [H.Hooks](https://github.com/HavenDV/H.Hooks/) 

[![Language](https://img.shields.io/badge/language-C%23-blue.svg?style=flat-square)](https://github.com/HavenDV/H.Hooks/search?l=C%23&o=desc&s=&type=Code) 
[![License](https://img.shields.io/github/license/HavenDV/H.Hooks.svg?label=License&maxAge=86400)](LICENSE.md) 
[![Requirements](https://img.shields.io/badge/Requirements-.NET%20Standard%202.0-blue.svg)](https://github.com/dotnet/standard/blob/master/docs/versions/netstandard2.0.md)
[![Build Status](https://github.com/HavenDV/H.Hooks/workflows/.NET/badge.svg?branch=master)](https://github.com/HavenDV/H.Hooks/actions?query=workflow%3A%22.NET%22)

Description

### Nuget

[![NuGet](https://img.shields.io/nuget/dt/H.Hooks.svg?style=flat-square&label=H.Hooks)](https://www.nuget.org/packages/H.Hooks/)

```
Install-Package H.Hooks
```

### Usage

```cs
using var keyboardHook = new LowLevelKeyboardHook();
keyboardHook.KeyUp += (_, args) => Console.WriteLine($"{nameof(keyboardHook.KeyUp)}: {args}");
keyboardHook.KeyDown += (_, args) => Console.WriteLine($"{nameof(keyboardHook.KeyDown)}: {args}");

keyboardHook.Start();

using var mouseHook = new LowLevelMouseHook();
mouseHook.Down += (_, args) => Console.WriteLine($"{nameof(mouseHook.Down)}: {args}");
mouseHook.Move += (_, args) => Console.WriteLine($"{nameof(mouseHook.Move)}: {args}");

mouseHook.Start();
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
hook.KeyUp += (_, args) => args.IsHandled = true;
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
