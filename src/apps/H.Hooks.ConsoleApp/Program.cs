using System;
using System.Threading.Tasks;
using H.Hooks;
using H.Tests.Extensions;

using var hook = new LowLevelKeyboardHook
{
    IsCapsLock = true,
    Handling = true,
    IsLeftRightGranularity = true,
    IsExtendedMode = true,
    HandleModifierKeys = true,
}.WithEventLogging();

hook.Start();

while (Console.ReadKey(false).Key != ConsoleKey.Escape)
{
    await Task.Delay(TimeSpan.FromMilliseconds(1));
}
