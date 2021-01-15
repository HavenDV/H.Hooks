using System;
using System.ComponentModel;
using System.Threading;
using H.Hooks.Core.Interop;
using H.Hooks.Core.Interop.WinUser;
using H.Hooks.Extensions;

namespace H.Hooks
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class Hook : IDisposable
    {
        #region Properties

        /// <summary>
        /// If activated, you need to use <see cref="ThreadPool.QueueUserWorkItem(WaitCallback)"/>
        /// when handling events(After set up args.Handled = true).
        /// </summary>
        public bool HandlingIsEnabled { get; set; }

        /// <summary>
        /// Returns <see langword="true"/> if thread is started.
        /// </summary>
        public bool IsStarted => Thread != null;

        /// <summary>
        /// 
        /// </summary>
        protected bool PushToThreadPool => !HandlingIsEnabled;

        private int IdHook { get; }
        private Thread? Thread { get; set; }
        private uint Id { get; set; }

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
        /// <param name="idHook"></param>
        protected internal Hook(int idHook)
        {
            IdHook = idHook;
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
        protected abstract bool InternalCallback(int nCode, int wParam, nint lParam);

        #endregion

        #region Private methods

        private nint Callback(int nCode, int wParam, nint lParam)
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
                Id = Kernel32.GetCurrentThreadId();

                User32.PeekMessage(
                    out _, 
                    -1, 
                    WM.QUIT,
                    WM.QUIT, 
                    PM.NOREMOVE);

                var handle = User32.SetWindowsHookEx(IdHook, Callback, 0, 0).Check();

                while (true)
                {
                    var result = User32.GetMessage(
                        out var msg, 
                        -1,
                        WM.QUIT,
                        WM.QUIT);
                    if (result != 0 || 
                        msg.msg == WM.QUIT)
                    {
                        break;
                    }

                    User32.DefWindowProc(msg.handle, msg.msg, msg.wParam, msg.lParam);
                }

                User32.UnhookWindowsHookEx(handle);
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

            User32.PostThreadMessage(Id, WM.QUIT, 0, 0);
            Thread?.Join();
            Thread = null;
        }

        /// <summary>
        /// Dispose internal system hook resources.
        /// </summary>
        public void Dispose()
        {
            Stop();
        }

        #endregion
    }
}