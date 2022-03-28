using System.ComponentModel;
using H.Hooks.Core.Interop;
using H.Hooks.Extensions;

namespace H.Hooks;

/// <summary>
/// Base class for all hooks.
/// </summary>
#if NET5_0_OR_GREATER
[System.Runtime.Versioning.SupportedOSPlatform("windows5.1.2600")]
#elif NETSTANDARD2_0_OR_GREATER || NET451_OR_GREATER
#else
#error Target Framework is not supported
#endif
public abstract class Hook : IDisposable
{
    #region Properties

    /// <summary>
    /// Allows you to intercept input for other applications and cancel events (via args.IsHandled = true). <br/>
    /// Do not enable this unless you need it. <br/>
    /// When enabled, overrides the automatic dispatch of events to the ThreadPool
    /// and may cause performance issues with any slow handlers. In this case,
    /// you need to use <see cref="ThreadPool.QueueUserWorkItem(WaitCallback)"/>
    /// when handling events (after set up args.IsHandled = true).
    /// </summary>
    public bool Handling { get; set; }

    /// <summary>
    /// Returns <see langword="true"/> if thread is started.
    /// </summary>
    public bool IsStarted { get; set; }

    /// <summary>
    /// See <see cref="Handling"/>.
    /// </summary>
    protected bool PushToThreadPool => !Handling;

    private WINDOWS_HOOK_ID Type { get; }
    private Thread? Thread { get; set; }
    private uint ThreadId { get; set; }
    private HOOKPROC Delegate { get; }

    #endregion

    #region Events

    /// <summary>
    /// 
    /// </summary>
    public event EventHandler<Exception>? ExceptionOccurred;

    private void OnExceptionOccurred(Exception value)
    {
        ExceptionOccurred?.Invoke(this, value, PushToThreadPool);
    }

    #endregion

    #region Constructors

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    internal Hook(WINDOWS_HOOK_ID type)
    {
        Type = type;
        Delegate = Callback;
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
    internal abstract bool InternalCallback(int nCode, WPARAM wParam, LPARAM lParam);

    #endregion

    #region Private methods

    private LRESULT Callback(int nCode, WPARAM wParam, LPARAM lParam)
    {
        try
        {
            if (nCode >= 0 && InternalCallback(nCode, wParam, lParam))
            {
                return new LRESULT(-1);
            }
        }
        catch (Exception exception)
        {
            OnExceptionOccurred(exception);
        }

        return PInvoke.CallNextHookEx(
            hhk: null,
            nCode: nCode,
            wParam: wParam,
            lParam: lParam);
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Starts hook thread.
    /// </summary>
    /// <exception cref="Win32Exception">If SetWindowsHookEx return error code</exception>
    public unsafe void Start()
    {
        if (Thread != null)
        {
            return;
        }

        Thread = new Thread(() =>
        {
            try
            {
                ThreadId = PInvoke.GetCurrentThreadId();

                var msg = new MSG();
                PInvoke.PeekMessage(
                    lpMsg: &msg,
                    hWnd: new HWND(-1),
                    wMsgFilterMin: 0,
                    wMsgFilterMax: 0,
                    wRemoveMsg: PEEK_MESSAGE_REMOVE_TYPE.PM_NOREMOVE);

                IsStarted = true;

                using var handle = PInvoke.SetWindowsHookEx(
                    idHook: Type,
                    lpfn: Delegate,
                    hmod: null,
                    dwThreadId: 0);

                while (true)
                {
                    PInvoke.GetMessage(
                        lpMsg: &msg,
                        hWnd: new HWND(-1),
                        wMsgFilterMin: 0,
                        wMsgFilterMax: 0).Check();

                    if (msg.message == PInvoke.WM_QUIT)
                    {
                        break;
                    }

                    _ = PInvoke.DefWindowProc(
                        hWnd: msg.hwnd,
                        Msg: msg.message,
                        wParam: msg.wParam,
                        lParam: msg.lParam);
                }
            }
            catch (Exception exception)
            {
                OnExceptionOccurred(exception);
            }
        })
        {
            IsBackground = true,
        };
        Thread.Start();
    }

    /// <summary>
    /// Stops hook thread.
    /// </summary>
    public void Stop()
    {
        if (Thread == null)
        {
            return;
        }

        while(!IsStarted)
        {
            Thread.Sleep(1);
        }

        PInvoke.PostThreadMessage(ThreadId, PInvoke.WM_QUIT, default, default).Check();
        Thread?.Join();
        Thread = null;
        IsStarted = false;
    }

    /// <summary>
    /// Dispose internal system hook resources.
    /// </summary>
    public void Dispose()
    {
        Stop();
        GC.SuppressFinalize(this);
    }

    #endregion
}
