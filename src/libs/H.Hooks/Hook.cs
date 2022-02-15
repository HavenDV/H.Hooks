using System.ComponentModel;
using H.Hooks.Core.Interop;
using H.Hooks.Core.Interop.WinUser;
using H.Hooks.Extensions;

namespace H.Hooks
{
    /// <summary>
    /// Base class for all hooks.
    /// </summary>
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

        private int Type { get; }
        private Thread? Thread { get; set; }
        private uint ThreadId { get; set; }
        private HookProc Delegate { get; }

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
        protected internal Hook(int type)
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
        protected abstract bool InternalCallback(int nCode, nint wParam, nint lParam);

        #endregion

        #region Private methods

        private nint Callback(int nCode, nint wParam, nint lParam)
        {
            try
            {
                if (nCode >= 0 && InternalCallback(nCode, wParam, lParam))
                {
                    return -1;
                }
            }
            catch (Exception exception)
            {
                OnExceptionOccurred(exception);
            }

            return User32.CallNextHookEx(0, nCode, wParam, lParam);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Starts hook thread.
        /// </summary>
        /// <exception cref="Win32Exception">If SetWindowsHookEx return error code</exception>
        public void Start()
        {
            if (Thread != null)
            {
                return;
            }

            Thread = new Thread(() =>
            {
                try
                {
                    ThreadId = Kernel32.GetCurrentThreadId();

                    User32.PeekMessage(
                        out _,
                        -1, 
                        0,
                        0, 
                        PM.NOREMOVE);

                    IsStarted = true;

                    var handle = User32.SetWindowsHookEx(Type, Delegate, 0, 0).Check();

                    try
                    {
                        while (true)
                        {
                            var result = User32.GetMessage(
                                out var msg,
                                -1,
                                0,
                                0);
                            if (result == -1)
                            {
                                InteropUtilities.ThrowWin32Exception();
                            }

                            if (msg.msg == WM.QUIT)
                            {
                                break;
                            }

                            User32.DefWindowProc(msg.handle, msg.msg, msg.wParam, msg.lParam);
                        }
                    }
                    finally
                    {
                        User32.UnhookWindowsHookEx(handle).Check();
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

            User32.PostThreadMessage(ThreadId, WM.QUIT, 0, 0).Check();
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
}