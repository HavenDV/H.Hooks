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
    AddKeyboardKeys = true,
    IsLeftRightGranularity = true,
    IsCapsLock = true,
    IsExtendedMode = true,
}.WithEventLogging();

keyboardHook.Start();
mouseHook.Start();

Console.WriteLine("Press any key combination or `Escape` to exit.");
while (Console.ReadKey(true).Key != ConsoleKey.Escape)
{
}
