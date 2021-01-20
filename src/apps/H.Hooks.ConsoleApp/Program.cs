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

await Task.Delay(TimeSpan.FromHours(5));
