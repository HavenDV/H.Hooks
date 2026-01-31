using H.Hooks;

using var keyboardHook = new LowLevelKeyboardHook
{
    IsCapsLock = true,
    Handling = true,
    IsLeftRightGranularity = true,
    IsExtendedMode = true,
    HandleModifierKeys = true,
};
keyboardHook.Up += static (_, args) =>
{
    Console.WriteLine($"{nameof(keyboardHook)}.{nameof(keyboardHook.Up)}: All keys: {args.Keys}. Current key: {args.CurrentKey}. IsSimulator : {args.IsSimulator}");
};
keyboardHook.Down += static (_, args) =>
{
    Console.WriteLine($"{nameof(keyboardHook)}.{nameof(keyboardHook.Down)}: All keys: {args.Keys}. Current key: {args.CurrentKey}. IsSimulator : {args.IsSimulator}");
};
using var mouseHook = new LowLevelMouseHook
{
    Handling = true,
    AddKeyboardKeys = true,
    IsLeftRightGranularity = true,
    IsCapsLock = true,
    IsExtendedMode = true,
};
mouseHook.Up += static (_, args) =>
{
    Console.WriteLine($"{nameof(mouseHook)}.{nameof(mouseHook.Up)}: All keys: {args.Keys}. Current key: {args.CurrentKey}. IsSimulator : {args.IsSimulator}");
};
mouseHook.Down += static (_, args) =>
{
    Console.WriteLine($"{nameof(mouseHook)}.{nameof(mouseHook.Down)}: All keys: {args.Keys}. Current key: {args.CurrentKey}. IsSimulator : {args.IsSimulator}");
};

keyboardHook.Start();
mouseHook.Start();

Console.WriteLine("Press any key combination or `Escape` to exit.");
while (Console.ReadKey(true).Key != ConsoleKey.Escape)
{
}
