namespace H.Hooks.IntegrationTests;

[TestClass]
public class MouseTests
{
    [TestMethod]
    public async Task DefaultTest()
    {
        using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        var cancellationToken = cancellationTokenSource.Token;

        using var hook = new LowLevelMouseHook().WithEventLogging();

        hook.Start();

        await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
    }

    [TestMethod]
    public async Task HandlingTest()
    {
        using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        var cancellationToken = cancellationTokenSource.Token;

        using var hook = new LowLevelMouseHook
        {
            Handling = true,
        }.WithEventLogging();
        hook.Move += (_, args) => args.IsHandled = true;
        hook.Down += (_, args) => args.IsHandled = true;
        hook.Up += (_, args) => args.IsHandled = true;

        hook.Start();

        await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
    }

    [TestMethod]
    public async Task AddKeyboardKeysTest()
    {
        using var cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        var cancellationToken = cancellationTokenSource.Token;

        using var hook = new LowLevelMouseHook
        {
            AddKeyboardKeys = true,
            IsLeftRightGranularity = true,
            IsCapsLock = true,
            IsExtendedMode = true,
        }.WithEventLogging();

        hook.Start();

        await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);
    }
}
