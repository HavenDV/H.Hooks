using System;
using System.Threading.Tasks;
using H.Hooks;
using H.Tests.Extensions;

using var keyboardHook = new LowLevelKeyboardHook
{
    IsCapsLock = true,
    Handling = true,
    IsLeftRightGranularity = true,
    IsExtendedMode = true,
    HandleModifierKeys = true,
}.WithEventLogging();
using var mouseHook = new LowLevelMouseHook
{
    Handling = true,
}.WithEventLogging();

keyboardHook.Start();
mouseHook.Start();

while (Console.ReadKey(false).Key != ConsoleKey.Escape)
{
    await Task.Delay(TimeSpan.FromMilliseconds(1)).ConfigureAwait(false);
}
