using System;
using System.ComponentModel;
using System.Threading;
using H.Hooks.Core.Interop;
using H.Hooks.Core.Interop.WinUser;
using H.Hooks.Extensions;

namespace H.Hooks
{
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

        protected bool PushToThreadPool => !HandlingIsEnabled;
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

        #region Protected methods

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
        /// Starts hook process.
        /// </summary>
        /// <exception cref="Win32Exception">If SetWindowsHookEx return error code</exception>
        internal void Start(HookProcedureType type)
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
                    WindowsMessages.WM_QUIT, 
                    WindowsMessages.WM_QUIT, 
                    PM.NOREMOVE);

                var handle = User32.SetWindowsHookEx(type, Callback, 0, 0).Check();

                while (true)
                {
                    var result = User32.GetMessage(
                        out var msg, 
                        -1,
                        WindowsMessages.WM_QUIT,
                        WindowsMessages.WM_QUIT);
                    if (result != 0 || 
                        msg.msg == WindowsMessages.WM_QUIT)
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

            User32.PostThreadMessage(Id, WindowsMessages.WM_QUIT, 0, 0);
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